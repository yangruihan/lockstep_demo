using UnityEngine;

/// <summary>
/// 角色移动组件
/// </summary>
public class MovementComponent : MonoBehaviour
{
    public void OnMoveMsgReceived(MoveMessage msg)
    {
        Debug.Log("OnMoveMsgReceived: " + msg.PlayerId + " (" + msg.X + ", " + msg.Y + ")");
    }
}
