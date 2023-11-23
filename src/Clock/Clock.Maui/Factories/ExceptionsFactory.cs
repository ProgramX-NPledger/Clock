namespace Clock.Maui.Factories;

public class ExceptionsFactory
{
    public static NotSupportedException CreateDeviceNotSupportedException()
    {
        return new NotSupportedException($"The current platform is not supported: {DeviceInfo.Current.Platform}");
    }
}