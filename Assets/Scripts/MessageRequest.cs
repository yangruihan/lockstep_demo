using System.Collections;
using System.Collections.Generic;
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

    #region 属性
    private List<BaseMessage> _msgBuffer = new List<BaseMessage>();
    public List<BaseMessage> msgBuffer
    {
        get
        {
            return _msgBuffer;
        }
    }
    #endregion

    #region Client
    [Client]
    public void SendMessage(BaseMessage message)
    {
        CmdSendMessage(message);
    }

    [Command]
    public void CmdSendMessage(BaseMessage message)
    {
        ReceiveMessage(message);
    }
    #endregion


    #region Server
    [Server]
    public void ReceiveMessage(BaseMessage message)
    {
        msgBuffer.Add(message);
    }
    #endregion

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != this)
            {
                Destroy(this);
            }
        }
    }
}
