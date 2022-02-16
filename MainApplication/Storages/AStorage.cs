using MainApplication.Objects;

namespace MainApplication.Storages;

public abstract class AStorage<T>
{
    protected string FilePath { get; set; }

    private readonly Mutex EditFilesMutex;

    protected AStorage(string filePath)
    {
        FilePath = filePath;
        EditFilesMutex = new Mutex();
    }

    /// <summary>
    /// Get stored element
    /// </summary>
    /// <returns></returns>
    public abstract T? GetElement();

    /// <summary>
    /// Get list of all stored elements
    /// </summary>
    /// <returns></returns>
    public abstract List<T> GetAllElements();

    /// <summary>
    /// Write element into storage
    /// </summary>
    /// <param name="obj"></param>
    public abstract void WriteElement(T obj);

    /// <summary>
    /// Add new element into storage
    /// </summary>
    /// <param name="obj"></param>
    public abstract void AddNewElement(T obj);

    /// <summary>
    /// Add element into file without rewrite it
    /// </summary>
    /// <param name="obj"></param>
    public abstract void AddNewElementWithoutRewrite(T obj);
    
    /// <summary>
    /// Delete element of file
    /// </summary>
    /// <param name="match"></param>
    public abstract void RemoveElement(Predicate<T> match);

    /// <summary>
    /// Get element by match
    /// </summary>
    /// <param name="match"></param>
    /// <returns>element or null if not found</returns>
    public abstract T? GetElementBy(Predicate<T> match);

    /// <summary>
    /// Edit element bu match
    /// </summary>
    /// <param name="match"></param>
    /// <param name="obj"></param>
    /// <returns>true if Success</returns>
    public abstract bool EditElementBy(Predicate<T> match, T obj);

    /// <summary>
    /// Delete content of file
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