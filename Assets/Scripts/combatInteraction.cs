using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class combatInteraction : MonoBehaviour
{
    public GameObject combatScreen;
    public GameObject map;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private bool isSpectating;
    private string characterName;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        isSpectating = false;
        for (int i = 0; i < 5; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == characterName)
            {
                isSpectating = true;
            }
        }

        for (int i = 0; i < players.Length; i++)
        {
            if ((players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase == 1 && i != playerID) || isSpectating)
            {
                if (map.transform.position.x < 140)
                {
                    combatScreen.SetActive(true);
                }
                else
                {
                    combatScreen.SetActive(false);
                }
            }
            else if (!combatScreen.activeInHierarchy)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = false;
            }
        }
    }
}
