using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
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

    [SyncVar]
    private Color _playerColor;
    public Color PlayerColor
    {
        set
        {
            _playerColor = value;
        }
        get
        {
            return _playerColor;
        }
    }

    private GameObject roleObj;

    #region Client
    [Client]
    private void GetReady()
    {
        CmdGetReady();
    }
    #endregion

    #region Server
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
    #endregion

    [ClientCallback]
    private void Start()
    {
        if (isLocalPlayer)
        {
            GameManager.Instance.LocalPlayer = this;
            GetReady();
        }

        SpawnRoleObj();
    }

    private void SpawnRoleObj()
    {
        roleObj = GameManager.Instance.rolePrefab;
        Instantiate(roleObj);

        InitRole();
    }

    private void InitRole()
    {
        SpriteRenderer image = roleObj.GetComponentInChildren<SpriteRenderer>();
        image.color = PlayerColor;
    }
}
