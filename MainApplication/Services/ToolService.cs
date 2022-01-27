namespace MainApplication.Services;

internal class ToolService
{
    private static readonly ToolService Instance = new();

    public static ToolService GetInstance()
    {
        return Instance;
    }

    public static Uri? IsValidUri(string? uri)
    {
        if (uri == null)
            return null;
        try
        {
            return new Uri(uri);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static T? IsValidEnumInteger<T>(int obj)
    {
        if (Enum.IsDefined(typeof(T), obj))
            return (T) Enum.Parse(typeof(T), obj.ToString());
        return default;
    }

    public static T? ConvertStringIntegerToEnum<T>(string? choiceString)
    {
        if (choiceString == null)
            return default;
        try
        {
            var convertedChoice = Convert.ToInt32(choiceString);
            return IsValidEnumInteger<T>(convertedChoice);
        }
        catch (FormatException)
        {
            return default;
        }
    }
}