using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// 玩家移动输入
/// </summary>
public class MovementInput : NetworkBehaviour
{

    void Update()
    {
        if (!isLocalPlayer) return;

        Vector2 inputDir = GetInputDirection();

        if (inputDir != Vector2.zero)
        {
            SendMoveMessage(inputDir);
        }
    }

    private Vector2 GetInputDirection()
    {
        float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float v = CrossPlatformInputManager.GetAxisRaw("Vertical");

        return new Vector2(h, v);
    }

    private void SendMoveMessage(Vector2 dir)
    {
        MoveMessage mmsg = new MoveMessage(GameManager.Instance.LocalPlayer.PlayerId, dir);

        MessageRequest.Instance.SendMessage(mmsg);
    }
}
