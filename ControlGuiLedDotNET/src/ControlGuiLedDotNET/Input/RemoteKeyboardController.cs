using System.Windows.Forms;
using ControlGuiLed.Serial;

namespace ControlGuiLed.Input;

/// <summary>
/// Translates remote-control key presses into PC keyboard input.
/// The power key toggles a mode in which the number keys act as navigation.
/// </summary>
public sealed class RemoteKeyboardController(IKeyboardInput keyboard)
{
    private bool _numberKeysAsNavigation;

    public void Handle(RemoteKey key)
    {
        switch (key)
        {
            case RemoteKey.Power:
                _numberKeysAsNavigation = !_numberKeysAsNavigation;
                break;

            case RemoteKey.VolumeUp:
                keyboard.Press(Keys.VolumeUp);
                break;

            case RemoteKey.FuncStop:
                keyboard.Press(Keys.Escape);
                break;

            case RemoteKey.Backwards:
                keyboard.Press(Keys.MediaPreviousTrack);
                break;

            case RemoteKey.PlayPause:
                keyboard.Press(Keys.MediaPlayPause);
                break;

            case RemoteKey.Forward:
                keyboard.Press(Keys.MediaNextTrack);
                break;

            case RemoteKey.Down:
                keyboard.Press(Keys.Down);
                break;

            case RemoteKey.VolumeDown:
                keyboard.Press(Keys.VolumeDown);
                break;

            case RemoteKey.Up:
                keyboard.Press(Keys.Up);
                break;

            case RemoteKey.Equalizer:
                keyboard.Press(Keys.Tab);
                break;

            case RemoteKey.StopRepeat:
                keyboard.Press(Keys.Enter);
                break;

            case RemoteKey.Digit0:
                PressDigit(Keys.D0);
                break;

            case RemoteKey.Digit1:
                PressDigit(Keys.D1);
                break;

            case RemoteKey.Digit2:
                PressDigit(Keys.D2);
                break;

            case RemoteKey.Digit3:
                PressDigit(Keys.D3);
                break;

            case RemoteKey.Digit4:
                keyboard.Press(_numberKeysAsNavigation ? Keys.Left : Keys.D4);
                break;

            case RemoteKey.Digit5:
                PressDigit(Keys.D5);
                break;

            case RemoteKey.Digit6:
                keyboard.Press(_numberKeysAsNavigation ? Keys.Right : Keys.D6);
                break;

            case RemoteKey.Digit7:
                PressDigit(Keys.D7);
                break;

            case RemoteKey.Digit8:
                PressDigit(Keys.D8);
                break;

            case RemoteKey.Digit9:
                PressDigit(Keys.D9);
                break;

            case RemoteKey.CancelRemoteControl:
            default:
                break;
        }
    }

    private void PressDigit(Keys key)
    {
        if (!_numberKeysAsNavigation)
        {
            keyboard.Press(key);
        }
    }
}
