using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int initAmountToPool;
}

public class GameObjPool : MonoBehaviour
{
    private static GameObjPool _instance;
    public static GameObjPool Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<ObjectPoolItem> itemsToPool;

    private Dictionary<string, GameObject> itemsTagMap = null;

    private Dictionary<string, Stack<GameObject>> pooledObjects = null;

    void Awake()
    {
        if (Instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        InitPool();
    }

    /// <summary>
    /// 得到对象池中的对象
    /// </summary>
    /// <returns></returns>
    public GameObject Get(string tag)
    {
        if (pooledObjects.ContainsKey(tag))
        {
            GameObject obj = null;
            var stack = pooledObjects[tag];
            if (stack.Count > 0)
            {
                obj = stack.Pop();
            }
            else
            {
                obj = Instantiate<GameObject>(obj);
                obj.SetActive(false);
            }
            return obj;
        }

        return null;
    }

    /// <summary>
    /// 回收对象
    /// </summary>
    /// <param name="obj"></param>
    public void Recycle(GameObject obj)
    {
        obj.SetActive(false);

        if (pooledObjects.ContainsKey(obj.tag))
        {
            var stack = pooledObjects[obj.tag];
            if (stack.Contains(obj))
            {
                obj = null;
            }
            else
            {
                stack.Push(obj);
            }
        }
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    private void InitPool()
    {
        itemsTagMap = new Dictionary<string, GameObject>();
        pooledObjects = new Dictionary<string, Stack<GameObject>>();
        foreach (var item in itemsToPool)
        {
            var count = item.initAmountToPool;
            var obj = item.objectToPool;

            itemsTagMap.Add(obj.tag, obj);

            for (int i = 0; i < count; i++)
            {
                var obj2 = Instantiate<GameObject>(obj);
                obj2.SetActive(false);

                if (pooledObjects.ContainsKey(obj2.tag))
                {
                    pooledObjects[obj2.tag].Push(obj2);
                }
                else
                {
                    Stack<GameObject> stack = new Stack<GameObject>();
                    stack.Push(obj2);
                    pooledObjects.Add(obj2.tag, stack);
                }
            }
        }
    }
}