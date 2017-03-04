using System;

[Serializable]
public class BaseMessage
{
    private byte _type;
	public byte Type
    {
		protected set
        {
            _type = value;
        }
        get
        {
            return _type;
        }
    }

    private uint _playerId;
    public uint PlayerId
    {
        set
        {
            _playerId = value;
        }
        get
        {
            return _playerId;
        }
    }
}
