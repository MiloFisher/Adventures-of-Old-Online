using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[BoltGlobalBehaviour]
public class NetworkCallbacks : Bolt.GlobalEventListener
{
    public override void SceneLoadLocalDone(string map)
    {
        // randomize a position
        var spawnPosition = new Vector3(100, 100, 0);

       
        //BoltNetwork.Instantiate(BoltPrefabs.Game_Piece, spawnPosition, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.PlayerInfo, spawnPosition, Quaternion.identity);
        BoltNetwork.Instantiate(BoltPrefabs.ServerInfo, spawnPosition, Quaternion.identity);
    }
}