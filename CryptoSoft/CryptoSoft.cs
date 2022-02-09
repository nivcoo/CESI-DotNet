namespace CryptoSoft;


public class CryptoSoft
{
    public CryptoSoft(string readPath, string writePath, string keyCript)
    {
        Encrypt(readPath, writePath, keyCript);
    }
    public void Encrypt(string fileToRead, string fileToWrite, string keyCrypt)
    {
        var file = System.IO.File.ReadAllText(fileToRead);
    
        var count = 0;
        var textfile = "";
        foreach (var letter in file.Where(letter => keyCrypt != null))
        {
            if (keyCrypt == null) continue;
            textfile += (char)((int) keyCrypt[count] ^ (int) letter);

            count++;
            if (count == keyCrypt.Length) count = 0;
        }
        System.IO.File.WriteAllText(fileToWrite,textfile);
    }
}