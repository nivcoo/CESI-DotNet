using MainApplication.Objects;

namespace MainApplication.Storages;

public interface IStorage<T>
{
    string FilePath { get; set; }

    public List<T> GetAllElements();

    void AddNewElement(T obj);

    void AddNewElementWithoutRewrite(T obj);
    
    void RemoveElement(Predicate<T> match);

    T? GetElementBy(Predicate<T> match);

    bool EditElementBy(Predicate<T> match, T obj);

    void ClearFile();
}