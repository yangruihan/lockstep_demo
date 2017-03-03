using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameServer : NetworkBehaviour
{
    public static long DEFAULT_START_FRAME = 0;

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

    [SyncVar]
    public long frameIdx;  // 帧号

    private float _accumilatedTime = 0f;
    private float _frameLength = 0.067f;

    private void Awake()
    {
        _instance = this;
    }

    [ServerCallback]
    private void Start()
    {
        frameIdx = DEFAULT_START_FRAME;
    }

    [ServerCallback]
    private void Update()
    {
        if (!GameManager.Instance.gameStart) return;

        _accumilatedTime = _accumilatedTime + Time.deltaTime;

        while (_accumilatedTime > _frameLength)
        {
            long curFrameIdx = frameIdx;

            ServerFrameTurn(curFrameIdx);

            frameIdx++;

            _accumilatedTime = _accumilatedTime - _frameLength;
        }
    }

    [Server]
    private void ServerFrameTurn(long curFrameIdx)
    {
        MessageRequest.Instance.ReturnMessage(curFrameIdx);
    }
}
