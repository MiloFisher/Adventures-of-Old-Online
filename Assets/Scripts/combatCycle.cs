using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class combatCycle : MonoBehaviour
{
    public GameObject resolution;
    public int phase;
    public GameObject spectatorScreen;
    public GameObject actionScreen;
    public GameObject rollScreen;
    public GameObject previewScreen;
    public GameObject resolveScreen;
    public GameObject combatScreen;

    private GameObject[] players;
    private GameObject serverInfo;
    private int playerID;
    private string characterName;
    private bool isSpectating;
    private bool isFighting;
    private bool someoneIsAlive;
    public int combatants;
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
        isFighting = false;
        for (int i = 0; i < 5; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == characterName)
            {
                isFighting = true;
            }
        }

        if (isSpectating)
        {
            spectatorScreen.SetActive(true);
            actionScreen.SetActive(false);
            rollScreen.SetActive(false);
            previewScreen.SetActive(false);
            resolveScreen.SetActive(false);
        }
        else if (isFighting)
        {
            spectatorScreen.SetActive(false);

            if (phase == 0)
                actionScreen.SetActive(true);
            else
                actionScreen.SetActive(false);
            if (phase == 1)
                rollScreen.SetActive(true);
            else
                rollScreen.SetActive(false);
            if (phase == 2)
                previewScreen.SetActive(true);
            else
                previewScreen.SetActive(false);
            if (phase == 3)
                resolveScreen.SetActive(true);
            else
                resolveScreen.SetActive(false);

            someoneIsAlive = false;
            for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
            {
                for (int k = 0; k < players.Length; k++)
                {
                    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                    {
                        if (!players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                            someoneIsAlive = true;
                        k = 100;
                    }
                }
            }

            if (!someoneIsAlive)
            {
                combatFailed();
            }
        }
        else
        {
            combatScreen.SetActive(false);
        }
    }

    public void IncrementPhase()
    {
        phase++;
    }

    public void DecrementPhase()
    {
        phase--;
    }

    public void ResetPhase()
    {
        phase = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedCombatTurn = true;
    }

    public void combatFailed()
    {
        resolution.GetComponent<combatResolution>().victory = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 3;
    }
}
