namespace MainApplication.Storages;

public interface IStorage
{
    string FileName { get; set; }

    public List<T> GetAllElements<T>();

    void AddNewElement<T>(T obj);

    void AddNewElementWithoutRewrite<T>(T obj);

    T? GetElementBy<T>(Predicate<T> match);

    bool EditElementBy<T>(Predicate<T> match, T obj);

    void ClearFile();
}