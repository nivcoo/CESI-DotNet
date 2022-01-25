using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MainApplication.Storages.NamingPolicies;

namespace MainApplication.Storages;

public class JsonStorage : IStorage
{
    public string FileName { get; set; }

    public JsonStorage(string fileName)
    {
        FileName = fileName;
    }

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(new ToUpperNamingPolicy()),
        },
        WriteIndented = true
    };

    public List<T> GetAllElements<T>()
    {
        try
        {
            var text = File.ReadAllText(FileName);
            Console.WriteLine(text);
            var elementsList = JsonSerializer.Deserialize<List<T>>(text, _serializerOptions);
            return elementsList ?? new List<T>();
        }
        catch (Exception)
        {
            return new List<T>();
        }
    }


    public void AddNewElement<T>(T obj)
    {
        var elementsList = GetAllElements<T>();
        elementsList.Add(obj);
        SerializeAndSaveIntoFiles(elementsList);
    }

    public void AddNewElementWithoutRewrite<T>(T obj)
    {
        var objects = new List<T> {obj};
        using var fs = new FileStream(FileName, FileMode.Open);
        var serializeObject = SerializeObject(objects);
        using var sw = new StreamWriter(fs);
        if (fs.Length > 1)
        {
            fs.Position = fs.Seek(-1, SeekOrigin.End);

            if (fs.ReadByte() == ']')
            {
                fs.SetLength(fs.Length - 2);
                var serializeObjectWithoutFirst = Regex.Split(serializeObject, Environment.NewLine).Skip(1);
                serializeObject = string.Join(Environment.NewLine, serializeObjectWithoutFirst.ToArray());
                sw.Write("  }," + Environment.NewLine);
            }
        }

        sw.Write(serializeObject);
        sw.Close();
        fs.Close();
    }

    public T? GetElementBy<T>(Predicate<T> match)
    {
        var elementsList = GetAllElements<T>();
        return elementsList.Find(match);
    }

    public bool EditElementBy<T>(Predicate<T> match, T obj)
    {
        var elementsList = GetAllElements<T>();
        var selected = elementsList.Find(match);
        if (selected == null)
            return false;
        elementsList.Remove(selected);
        elementsList.Add(obj);
        SerializeAndSaveIntoFiles(elementsList);
        return true;
    }

    public void ClearFile()
    {
        File.WriteAllText(FileName, string.Empty);
    }

    private void SerializeAndSaveIntoFiles<T>(List<T> objects)
    {
        File.WriteAllText(FileName, SerializeObject(objects));
    }

    private string SerializeObject(object obj)
    {
        return JsonSerializer.Serialize(obj, _serializerOptions);
    }
}