namespace ControlGuiLed.Serial;

/// <summary>
/// Key codes sent by the controller. The numeric values are part of the
/// serial protocol.
/// </summary>
public enum RemoteKey : byte
{
    Power = 0,
    VolumeUp = 1,
    FuncStop = 2,
    Backwards = 3,
    PlayPause = 4,
    Forward = 5,
    Down = 6,
    VolumeDown = 7,
    Up = 8,
    Digit0 = 9,
    Equalizer = 10,
    StopRepeat = 11,
    Digit1 = 12,
    Digit2 = 13,
    Digit3 = 14,
    Digit4 = 15,
    Digit5 = 16,
    Digit6 = 17,
    Digit7 = 18,
    Digit8 = 19,
    Digit9 = 20,
    CancelRemoteControl = 21,
}
