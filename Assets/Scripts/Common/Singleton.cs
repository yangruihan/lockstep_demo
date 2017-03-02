/// <summary>
/// 单例抽象类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : class, new()
{
    private static object _lockObject = new object();
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                lock (_lockObject)
                {
                    if (_instance == null)
                        _instance = new T();
                }

            return _instance;
        }
    }

    public void Free()
    {
        _instance = null;
        FreeSingleton();
    }

    protected virtual void FreeSingleton() { }
}
