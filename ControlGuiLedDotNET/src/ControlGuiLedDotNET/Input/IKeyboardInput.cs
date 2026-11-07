using System.Windows.Forms;

namespace ControlGuiLed.Input;

public interface IKeyboardInput
{
    void Press(Keys key);
}
