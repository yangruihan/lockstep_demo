using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{
    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    private Color[] _colors = { Color.red, Color.yellow, Color.green, Color.blue };

    public Player _localPlayer;
    public Player LocalPlayer
    {
        set
        {
            _localPlayer = value;
        }
        get
        {
            return _localPlayer;
        }
    }

    public bool gameStart = false;              // 游戏开始的标志

    public GameObject rolePrefab;               // 角色prefab

    private static uint currentPlayerId = 0;    // 当前玩家的id号，用于分配id
    private uint _readyCount = 0;               // 准备好的玩家数量，用于判断是否开始游戏

    /// <summary>
    /// 得到一个合法的玩家id
    /// </summary>
    /// <returns></returns>
    public uint GetValidPlayerId()
    {
        return currentPlayerId++;
    }

    /// <summary>
    /// 得到一个合法的颜色
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
    public Color GetValidColor(uint playerId)
    {
        Debug.Log(playerId);

        return _colors[playerId % _colors.Length];
    }

    [Server]
    public void PlayerGetReady()
    {
        _readyCount++;

        if (_readyCount == players.Count)
        {
            gameStart = true;
        }
    }

    public void AddPlayer(Player player)
    {
        if (!players.ContainsKey(player.PlayerId))
        {
            players.Add(player.PlayerId, player);
            OnPlayerAdded(player);
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnPlayerAdded(Player player)
    {
        Debug.Log("OnPlayerAdded: playerId " + player.PlayerId);
    }
}
