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

    public bool gameStart = false;

    private static uint currentPlayerId = 0;
    private uint _readyCount = 0;

    public uint GetValidPlayerId()
    {
        return currentPlayerId++;
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
