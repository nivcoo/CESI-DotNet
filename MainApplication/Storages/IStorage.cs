namespace MainApplication.Storages;

public interface IStorage<T>
{
    string FileName { get; set; }

    public List<T> GetAllElements();

    void AddNewElement(T obj);

    void AddNewElementWithoutRewrite(T obj);

    T? GetElementBy(Predicate<T> match);

    bool EditElementBy(Predicate<T> match, T obj);

    void ClearFile();
}