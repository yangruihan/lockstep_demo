using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    #region 属性
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

    [SyncVar]
    private Vector3 _spawnPosition;
    public Vector3 SpawnPosition
    {
        set
        {
            _spawnPosition = value;
        }
        get
        {
            return _spawnPosition;
        }
    }
    #endregion

    private GameObject roleObj;
    public GameObject RoleObj
    {
        get
        {
            return roleObj;
        }
    }

    public T GetObjComponent<T>()
    {
        T t = roleObj.GetComponent<T>();
        return t;
    }

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
        GameManager.Instance.AddPlayer(this);

        SpawnRoleObj();

        if (isLocalPlayer)
        {
            InitLocalPlayer();
        }
    }

    private void InitLocalPlayer()
    {
        GameManager.Instance.LocalPlayer = this;
        GetReady();
    }

    private void SpawnRoleObj()
    {
        roleObj = Instantiate(GameManager.Instance.rolePrefab, SpawnPosition, Quaternion.identity);
        roleObj.name = "Role" + PlayerId;

        InitRole();
    }

    private void InitRole()
    {
        SpriteRenderer image = roleObj.GetComponentInChildren<SpriteRenderer>();
        image.color = PlayerColor;
    }
}
