using UnityEngine;

/// <summary>
/// 角色移动组件
/// </summary>
public class MovementComponent : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void OnMoveMsgReceived(MoveMessage msg)
    {
        rigidBody.AddForce(new Vector2(msg.X, msg.Y));
    }
}
