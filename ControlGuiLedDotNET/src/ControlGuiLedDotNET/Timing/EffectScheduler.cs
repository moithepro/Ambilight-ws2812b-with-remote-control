using System.Diagnostics;
using ControlGuiLed.Effects;
using Haukcode.HighResolutionTimer;

namespace ControlGuiLed.Timing;

/// <summary>
/// Runs the selected effect on a dedicated high-resolution timer thread.
/// </summary>
public sealed class EffectScheduler : IDisposable
{
    private readonly Lock _sync = new();

    private HighResolutionTimer? _timer;
    private Thread? _worker;
    private ILedEffect? _effect;
    private volatile bool _running;
    private bool _disposed;

    public void Start(ILedEffect effect, int intervalMs)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(intervalMs);

        lock (_sync)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            StopCore();

            _effect = effect;
            effect.Reset();

            _timer = new HighResolutionTimer();
            _timer.SetPeriod(intervalMs);

            _running = true;
            _timer.Start();

            _worker = new Thread(Run)
            {
                IsBackground = true,
                Name = "LED effect renderer",
            };
            _worker.Start();
        }
    }

    public void Stop()
    {
        lock (_sync)
        {
            StopCore();
        }
    }

    public void Dispose()
    {
        lock (_sync)
        {
            _disposed = true;
            StopCore();
        }
    }

    private void Run()
    {
        while (_running)
        {
            try
            {
                _effect?.Render();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"LED effect failed: {exception}");
            }

            if (!_running)
            {
                break;
            }

            try
            {
                _timer?.WaitForTrigger();
            }
            catch (ObjectDisposedException)
            {
                break;
            }
        }
    }

    private void StopCore()
    {
        _running = false;
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        if (_worker is not null && _worker != Thread.CurrentThread)
        {
            _worker.Join();
        }

        _worker = null;
        _effect = null;
    }
}
