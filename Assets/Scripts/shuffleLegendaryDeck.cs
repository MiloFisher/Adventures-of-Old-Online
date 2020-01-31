using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuffleLegendaryDeck : MonoBehaviour
{
    //private GameObject[] serverInfoImposters;
    private GameObject serverInfo;
    private GameObject[] players;
    private string[] playerClasses = new string[6];
    // Start is called before the first frame update
    void Start()
    {
        //serverInfoImposters = GameObject.FindGameObjectsWithTag("Server Info");
        //for (int i = 0; i < serverInfoImposters.Length; i++)
        //{
        //    if (serverInfoImposters[i].GetComponent<BoltEntity>().GetState<IServerInfo>().Players == "0")
        //    {
        //        Destroy(serverInfoImposters[i]);
        //        i--;
        //    }
        //}
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");

        for (int i = 0; i < players.Length; i++)
        {
            playerClasses[i] = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class;
        }

        if (serverInfo != null)
        {
            serverInfo.GetComponent<serverInfoBehaviour>().shuffleLegendaryDeck(playerClasses);
        }
    }
}
