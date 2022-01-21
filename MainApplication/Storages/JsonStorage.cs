namespace MainApplication.Storages;

public class JsonStorage : IStorage
{
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