using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class combatLobby : MonoBehaviour
{
    public GameObject monsterDisplay;
    public GameObject[] playerDisplays;
    public GameObject[] playerNames;
    public GameObject fullAssistButton;
    public GameObject fullAssistText;
    public GameObject assistButton;
    public GameObject assistText;
    public GameObject spectateButton;
    public GameObject startCombatButton;
    public GameObject cancelButton;
    public GameObject waitingOnPlayers;
    public GameObject resolution;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private string monster;
    private string combatLeader;
    private int leaderID;
    private bool inParty;
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
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[0] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[0] != "")
        {
            //assign leader
            combatLeader = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator];
            //serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[0] = combatLeader;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == combatLeader)
                {
                    leaderID = i;
                }
            }

            //stealth past monster
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth)
            {
                resolution.GetComponent<combatResolution>().victory = true;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 3;
            }

            //make everyone join bossfight if its a boss fight
            if (players[leaderID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight = true;

            //draw monster
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
            {
                monster = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area-1];
                monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + monster, typeof(Sprite));
            }
            else
            {
                monster = players[leaderID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter;
                monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + monster, typeof(Sprite));
            }

            //draw people in combat party
            for (int i = 0; i < 6; i++)
            {
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != "")
                {
                    for (int k = 0; k < players.Length; k++)
                    {
                        if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i])
                        {
                            playerDisplays[i].GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                            playerNames[i].GetComponent<Text>().text = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                            playerDisplays[i].SetActive(true);
                        }
                    }

                }
                else
                {
                    playerDisplays[i].SetActive(false);
                    playerDisplays[i].GetComponent<Image>().sprite = null;
                    playerNames[i].GetComponent<Text>().text = null;
                }
            }

            //draw assist button
            assistText.GetComponent<Text>().text = "Assist " + combatLeader;
            fullAssistText.GetComponent<Text>().text = "Assist " + combatLeader;

            inParty = false;
            for (int i = 0; i < 6; i++)
            {
                if (characterName == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i])
                    inParty = true;
            }
            for (int i = 0; i < 5; i++)
            {
                if (characterName == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i])
                    inParty = true;
            }

            if (inParty)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsAssisting = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsSpectating = false;
                if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight && characterName != combatLeader)
                    cancelButton.SetActive(true);
                else
                    cancelButton.SetActive(false);
                assistButton.SetActive(false);
                spectateButton.SetActive(false);
                fullAssistButton.SetActive(false);
                waitingOnPlayers.SetActive(false);
                if (characterName == combatLeader)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                    {
                        if(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 1)
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss1 = true;
                        else if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 2)
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss2 = true;
                        else if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 3)
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss3 = true;

                        int playersIn = 0;
                        int totalPlayers = players.Length;

                        for (int i = 0; i < 6; i++)
                        {
                            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != "")
                                playersIn++;
                        }
                        for (int i = 0; i < players.Length; i++)
                        {
                            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                                totalPlayers--;
                        }

                        if (playersIn == totalPlayers)
                        {
                            startCombatButton.SetActive(true);
                            waitingOnPlayers.SetActive(false);
                        }
                        else
                        {
                            startCombatButton.SetActive(false);
                            waitingOnPlayers.SetActive(true);
                        }
                    }
                    else
                    {
                        int playersIn = 0;
                        int totalPlayers = players.Length;

                        for (int i = 0; i < 6; i++)
                        {
                            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != "")
                                playersIn++;
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] != "")
                                playersIn++;
                        }

                        if (playersIn == totalPlayers)
                        {
                            startCombatButton.SetActive(true);
                            waitingOnPlayers.SetActive(false);
                        }
                        else
                        {
                            startCombatButton.SetActive(false);
                            waitingOnPlayers.SetActive(true);
                        }
                    }
                }
                else
                {
                    startCombatButton.SetActive(false);
                    waitingOnPlayers.SetActive(false);
                }
            }
            else
            {
                if (locationCheck(leaderID))
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                    {
                        fullAssistButton.SetActive(true);
                        assistButton.SetActive(false);
                        spectateButton.SetActive(false);
                        waitingOnPlayers.SetActive(false);
                    }
                    else
                    {
                        fullAssistButton.SetActive(false);
                        assistButton.SetActive(true);
                        spectateButton.SetActive(false);
                        waitingOnPlayers.SetActive(false);
                    }
                }
                else
                {
                    fullAssistButton.SetActive(false);
                    spectateButton.SetActive(true);
                    assistButton.SetActive(false);
                    waitingOnPlayers.SetActive(false);
                }
                cancelButton.SetActive(false);
                startCombatButton.SetActive(false);
                waitingOnPlayers.SetActive(false);
            }
        }
    }

    public void joinCombatants()
    {
        //for (int i = 0; i < 6; i++)
        //{
        //    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == "")
        //    {
        //        serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] = characterName;
        //        i = 100;
        //    }
        //}
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsAssisting = true;

    }

    public void joinSpectators()
    {
        //for (int i = 0; i < 5; i++)
        //{
        //    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == "")
        //    {
        //        serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] = characterName;
        //        i = 100;
        //    }
        //}
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsSpectating = true;
    }

    public void leaveGroup()
    {
        //for (int i = 0; i < 6; i++)
        //{
        //    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == characterName)
        //    {
        //        serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] = null;
        //        i = 100;
        //    }
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == characterName)
        //    {
        //        serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] = null;
        //        i = 100;
        //    }
        //}
        //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsAssisting = false;
        //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsSpectating = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LeftGroup = true;
    }

    public bool locationCheck(int targetID)
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            return false;

        List<string> location = new List<string>(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location.Split(','));
        int playerX = int.Parse(location[0]);
        int playerY = int.Parse(location[1]);
        int shifter;
        int targetX;
        int targetY;

        if (playerX % 2 == 0)
            shifter = -1;
        else
            shifter = 0;

        location = new List<string>(players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location.Split(','));
        targetX = int.Parse(location[0]);
        targetY = int.Parse(location[1]);

        if ((targetX == playerX && targetY == playerY) || (targetX == playerX && targetY == playerY + 1) || (targetX == playerX && targetY == playerY - 1) || (targetX == playerX + 1 && targetY == playerY + 1 + shifter) || (targetX == playerX + 1 && targetY == playerY + shifter) || (targetX == playerX - 1 && targetY == playerY + 1 + shifter) || (targetX == playerX - 1 && targetY == playerY + shifter))
        {
            return true;
        }
        return false;
    }
}
