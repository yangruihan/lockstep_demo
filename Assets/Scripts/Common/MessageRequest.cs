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
        CmdSendMessage(message);
    }

    [ClientRpc]
    public void RpcReturnMessage(byte[] bytes)
    {
        MessageQueue msgQueue = MessageManager.Instance.DesrializeMessageQueue(bytes);

        if (!MessageManager.Instance.WaitMsgs.ContainsKey(msgQueue.frameIdx))
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
		
	}
    #endregion

    #region Server
    [Command]
    public void CmdSendMessage(BaseMessage message)
    {
        ReceiveMessage(message);
    }

    [Server]
    public void ReceiveMessage(BaseMessage message)
    {
        MessageManager.Instance.MsgBuffer.Add(message);
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

        RpcReturnMessage(MessageManager.Instance.SerializeMessageQueue(msgQueue));
    }
    #endregion

    private void Awake()
    {
        _instance = this;
    }
}
