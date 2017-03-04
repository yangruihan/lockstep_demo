using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    #region Singleton
    private static DebugHelper _instance;

    public static DebugHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("_DebugHelper");
                _instance = obj.AddComponent<DebugHelper>();
            }

            return _instance;
        }
    }
    #endregion

    public void Log(string content)
    {
        Debug.Log(content);
    }

    public void LogError(string content)
    {
        Debug.LogError(content);
    }

    public void LogWarning(string content)
    {
        Debug.LogWarning(content);
    }
}
