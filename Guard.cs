namespace WebServer.Server.Common;

public static class Guard
{
    public static void AgainstNull(object value, string name = null)
    {
        if (value == null)
        {
            name ??= "Value";

            throw new ArgumentException($"{name} can't be null");
        }
    }
}