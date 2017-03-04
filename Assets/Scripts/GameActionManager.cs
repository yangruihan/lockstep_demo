using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionManager : MonoBehaviour
{
    private void Awake()
    {
        RegisteActionHanler();
    }

    private void RegisteActionHanler()
    {
        Debug.Log("RegisteActionHanler");

        MessageManager.Instance.RegisteMessage(MessageType.MOVE_MSG, OnMoveMessageReceived);
    }

    private void OnMoveMessageReceived(BaseMessage msg)
    {
        MoveMessage mmsg = msg as MoveMessage;
        uint playerId = mmsg.PlayerId;

        MovementComponent movementComponent = GameManager.Instance.GetPlayerComponent<MovementComponent>(playerId);
        if (movementComponent != null)
        {
            movementComponent.OnMoveMsgReceived(mmsg);
        }
    }
}
