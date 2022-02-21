using MainApplication.Objects;

namespace MainApplication.Storages;

public abstract class AStorage<T>
{
    private readonly Mutex EditFilesMutex;

    protected AStorage(string filePath)
    {
        FilePath = filePath;
        EditFilesMutex = new Mutex();
    }

    protected string FilePath { get; set; }

    /// <summary>
    ///     Get an element that's been stored
    /// </summary>
    /// <returns></returns>
    public abstract T? GetElement();

    /// <summary>
    ///     Get the list of all the elements that've been stored
    /// </summary>
    /// <returns></returns>
    public abstract List<T> GetAllElements();

    /// <summary>
    ///     Write the current element into storage
    /// </summary>
    /// <param name="obj"></param>
    public abstract void WriteElement(T obj);

    /// <summary>
    ///     Add a new element into storage
    /// </summary>
    /// <param name="obj"></param>
    public abstract void AddNewElement(T obj);

    /// <summary>
    ///     Add an element into file without rewrite all the file
    /// </summary>
    /// <param name="obj"></param>
    public abstract void AddNewElementWithoutRewrite(T obj);

    /// <summary>
    ///     Delete an element from the file
    /// </summary>
    /// <param name="match"></param>
    public abstract void RemoveElement(Predicate<T> match);

    /// <summary>
    ///     Get a matching element
    /// </summary>
    /// <param name="match"></param>
    /// <returns>element or null if not found</returns>
    public abstract T? GetElementBy(Predicate<T> match);

    /// <summary>
    ///     Edit a matching elmement 
    /// </summary>
    /// <param name="match"></param>
    /// <param name="obj"></param>
    /// <returns>true if Success</returns>
    public abstract bool EditElementBy(Predicate<T> match, T obj);

    /// <summary>
    ///     Delete content from the file
    /// </summary>
    public abstract void ClearFile();

    public static implicit operator AStorage<T>(JsonStorage<Config> v)
    {
        throw new NotImplementedException();
    }

    protected void RunMutexAction(Action action)
    {
        try
        {
            EditFilesMutex.WaitOne(30000);
            action.Invoke();
        }
        catch (Exception)
        {
        }
        finally
        {
            EditFilesMutex.ReleaseMutex();
        }
    }

    protected object? RunMutexFunc(Func<object?> func)
    {
        try
        {
            EditFilesMutex.WaitOne(30000);
            var rtn = Task.Run(() => func.Invoke());
            rtn.Wait();
            return rtn.Result;
        }
        catch (Exception)
        {
        }
        finally
        {
            EditFilesMutex.ReleaseMutex();
        }
        return default;
    }
}
