using System.Xml;
using System.Xml.Serialization;

namespace MainApplication.Storages;

public class XmlStorage<T> : AStorage<T>

{
    public XmlStorage(string filePath) : base(filePath)
    {
    }

    public override T? GetElement()
    {
        var obj = RunMutexFunc(() =>
        {
            var serializer = new XmlSerializer(typeof(T));
            using Stream reader = new FileStream(FilePath, FileMode.Open);
            var elementsList = (T?)serializer.Deserialize(reader);
            return elementsList ?? default;
        });

        return (T?)obj;
    }

    public override List<T> GetAllElements()
    {
        if (RunMutexFunc(() =>
        {
            using Stream reader = new FileStream(FilePath, FileMode.Open);
            var serializer = new XmlSerializer(typeof(List<T>));
            var elementsList = new List<T>();
            try
            {
                elementsList = (List<T>?)serializer.Deserialize(reader);
            } catch { }


            return elementsList;
        }) is List<T> list)
            return list;
        return new List<T>();
    }

    public override void WriteElement(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        if (obj != null)
            SerializeAndSaveIntoFiles(serializer, obj);
    }

    public override void AddNewElement(T obj)
    {
        var elementsList = GetAllElements();
        elementsList.Add(obj);
        var serializer = new XmlSerializer(typeof(List<T>));
        SerializeAndSaveIntoFiles(serializer, elementsList);
    }

    public override void AddNewElementWithoutRewrite(T obj)
    {
        AddNewElement(obj);
    }

    public override void RemoveElement(Predicate<T> match)
    {
        var elementsList = GetAllElements();
        var element = elementsList.Find(match);
        if (element == null)
            return;
        elementsList.Remove(element);
        var serializer = new XmlSerializer(typeof(List<T>));
        SerializeAndSaveIntoFiles(serializer, elementsList);
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
        var serializer = new XmlSerializer(typeof(List<T>));
        SerializeAndSaveIntoFiles(serializer, elementsList);
        return true;
    }

    private void SerializeAndSaveIntoFiles(XmlSerializer serializer, object obj)
    {
        RunMutexAction(() =>
        {
            using var writer = new StreamWriter(FilePath);
            using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = true });
            serializer.Serialize(xmlWriter, obj);
            xmlWriter.Close();
            writer.Close();

        });
    }
}