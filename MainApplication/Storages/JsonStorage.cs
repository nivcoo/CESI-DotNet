namespace MainApplication.Storages;
/// <summary>
/// This manages all the actions related to the JSON files,  GET all the elements, ADD a new one or even EDIT.
/// </summary>
public class JsonStorage : IStorage
{
 
    public string FileName { get; set; } /// <summary>
                                         /// 
                                         /// </summary>
                                         /// <param name="fileName"></param>

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