namespace MainApplication.Services;

internal class ToolService
{
    private static readonly ToolService Instance = new();

    public static ToolService GetInstance()
    {
        return Instance;
    }
    
    /// <summary>
    /// Is value Uri
    /// </summary>
    /// <param name="uri"></param>
    /// <returns>uri object or null if error</returns>
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
    
    /// <summary>
    /// Check if object is present into enum
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>object or null if error</returns>
    private static T? IsValidEnumInteger<T>(int obj)
    {
        if (Enum.IsDefined(typeof(T), obj))
            return (T) Enum.Parse(typeof(T), obj.ToString());
        return default;
    }

    /// <summary>
    /// Convert string to integer and integer to enum
    /// </summary>
    /// <param name="choiceString"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>object or null if error</returns>
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
    
    /// <summary>
    /// Get current timestamp
    /// </summary>
    /// <returns>timestamp</returns>
    public static long GetTimestamp()
    {
        return DateTime.Now.ToFileTime();
    }

    /// <summary>
    /// Convert bytes to string
    /// </summary>
    /// <param name="fileHash"></param>
    /// <returns>string</returns>
    public static string BytesToString(IEnumerable<byte> fileHash)
    {
        return fileHash.Aggregate("", (current, b) => current + b.ToString("x2"));
    }

    /// <summary>
    /// Compare two file with hash
    /// </summary>
    /// <param name="file1"></param>
    /// <param name="file2"></param>
    /// <returns>true if identical</returns>
    public static bool FileCompare(string file1, string file2)
    {
        int file1Byte, file2Byte;
        if (file1 == file2)
            return true;
        if (!Directory.Exists(Path.GetDirectoryName(file2)))
            return false;
        if (!File.Exists(file1) || !File.Exists(file2))
            return false;
        using var fs1 = new FileStream(file1, FileMode.Open, FileAccess.Read);
        using var fs2 = new FileStream(file2, FileMode.Open, FileAccess.Read);

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