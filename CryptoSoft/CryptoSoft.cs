namespace CryptoSoft;

public static class CryptoSoft
{

    /// <summary>
    /// Get current timestamp
    /// </summary>
    /// <returns>timestamp</returns>
    private static long GetTimestamp()
    {
        return DateTime.Now.ToFileTime();
    }

    public static long EncryptFile(string inputFile, string outputFile, string key)
    {

        using var fileIn = new FileStream(inputFile, FileMode.Open);
        using var fileOut = new FileStream(outputFile, FileMode.Create);

        var startTimestamp = GetTimestamp();
        var keyBytes = System.Text.Encoding.ASCII.GetBytes(key);
        var keyLen = keyBytes.Length;
        var buffer = new byte[4096];
        while (true)
        {
            var bytesRead = fileIn.Read(buffer);
            if (bytesRead == 0)
                break;
            for (var i = 0; i < bytesRead; i++)
                buffer[i] = (byte)(buffer[i] ^ keyBytes[i % keyLen]);
            fileOut.Write(buffer, 0, bytesRead);
        }

        fileIn.Close();
        fileOut.Close();
        var endTimestamp = GetTimestamp();
        return endTimestamp - startTimestamp;
    }
}