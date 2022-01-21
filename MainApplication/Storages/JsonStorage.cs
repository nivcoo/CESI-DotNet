namespace MainApplication.Storages;

public class JsonStorage : IStorage
{
    public string FileName { get; set; }

    public JsonStorage(string fileName)
    {
        FileName = fileName;
    }

    public List<IDictionary<string, object>> GetAllElements()
    {
        throw new NotImplementedException();
    }

    public void AddNewElement(IDictionary<string, object> objects)
    {
        throw new NotImplementedException();
    }

    public void EditElementByName(string name, IDictionary<string, object> objects)
    {
        throw new NotImplementedException();
    }
}