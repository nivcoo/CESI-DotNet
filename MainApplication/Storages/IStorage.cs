namespace MainApplication.Storages;

public interface IStorage
{
    string FileName { get; set; } 

    List<IDictionary<string, object>> GetAllElements();  
    /// <param name="objects"></param>
    
    void AddNewElement(IDictionary<string, object> objects);
    /// <param name="name"></param>
    /// <param name="objects"></param>
                                                            

    void EditElementByName(string name, IDictionary<string, object> objects);
}