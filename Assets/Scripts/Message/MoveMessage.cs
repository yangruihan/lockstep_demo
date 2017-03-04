using System;
using UnityEngine;

[Serializable]
public class MoveMessage : BaseMessage
{
    int rate = 100;
    private int _x;
    public float X
    {
        set
        {
            _x = (int)(value * rate);
        }
        get
        {
            return _x / rate * 1.0f;
        }
    }
    private int _y;
    public float Y
    {
        set
        {
            _y = (int)(value * rate);
        }
        get
        {
            return _y / rate * 1.0f;
        }
    }

    public MoveMessage(uint playerId, float x, float y)
    {
        Type = MessageType.MOVE_MSG;
        this.PlayerId = playerId;
        this.X = x;
        this.Y = y;
    }

    public MoveMessage(uint playerId, Vector2 dir) : this(playerId, dir.x, dir.y)
    {
    }
}