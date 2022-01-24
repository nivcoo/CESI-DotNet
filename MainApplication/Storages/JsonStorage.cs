using System.Text.Json;
using System.Text.Json.Serialization;
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


    public void AddNewElement<T>(T objects)
    {
        var elementsList = GetAllElements<T>();
        elementsList.Add(objects);
        SaveIntoFiles(elementsList);
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
        SaveIntoFiles(elementsList);
        return true;
    }

    private void SaveIntoFiles<T>(List<T> objects)
    {
        var jsonToOutput = JsonSerializer.Serialize(objects, _serializerOptions);
        File.WriteAllText(FileName, jsonToOutput);
    }
}