namespace ControlGuiLed.Serial;

public static class RemoteKeyMap
{
    public static bool TryMap(byte code, out RemoteKey key)
    {
        if (code <= (byte)RemoteKey.CancelRemoteControl)
        {
            key = (RemoteKey)code;
            return true;
        }

        key = default;
        return false;
    }
}
