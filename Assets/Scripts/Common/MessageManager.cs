using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MessageManager : Singleton<MessageManager>
{
    #region 属性
    private static Dictionary<byte, Action<BaseMessage>> _msgHandlerMap = new Dictionary<byte, Action<BaseMessage>>();

    private List<BaseMessage> _msgBuffer = new List<BaseMessage>();
    public List<BaseMessage> MsgBuffer
    {
        get
        {
            return _msgBuffer;
        }
    }

    private Dictionary<long, MessageQueue> _frameMsgs = new Dictionary<long, MessageQueue>();
    public Dictionary<long, MessageQueue> FrameMsgs
    {
        get
        {
            return _frameMsgs;
        }
    }

    private Dictionary<long, MessageQueue> _waitMsgs = new Dictionary<long, MessageQueue>();
    public Dictionary<long, MessageQueue> WaitMsgs
    {
        get
        {
            return _waitMsgs;
        }
    }
    #endregion

	/// <summary>
	/// 注册消息
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="action">Action.</param>
    public void RegisteMessage(byte type, Action<BaseMessage> action)
    {
        _msgHandlerMap.Add(type, action);
    }

	/// <summary>
	/// 得到对应消息的回调函数
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="type">Type.</param>
    public Action<BaseMessage> GetAction(byte type)
    {
        if (_msgHandlerMap.ContainsKey(type))
        {
            return _msgHandlerMap[type];
        }

        return null;
    }

	/// <summary>
	/// 得到某一帧消息
	/// </summary>
	/// <returns>The messages.</returns>
	/// <param name="frameIdx">Frame index.</param>
    public MessageQueue GetMessages(long frameIdx)
    {
        if (WaitMsgs.ContainsKey(frameIdx))
        {
			return WaitMsgs [frameIdx];
        }
		else
		{
			
		}

        return null;
    }

	/// <summary>
	/// 序列化消息队列
	/// </summary>
	/// <returns>The message queue.</returns>
	/// <param name="msgQueue">Message queue.</param>
    public byte[] SerializeMessageQueue(MessageQueue msgQueue)
    {
        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, msgQueue);
        stream.Position = 0;
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        stream.Flush();
        stream.Close();

        return buffer;
    }

	/// <summary>
	/// 反序列化消息队列
	/// </summary>
	/// <returns>The message queue.</returns>
	/// <param name="bytes">Bytes.</param>
    public MessageQueue DesrializeMessageQueue(byte[] bytes)
    {
        MessageQueue msgQueue;

        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(bytes);
        msgQueue = (MessageQueue)formatter.Deserialize(stream);
        stream.Flush();
        stream.Close();

        return msgQueue;
    }

	/// <summary>
	/// 处理消息
	/// </summary>
	/// <param name="msg">Message.</param>
	public void HandleMessage(BaseMessage msg)
	{
		Action<BaseMessage> action = GetAction (msg.Type);

		if (action != null)
		{
			action (msg);
		}
	}

	public void RemoveWaitMessage(long frameIdx)
	{
		if (WaitMsgs.ContainsKey(frameIdx))
		{
			WaitMsgs.Remove (frameIdx);
		}	
	}
}