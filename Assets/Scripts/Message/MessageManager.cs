using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MessageManager : Singleton<MessageManager>
{
    #region 属性
    private static Dictionary<byte, List<Action<BaseMessage>>> _msgHandlerMap = new Dictionary<byte, List<Action<BaseMessage>>>();

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
        List<Action<BaseMessage>> actions;
        if (_msgHandlerMap.ContainsKey(type))
        {
            actions = _msgHandlerMap[type];
            actions.Add(action);
        }
        else
        {
            actions = new List<Action<BaseMessage>>();
            actions.Add(action);
            _msgHandlerMap.Add(type, actions);
        }
    }

    /// <summary>
    /// 得到对应消息的回调函数
    /// </summary>
    /// <returns>The action.</returns>
    /// <param name="type">Type.</param>
    public List<Action<BaseMessage>> GetActions(byte type)
    {
        if (_msgHandlerMap.ContainsKey(type))
        {
            return _msgHandlerMap[type];
        }

        return null;
    }

    private int _tryTime = 0;
    /// <summary>
    /// 得到某一帧消息
    /// </summary>
    /// <returns>The messages.</returns>
    /// <param name="frameIdx">Frame index.</param>
    public MessageQueue GetMessages(long frameIdx)
    {
        if (WaitMsgs.ContainsKey(frameIdx))
        {
            return WaitMsgs[frameIdx];
        }
        else
        {
            if (LockstepManager.Instance.frameIdx < GameServer.Instance.frameIdx)
            {
                _tryTime++;

                if (_tryTime >= 3)
                {
                    MessageRequest.Instance.RequestMessageQueueAtFrame(frameIdx);
                    _tryTime = 0;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 序列化消息队列
    /// </summary>
    /// <returns>The message queue.</returns>
    /// <param name="msgQueue">Message queue.</param>
    public byte[] SerializeObj<T>(T obj)
    {
        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, obj);
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
    public T DesrializeObj<T>(byte[] bytes)
    {
        T obj;

        IFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(bytes);
        obj = (T)formatter.Deserialize(stream);
        stream.Flush();
        stream.Close();

        return obj;
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="msg">Message.</param>
    public void HandleMessage(BaseMessage msg)
    {
        List<Action<BaseMessage>> actions = GetActions(msg.Type);

        if (actions == null)
        {
            DebugHelper.Instance.Log("Action null");
        }

        if (actions != null)
        {
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i](msg);
            }
        }
    }

    public void CompleteMessageQueue(long frameIdx)
    {
        RemoveWaitMessage(frameIdx);
    }

    public void RemoveWaitMessage(long frameIdx)
    {
        if (WaitMsgs.ContainsKey(frameIdx))
        {
            WaitMsgs.Remove(frameIdx);
        }
    }
}