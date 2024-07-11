

using ScreenCapture.NET;

public class DirectXScreenCapture : IDisposable
{
    IScreenCaptureService ScreenCaptureService;
    IScreenCapture screenCapture;
    IEnumerable<Display> displays;
    IEnumerable<GraphicsCard> graphicsCards;
    ICaptureZone fullscreen;
    public DirectXScreenCapture()
    {
        Initialize();
    }

    private void Initialize()
    {
        ScreenCaptureService = new DX11ScreenCaptureService();
        graphicsCards = ScreenCaptureService.GetGraphicsCards();
        displays = ScreenCaptureService.GetDisplays(graphicsCards.First());
        screenCapture = ScreenCaptureService.GetScreenCapture(displays.First());

        fullscreen = screenCapture.RegisterCaptureZone(0, 0, screenCapture.Display.Width, screenCapture.Display.Height);


    }


    public IImage CaptureScreenRegion()
    {
        screenCapture.CaptureScreen();
        using (fullscreen.Lock()) {
            return fullscreen.Image;
        }
    }

    public void Dispose()
    {
    }
}