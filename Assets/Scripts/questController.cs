using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class questController : MonoBehaviour
{
	public string quest;
    public bool questCompleted;
    public bool canGetPayout = true;

	public bool visitedCastleGuard;
	public bool visitedLostAdventurer;
	public bool visitedElvenHunter;
	public bool visitedOminousCat;
	public bool visitedLuckyFisherman;
	public bool visitedCharmingBard;
	public bool visitedSleepyMermaid;

	public int treasureCardsDrawn;
	public int encounterCardsDrawn;
	public int monstersKilled;
	public int abilityChargesSpent;
	public int treasureTrovesSearched;

    private bool[] stepsCompleted = new bool[5];
    public GameObject[] completionMarkers;
    public GameObject pickLegendary;

    private int id;
    private GameObject[] players;
    private int tempGold;
    private GameObject player;
    private bool assignedQuest2;
    private bool assignedQuest3;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < completionMarkers.Length; i++)
            completionMarkers[i].SetActive(false);

        assignQuest();
        drawTwoTreasures();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<movement>().area == 2 && !assignedQuest2)
        {
            assignQuest();
            assignedQuest2 = true;
        }
        else if(player.GetComponent<movement>().area == 3 && !assignedQuest3)
        {
            assignQuest();
            assignedQuest3 = true;
        }

        if (quest != null)
        {
            //draws correct quest
            gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("Quest Cards/" + quest, typeof(Sprite));

            //clears stepsCompleted markers
            for (int i = 0; i < stepsCompleted.Length; i++)
                stepsCompleted[i] = false;
            questCompleted = false;

            //checks quest progress
            if(quest != null && quest != "")
                id = int.Parse(quest.Substring(1,quest.Length-1));

            if (id < 8)//first area quest
            {
                switch (id)
                {
                    case 0:
                        if (visitedCastleGuard)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 1:
                        if (visitedLostAdventurer)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 2:
                        if (visitedCastleGuard)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 3:
                        if (visitedLostAdventurer)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 4:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 5:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 6:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        break;
                    case 7:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        break;
                }

                if (stepsCompleted[0] && stepsCompleted[1] && stepsCompleted[2])
                {
                    questCompleted = true;
                    if (canGetPayout)
                    {
                        StartCoroutine(payout(1));
                    }
                }
            }
            else if (id < 16)//second area quest
            {
                switch (id)
                {
                    case 8:
                        if (visitedElvenHunter)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (visitedOminousCat)
                            stepsCompleted[3] = true;
                        break;
                    case 9:
                        if (visitedLuckyFisherman)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[3] = true;
                        break;
                    case 10:
                        if (visitedOminousCat)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[3] = true;
                        break;
                    case 11:
                        if (visitedElvenHunter)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (visitedLuckyFisherman)
                            stepsCompleted[3] = true;
                        break;
                    case 12:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (visitedLuckyFisherman)
                            stepsCompleted[3] = true;
                        break;
                    case 13:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 2)
                            stepsCompleted[3] = true;
                        break;
                    case 14:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 2)
                            stepsCompleted[3] = true;
                        break;
                    case 15:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (visitedOminousCat)
                            stepsCompleted[3] = true;
                        break;
                }

                if (stepsCompleted[0] && stepsCompleted[1] && stepsCompleted[2] && stepsCompleted[3])
                {
                    questCompleted = true;
                    if (canGetPayout)
                    {
                        StartCoroutine(payout(2));
                    }
                }
            }
            else //third area quest
            {
                switch (id)
                {
                    case 16:
                        if (visitedCharmingBard)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (visitedSleepyMermaid)
                            stepsCompleted[3] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 17:
                        if (visitedSleepyMermaid)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[3] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 18:
                        if (visitedCharmingBard)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[3] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 19:
                        if (visitedSleepyMermaid)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (visitedCharmingBard)
                            stepsCompleted[3] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 20:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (visitedCharmingBard)
                            stepsCompleted[3] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 21:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (treasureCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 2)
                            stepsCompleted[3] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 22:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[2] = true;
                        if (treasureTrovesSearched >= 2)
                            stepsCompleted[3] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[4] = true;
                        break;
                    case 23:
                        if (treasureTrovesSearched >= 1)
                            stepsCompleted[0] = true;
                        if (encounterCardsDrawn >= 3)
                            stepsCompleted[1] = true;
                        if (abilityChargesSpent >= 2)
                            stepsCompleted[2] = true;
                        if (visitedSleepyMermaid)
                            stepsCompleted[3] = true;
                        if (monstersKilled >= 2)
                            stepsCompleted[4] = true;
                        break;
                }

                if (stepsCompleted[0] && stepsCompleted[1] && stepsCompleted[2] && stepsCompleted[3] && stepsCompleted[4])
                {
                    questCompleted = true;
                    if (canGetPayout)
                    {
                        StartCoroutine(payout(3));
                    }
                }
            }

            //draw appropriate completion markers
            for (int i = 0; i < stepsCompleted.Length; i++)
            {
                if (stepsCompleted[i])
                    completionMarkers[i].SetActive(true);
                else
                    completionMarkers[i].SetActive(false);
            }

            if(questCompleted)
                completionMarkers[5].SetActive(true);
            else
                completionMarkers[5].SetActive(false);
        }
	}

    public void assignQuest()
    {
        GameObject serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string Username = sr.ReadLine();
        string Name = sr.ReadLine();
        sr.Close();

        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().PlayerIDs.Length; i++)
        {
            if (Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().PlayerIDs[i])
            {
                if(player.GetComponent<movement>().area == 1)
                    quest = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().QuestDeck1[i];
                else if (player.GetComponent<movement>().area == 2)
                    quest = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().QuestDeck2[i];
                else if (player.GetComponent<movement>().area == 3)
                    quest = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().QuestDeck3[i];
                canGetPayout = true;
            }
        }

        treasureCardsDrawn = 0;
        encounterCardsDrawn = 0;
        monstersKilled = 0;
        abilityChargesSpent = 0;
        treasureTrovesSearched = 0;

        if (quest == null || quest == "")
            assignQuest();
    }

    public void drawTwoTreasures()
    {
        int playerID = 0;
        GameObject serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string Username = sr.ReadLine();
        string Name = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == Name)
            {
                playerID = i;
            }
        }

        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().PlayerIDs.Length; i++)
        {
            if (Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().PlayerIDs[i])
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[0] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + (i * 2)];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[1] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + (i * 2) + 1];
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[0] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[0] == "")
            drawTwoTreasures();
    }

    IEnumerator payout(int level)
    {
        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().CreateNotification("You completed your quest!");
        nc.GetComponent<NotificationController>().SendNotification("all", players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " completed their quest!");

        canGetPayout = false;
        if (level == 1)
        {
            tempGold = int.Parse(players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
            tempGold += 100;
            players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = tempGold + ""; 
        }
        else if (level == 2)
        {
            tempGold = int.Parse(players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
            tempGold += 200;
            players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = tempGold + "";
        }
        else if (level == 3)
        {
            //draw legendary card
            pickLegendary.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
