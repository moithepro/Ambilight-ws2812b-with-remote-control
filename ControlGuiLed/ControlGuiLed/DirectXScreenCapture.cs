using ControlGuiLed.Properties;
using NAudio.CoreAudioApi;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

public class DirectXScreenCapture : IDisposable
{
    private SharpDX.Direct3D11.Device device;
    private Texture2D screenTexture;
    private OutputDuplication outputDuplication;

    public DirectXScreenCapture()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Create factory and adapter
        using (var factory = new Factory1())
        {
            var adapter = factory.GetAdapter1(0);
            var output = adapter.GetOutput(0);
            var output1 = output.QueryInterface<Output1>();

            // Create the Direct3D11 device
            device = new SharpDX.Direct3D11.Device(adapter, DeviceCreationFlags.BgraSupport);

            // Ensure the device is compatible with the output
            if (output1.Description.DesktopBounds.Right == 0 || output1.Description.DesktopBounds.Bottom == 0)
            {
                throw new InvalidOperationException("Output has invalid desktop bounds.");
            }

            // Attempt to duplicate the output
            try
            {
                outputDuplication = output1.DuplicateOutput(device);
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode == Result.InvalidArg)
                {
                    throw new InvalidOperationException("Failed to duplicate output. Invalid arguments provided.", ex);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    private void CreateScreenTexture(int width, int height)
    {
        var textureDesc = new Texture2DDescription
        {
            CpuAccessFlags = CpuAccessFlags.Read,
            BindFlags = BindFlags.None,
            Format = Format.B8G8R8A8_UNorm,
            Width = width,
            Height = height,
            OptionFlags = ResourceOptionFlags.None,
            MipLevels = 1,
            ArraySize = 1,
            SampleDescription = { Count = 1, Quality = 0 },
            Usage = ResourceUsage.Staging
        };

        screenTexture?.Dispose();
        screenTexture = new Texture2D(device, textureDesc);
    }

    public byte[,,] CaptureScreenRegion(int x, int y, int width, int height)
    {
        var factory = new Factory1();
        var adapter = factory.GetAdapter1(0);
        var output = adapter.GetOutput(0);
        var output1 = output.QueryInterface<Output1>();
        outputDuplication = output1.DuplicateOutput(device);

        CreateScreenTexture(width, height);

        SharpDX.DXGI.Resource screenResource;
        OutputDuplicateFrameInformation duplicateFrameInformation;

        outputDuplication.AcquireNextFrame(500, out duplicateFrameInformation, out screenResource);
        using (var screenTexture2D = screenResource.QueryInterface<Texture2D>())
        {
            var region = new ResourceRegion(x, y, 0, x + width, y + height, 1);
            device.ImmediateContext.CopySubresourceRegion(screenTexture2D, 0, region, screenTexture, 0);
            var dataBox = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, SharpDX.Direct3D11.MapFlags.None);

            byte[,,] imageData = new byte[height, width, 4];

            var sourcePtr = dataBox.DataPointer;
            int rowPitch = dataBox.RowPitch;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int pixelIndex = col * 4;
                    imageData[row, col, 0] = Marshal.ReadByte(sourcePtr, pixelIndex);     // Blue
                    imageData[row, col, 1] = Marshal.ReadByte(sourcePtr, pixelIndex + 1); // Green
                    imageData[row, col, 2] = Marshal.ReadByte(sourcePtr, pixelIndex + 2); // Red
                    imageData[row, col, 3] = Marshal.ReadByte(sourcePtr, pixelIndex + 3); // Alpha
                }
                sourcePtr = IntPtr.Add(sourcePtr, rowPitch);
            }

            device.ImmediateContext.UnmapSubresource(screenTexture, 0);
            screenResource.Dispose();
            outputDuplication.ReleaseFrame();

            return imageData;
        }
    }

    public void Dispose()
    {
        outputDuplication?.Dispose();
        screenTexture?.Dispose();
        device?.Dispose();
    }
}
