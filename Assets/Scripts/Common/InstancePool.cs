using System.Collections.Generic;

/// <summary>
/// 可入对象池接口
/// </summary>
public interface IPoolable
{

    void OnAwakeFromPool();

    void OnReturnToPool();
}

/// <summary>
/// 对象池
/// </summary>
/// <typeparam name="T"></typeparam>
public class InstancePool<T> where T : IPoolable, new()
{
    protected Stack<T> _instances;

    public InstancePool(int preSize = 0)
    {
        _instances = new Stack<T>();
        for (int i = 0; i < preSize; i++)
        {
            T instance = default(T);
            _instances.Push((instance == null) ? new T() : instance);
        }
    }

    public T Get()
    {
        T instance;
        if (IsEmpty())
        {
            instance = default(T);
            instance = (instance == null) ? new T() : instance;
        }
        else
        {
            instance = _instances.Pop();
        }

        instance.OnAwakeFromPool();
        return instance;
    }

    public void Recycle(T instance)
    {
        if (_instances.Contains(instance))
        {
            instance = default(T);
        }
        else
        {
            instance.OnReturnToPool();
            _instances.Push(instance);
        }
    }

    private bool IsEmpty()
    {
        return _instances.Count == 0;
    }
}
