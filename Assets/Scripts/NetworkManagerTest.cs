using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerTest : NetworkManager
{
    public int numberOfPlayers = 0;
    public GameObject draftPool;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        if (draftPool == null)
        {
            draftPool = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "DraftPool"));
            NetworkServer.Spawn(draftPool);
        }
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);
        numberOfPlayers++;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        numberOfPlayers--;
    }
}
