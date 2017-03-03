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
	private float _frameLength = 0.025f;

    private void Awake()
    {
        _instance = this;
    }

    [ClientCallback]
    private void Update()
    {
		_accumilatedTime = _accumilatedTime + Time.deltaTime;

		while (_accumilatedTime > _frameLength) {
		
			while (HasNextFrame ()) {
				GameFrameTurn ();
			}

			_accumilatedTime = _accumilatedTime - _frameLength;
		}
	}

    #region Client
	[Client]
	private bool HasNextFrame()
	{
		return !(MessageManager.Instance.GetMessages (frameIdx) == null);
	}

    [Client]
    private void GameFrameTurn()
    {
		if (HandleMessages())
		{
			frameIdx++;
		}
    }

    [Client]
	private bool HandleMessages()
    {
        MessageQueue msgQueue = MessageManager.Instance.GetMessages(frameIdx);

        if (msgQueue != null)
        {
			for (int i = 0; i < msgQueue.messages.Length; i++)
			{
				MessageManager.Instance.HandleMessage (msgQueue.messages [i]);
			}

			MessageManager.Instance.RemoveWaitMessage (frameIdx);

			return true;
        }

		return false;
    }
    #endregion
}