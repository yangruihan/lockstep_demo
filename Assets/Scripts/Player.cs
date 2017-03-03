using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private uint _playerId;
    public uint PlayerId
    {
        set
        {
            _playerId = value;
        }
        get
        {
            return _playerId;
        }
    }

    [Client]
    private void GetReady()
    {
        CmdGetReady();
    }

    [Command]
    private void CmdGetReady()
    {
        TellManagerGetReady();
    }

    [Server]
    private void TellManagerGetReady()
    {
        GameManager.Instance.PlayerGetReady();
    }

    [ClientCallback]
    private void Start()
    {
        if (!isLocalPlayer) return;

        GetReady();
    }
}
