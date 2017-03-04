using System;

[Serializable]
public class StringMessage : BaseMessage {

	private string _content;
	public string Content {
		set
		{
			_content = value;
		}
		get
		{
			return _content;
		}
	}

	public StringMessage()
	{
		this.Type = MessageType.STRING_MSG;
	}
}
