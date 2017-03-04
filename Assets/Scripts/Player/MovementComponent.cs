using UnityEngine;

/// <summary>
/// 角色移动组件
/// </summary>
public class MovementComponent : MonoBehaviour
{
    public float accelerationForce = 1.5f;

    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void OnMoveMsgReceived(MoveMessage msg)
    {
        rigidBody.AddForce(new Vector2(msg.X * accelerationForce, msg.Y * accelerationForce));
    }
}
