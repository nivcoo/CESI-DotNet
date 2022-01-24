namespace MainApplication.Storages;

public interface IStorage
{
    string FileName { get; set; }

    public List<T> GetAllElements<T>();

    void AddNewElement<T>(T objects);

    T? GetElementBy<T>(Predicate<T> match);
    bool EditElementBy<T>(Predicate<T> match, T obj);
}