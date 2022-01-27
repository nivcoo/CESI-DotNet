using MainApplication.Objects;

namespace MainApplication.Storages;

public abstract class AStorage<T>
{
    protected string FilePath { get; set; }
    
    public AStorage(string filePath)
    {
        FilePath = filePath;
    }

    public abstract List<T> GetAllElements();

    public abstract void AddNewElement(T obj);

    public abstract void AddNewElementWithoutRewrite(T obj);
    
    public abstract void RemoveElement(Predicate<T> match);

    public abstract T? GetElementBy(Predicate<T> match);

    public abstract bool EditElementBy(Predicate<T> match, T obj);

    public abstract void ClearFile();
}