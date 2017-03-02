using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameServer : NetworkBehaviour
{
    #region Singleton
    private static GameServer _instance;
    public static GameServer Instance
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

    [ServerCallback]

    private void Awake()
    {
        _instance = this;
    }

    [ServerCallback]

    private void Update()
    {
        _accumilatedTime = _accumilatedTime + Time.deltaTime;

        while (_accumilatedTime > _frameLength)
        {
            ServerFrameTurn();
            _accumilatedTime = _accumilatedTime - _frameLength;
        }
    }

    [Server]
    private void ServerFrameTurn()
    {
        ReturnMessages();
        frameIdx++;
    }

    [Server]
    private void ReturnMessages()
    {
        MessageRequest.Instance.ReturnMessage(frameIdx);
    }
}
