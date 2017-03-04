using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Player player = gamePlayer.GetComponent<Player>();

        player.PlayerId = GameManager.Instance.GetValidPlayerId();
        player.PlayerColor = GameManager.Instance.GetRandomColor(player.PlayerId);
        player.SpawnPosition = GameManager.Instance.GetRandomPosition(player.PlayerId);
        GameManager.Instance.AddPlayer(player);
    }
}
