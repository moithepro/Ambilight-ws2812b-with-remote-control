using System.Diagnostics;
using System.IO.Ports;

namespace ControlGuiLed.Serial;

/// <summary>
/// Keeps a serial port open, reconnects when it disappears, raises the
/// received remote-control keys, and serializes writes from multiple threads.
/// </summary>
public sealed class SerialConnection : IDisposable
{
    private static readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan DisposeTimeout = TimeSpan.FromSeconds(2);

    private readonly int _baudRate;
    private readonly Lock _writeLock = new();
    private readonly CancellationTokenSource _cancellation = new();

    private string _portName;
    private SerialPort? _port;
    private Task? _connectionTask;
    private int _connectionVersion;
    private volatile bool _isConnected;
    private bool _disposed;

    public SerialConnection(string portName, int baudRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(portName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(baudRate);

        _portName = portName;
        _baudRate = baudRate;
    }

    public event EventHandler<bool>? ConnectionChanged;

    public event EventHandler<RemoteKey>? RemoteKeyReceived;

    public bool IsConnected => _isConnected;

    public string PortName
    {
        get
        {
            lock (_writeLock)
            {
                return _portName;
            }
        }
    }

    /// <summary>
    /// Changes the port and reconnects. The current connection is closed so
    /// that the connection loop immediately starts over with the new name.
    /// </summary>
    public void ChangePort(string portName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(portName);
        ObjectDisposedException.ThrowIf(_disposed, this);

        lock (_writeLock)
        {
            if (string.Equals(_portName, portName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _portName = portName;
            _connectionVersion++;
            CloseCurrentPort();
        }
    }

    public void Start()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        _connectionTask ??= Task.Run(() => ConnectionLoopAsync(_cancellation.Token));
    }

    public void Write(byte[] data)
    {
        lock (_writeLock)
        {
            if (_port is not { IsOpen: true } port)
            {
                return;
            }

            try
            {
                port.Write(data, 0, data.Length);
            }
            catch (Exception exception) when (IsRecoverable(exception))
            {
                Debug.WriteLine($"Serial write failed: {exception.Message}");
            }
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _cancellation.Cancel();

        lock (_writeLock)
        {
            CloseCurrentPort();
        }

        try
        {
            _connectionTask?.Wait(DisposeTimeout);
        }
        catch (AggregateException)
        {
            // The connection loop is cancellable; nothing useful to report.
        }

        _cancellation.Dispose();
    }

    private async Task ConnectionLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            SerialPort? port = null;
            string portName;
            int connectionVersion;

            lock (_writeLock)
            {
                portName = _portName;
                connectionVersion = _connectionVersion;
            }

            try
            {
                port = new SerialPort(portName, _baudRate)
                {
                    Handshake = Handshake.None,
                    ReadTimeout = SerialPort.InfiniteTimeout,
                    WriteTimeout = 1000,
                };

                port.Open();

                var isCurrentConnection = false;
                lock (_writeLock)
                {
                    if (connectionVersion == _connectionVersion)
                    {
                        _port = port;
                        isCurrentConnection = true;
                    }
                }

                if (!isCurrentConnection)
                {
                    // The port changed while this attempt was opening.
                    continue;
                }

                SetConnected(true);
                await ReadLoopAsync(port, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception exception) when (IsRecoverable(exception))
            {
                Debug.WriteLine($"Serial connection to {_portName} failed: {exception.Message}");
            }
            finally
            {
                lock (_writeLock)
                {
                    if (ReferenceEquals(_port, port))
                    {
                        _port = null;
                    }
                }

                SetConnected(false);
                port?.Dispose();
            }

            try
            {
                await Task.Delay(ReconnectDelay, cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private async Task ReadLoopAsync(SerialPort port, CancellationToken cancellationToken)
    {
        var buffer = new byte[256];

        while (!cancellationToken.IsCancellationRequested)
        {
            var bytesRead = await port.BaseStream
                .ReadAsync(buffer, cancellationToken)
                .ConfigureAwait(false);

            if (bytesRead == 0)
            {
                break;
            }

            for (var index = 0; index < bytesRead; index++)
            {
                if (RemoteKeyMap.TryMap(buffer[index], out var key))
                {
                    RemoteKeyReceived?.Invoke(this, key);
                }
            }
        }
    }

    // Must be called while _writeLock is held.
    private void CloseCurrentPort()
    {
        try
        {
            _port?.Close();
        }
        catch (Exception exception) when (IsRecoverable(exception))
        {
            Debug.WriteLine($"Serial close failed: {exception.Message}");
        }

        _port = null;
    }

    private void SetConnected(bool connected)
    {
        if (_isConnected == connected)
        {
            return;
        }

        _isConnected = connected;
        ConnectionChanged?.Invoke(this, connected);
    }

    private static bool IsRecoverable(Exception exception) =>
        exception is IOException
            or UnauthorizedAccessException
            or ArgumentException
            or InvalidOperationException
            or TimeoutException
            or NotSupportedException;
}
