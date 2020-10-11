using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerTest : NetworkManager
{
    public List<GameObject> players = new List<GameObject>();

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        players.Add(player);
        DraftPool.instance.numberOfPlayers++;

        if (DraftPool.instance.numberOfPlayers == 2)
        {
            foreach (GameObject newPlayer in players)
            {
                newPlayer.GetComponent<Player>().RpcStartGame();
            }
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
    }
}
