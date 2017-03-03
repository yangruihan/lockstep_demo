using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageRequest : NetworkBehaviour
{
    #region Singleton
    private static MessageRequest _instance;

    public static MessageRequest Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    #region Client
    [Client]
    public void SendMessage(BaseMessage message)
    {
        CmdSendMessage(MessageManager.Instance.SerializeObj<BaseMessage>(message));
    }

    [ClientRpc]
    public void RpcReturnMessage(byte[] bytes)
    {
        MessageQueue msgQueue = MessageManager.Instance.DesrializeObj<MessageQueue>(bytes);

        if (msgQueue != null && !MessageManager.Instance.WaitMsgs.ContainsKey(msgQueue.frameIdx))
        {
            MessageManager.Instance.WaitMsgs.Add(msgQueue.frameIdx, msgQueue);
        }
    }

    /// <summary>
    /// 请求某一帧的消息队列
    /// </summary>
    /// <param name="frameIdx">Frame index.</param>
    [Client]
    public void RequestMessageQueueAtFrame(long frameIdx)
    {
        CmdRequestMessageQueueAtFrame(frameIdx);
    }
    #endregion

    #region Server
    [Command]
    public void CmdSendMessage(byte[] bytes)
    {
        ReceiveMessage(bytes);
    }

    [Command]
    public void CmdRequestMessageQueueAtFrame(long frameIdx)
    {
        ReturnMessageAtFrame(frameIdx);
    }

    [Server]
    public void ReceiveMessage(byte[] bytes)
    {
        MessageManager.Instance.MsgBuffer.Add(MessageManager.Instance.DesrializeObj<BaseMessage>(bytes));
    }

    [Server]
    public void ReturnMessage(long frameIdx)
    {
        BaseMessage[] msgs = MessageManager.Instance.MsgBuffer.ToArray();
        MessageQueue msgQueue = new MessageQueue(frameIdx, msgs);

        MessageManager.Instance.MsgBuffer.Clear();

        if (!MessageManager.Instance.FrameMsgs.ContainsKey(frameIdx))
        {
            MessageManager.Instance.FrameMsgs.Add(frameIdx, msgQueue);
        }

        RpcReturnMessage(MessageManager.Instance.SerializeObj<MessageQueue>(msgQueue));
    }

    [Server]
    public void ReturnMessageAtFrame(long frameIdx)
    {
        MessageQueue msgQueue = null;

        if (MessageManager.Instance.FrameMsgs.ContainsKey(frameIdx))
        {
            msgQueue = MessageManager.Instance.FrameMsgs[frameIdx];
        }

        RpcReturnMessage(MessageManager.Instance.SerializeObj<MessageQueue>(msgQueue));
    }
    #endregion

    private void Start()
    {
        if (isLocalPlayer)
        {
            _instance = this;
        }
    }
}
