using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    #region Singleton
    private static FoodManager _instance;

    public static FoodManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public float spawnFoodInterval = 5f;
    private float spawnFoodTimer = 0f;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        spawnFoodTimer = spawnFoodTimer + Time.deltaTime;
        if (spawnFoodTimer >= spawnFoodInterval)
        {
            spawnFoodTimer = 0;
            GameObject food = GameObjPool.Instance.Get("Food");
            food.transform.position = GetRandomPosition();
            food.SetActive(true);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 pos = new Vector3(
            RandomHelper.Instance.GetRandomInt(GameManager.Instance.gameWidth),
            RandomHelper.Instance.GetRandomInt(GameManager.Instance.gameHeight),
            1);
        return pos;
    }
}
