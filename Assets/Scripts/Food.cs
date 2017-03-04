using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("eat by " + other.name);

        GameObjPool.Instance.Recycle(this.gameObject);
    }
}