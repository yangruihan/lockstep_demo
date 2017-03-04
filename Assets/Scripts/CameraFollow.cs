using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject localRoleObj;

    private void Start()
    {
        SetLocalRoleObj();
    }

    private void Update()
    {
        if (localRoleObj != null)
        {
            transform.position = new Vector3(localRoleObj.transform.position.x, localRoleObj.transform.position.y, -10);
        }
        else
        {
            SetLocalRoleObj();
        }
    }

    private void SetLocalRoleObj()
    {
        if (GameManager.Instance.LocalPlayer != null)
        {
            localRoleObj = GameManager.Instance.LocalPlayer.RoleObj;
        }
    }
}
