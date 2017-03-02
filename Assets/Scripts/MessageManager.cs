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

    public void RegisteMessage(byte type, Action<BaseMessage> action)
    {
        _msgHandlerMap.Add(type, action);
    }

    public Action<BaseMessage> GetAction(byte type)
    {
        if (_msgHandlerMap.ContainsKey(type))
        {
            return _msgHandlerMap[type];
        }

        return null;
    }

    public MessageQueue GetMessages(long frameIdx)
    {
        if (WaitMsgs.ContainsKey(frameIdx))
        {
            return WaitMsgs[frameIdx];
        }

        return null;
    }

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
}