using ControlGuiLed.App;

namespace ControlGuiLed;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var controller = AppComposition.CreateController();
        using var mainForm = new MainForm(controller);

        Application.ApplicationExit += (_, _) => controller.Shutdown();
        Application.Run(mainForm);
    }
}
