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

    public static long GetTimestamp()
    {
        return DateTime.Now.ToFileTime();
    }

    public static string BytesToString(IEnumerable<byte> fileHash)
    {
        return fileHash.Aggregate("", (current, b) => current + b.ToString("x2"));
    }

    public static bool FileCompare(string file1, string file2)
    {
        int file1Byte, file2Byte;
        if (file1 == file2)
            return true;
        var fs1 = new FileStream(file1, FileMode.Open);
        var fs2 = new FileStream(file2, FileMode.Open);

        if (fs1.Length != fs2.Length)
        {
            fs1.Close();
            fs2.Close();
            return false;
        }

        do
        {
            file1Byte = fs1.ReadByte();
            file2Byte = fs2.ReadByte();
        } while (file1Byte == file2Byte && file1Byte != -1);

        fs1.Close();
        fs2.Close();

        return file1Byte - file2Byte == 0;
    }
}