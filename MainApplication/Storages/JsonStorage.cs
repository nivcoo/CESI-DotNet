using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using MainApplication.Storages.NamingPolicies;

namespace MainApplication.Storages;

public class JsonStorage<T> : AStorage<T>
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter(new ToUpperNamingPolicy())
        },
        WriteIndented = true
    };

    public JsonStorage(string filePath) : base(filePath)
    {
    }

    public override T? GetElement()
    {
        var obj = RunMutexFunc(() =>
        {
            var text = File.ReadAllText(FilePath);

            if (text == "")
                return default;
            T? element = default;
            try
            {
                element = JsonSerializer.Deserialize<T>(text.Trim(), _serializerOptions);
            } catch { }

            return element ?? default;
        });

        return (T?) obj;
    }

    public override List<T> GetAllElements()
    {
        if (RunMutexFunc(() =>
            {
                var text = File.ReadAllText(FilePath);
                if (text == "")
                    return new List<T>();
                var elementsList = JsonSerializer.Deserialize<List<T>>(text.Trim(), _serializerOptions);
                return elementsList ?? new List<T>();
            }) is List<T> list)
            return list;
        return new List<T>();
    }

    public override void WriteElement(T obj)
    {
        if (obj != null)
            SerializeAndSaveIntoFiles(obj);
    }


    public override void AddNewElement(T obj)
    {
        var elementsList = GetAllElements();
        elementsList.Add(obj);
        SerializeAndSaveIntoFiles(elementsList);
    }

    public override void AddNewElementWithoutRewrite(T obj)
    {
        RunMutexAction(() =>
        {
            var objects = new List<T> {obj};
            using var fs = new FileStream(FilePath, FileMode.Open);
            var serializeObject = SerializeObject(objects);
            using var sw = new StreamWriter(fs);
            if (fs.Length > 1)
            {
                fs.Position = fs.Seek(-1, SeekOrigin.End);

                if (fs.ReadByte() == ']')
                {
                    fs.SetLength(fs.Length - 4 - Environment.NewLine.Length);
                    var serializeObjectWithoutFirst = Regex.Split(serializeObject, Environment.NewLine).Skip(1);
                    serializeObject = string.Join(Environment.NewLine, serializeObjectWithoutFirst.ToArray());
                    sw.Write("  }," + Environment.NewLine);
                }
            }
            sw.Write(serializeObject);
            sw.Close();
            fs.Close();
        });
    }

    public override void RemoveElement(Predicate<T> match)
    {
        var elementsList = GetAllElements();
        var element = elementsList.Find(match);
        if (element == null)
            return;
        elementsList.Remove(element);
        SerializeAndSaveIntoFiles(elementsList);
    }

    public override T? GetElementBy(Predicate<T> match)
    {
        var elementsList = GetAllElements();
        return elementsList.Find(match);
    }

    public override bool EditElementBy(Predicate<T> match, T obj)
    {
        var elementsList = GetAllElements();
        var selected = elementsList.Find(match);
        if (selected == null)
            return false;
        elementsList.Remove(selected);
        elementsList.Add(obj);
        SerializeAndSaveIntoFiles(elementsList);
        return true;
    }

    private void SerializeAndSaveIntoFiles(object obj)
    {
        RunMutexAction(() => File.WriteAllText(FilePath, SerializeObject(obj)));
    }

    private string SerializeObject(object obj)
    {
        return JsonSerializer.Serialize(obj, _serializerOptions);
    }
}
