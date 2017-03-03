using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LockstepManager : NetworkBehaviour
{
    #region Singleton
    private static LockstepManager _instance;
    public static LockstepManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public long frameIdx = 0;  // 帧号

    private float _accumilatedTime = 0f;
    private float _frameLength = 0.05f;

    private void Awake()
    {
        _instance = this;
    }

    [ClientCallback]
    private void Update()
    {
        _accumilatedTime = _accumilatedTime + Time.deltaTime;

        while (_accumilatedTime > _frameLength)
        {
            GameFrameTurn();
            _accumilatedTime = _accumilatedTime - _frameLength;
        }
    }

    #region Client
    [Client]
    private void GameFrameTurn()
    {
        ReceiveMessages();
    }

    [Client]
    private void ReceiveMessages()
    {
        MessageQueue msgQueue = MessageManager.Instance.GetMessages(frameIdx);

        if (msgQueue != null)
        {
            foreach (var msg in msgQueue.messages)
            {
				if (msg.Type == MessageType.STRING_MSG)
				{
					StringMessage smsg = msg as StringMessage;
					Debug.Log (smsg.Content);
				}
            }

            frameIdx++;
        }
    }
    #endregion
}