using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class combatController : MonoBehaviour
{
    public int phase;
    public GameObject lobby;
    public GameObject cycle;
    public GameObject resolution;
    public GameObject[] subPhases;
    public GameObject abilityCharges;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string combatLeader;
    private int leaderID;
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
        abilityCharges.GetComponent<Text>().text = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges;

        isSpectating = false;
        for (int i = 0; i < 5; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == characterName)
            {
                isSpectating = true;
            }
        }

        combatLeader = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator];
        //serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[0] = combatLeader;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == combatLeader)
            {
                leaderID = i;
            }
        }
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase == 3)
        {
            phase = 3;
        }
        else
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase > players[leaderID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase && !isSpectating)
            {
                phase = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase;
            }
            else
            {
                if (players[leaderID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase < 3)
                    phase = players[leaderID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase;
                else
                    phase = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase;
            }
        }


        switch (phase)
        {
            case 0: break;
            case 1: lobby.SetActive(true); cycle.SetActive(false); resolution.SetActive(false); break;
            case 2: lobby.SetActive(false); cycle.SetActive(true); resolution.SetActive(false); break;
            case 3: lobby.SetActive(false); cycle.SetActive(false); resolution.SetActive(true); break;
        }

        if (phase > 0)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = true;
        else
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = false;
    }

    public void incrementPhase()
    {
        if (phase == 1)
        {
            //switch (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility)
            //{
            //    case 1:
            //        break;
            //}
        }
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase++;
        //phase++;
    }
}
