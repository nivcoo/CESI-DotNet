namespace MainApplication.Storages;

public interface IStorage
{
    IDictionary<string, object> GetAllElements();

    void AddNewElement(IDictionary<string, object> objects);

    void EditElementByName(string name, IDictionary<string, object> objects);
}