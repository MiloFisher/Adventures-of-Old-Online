using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class turnManager : MonoBehaviour
{
    public GameObject[] playerPieces;
    public GameObject[] trovesPieces;
    public GameObject combatScreen;
    public GameObject levelImprovementScreen;
    public GameObject burnCardButton;
    public GameObject burnedEncounterMessage;
    public GameObject quest;
    public GameObject continueButton;
    public GameObject yourTurnMessage;
    public GameObject rollForMovement;
    public GameObject movementDisplay;
    public GameObject movementButton;
    public GameObject movementPhaseMessage;
    public GameObject rollForEncounter;
    public GameObject encounterButton;
    public GameObject encounterRollDisplay;
    public GameObject encounterPhaseMessage;
    public GameObject guaranteedEncounterMessage;
    public GameObject encounterDeck;
    public GameObject encounterDisplay;
    public GameObject[] encounterFails;
    public GameObject treasureTrove;
    public GameObject treasureTroveDisplay1;
    public GameObject treasureTroveDisplay2;
    public GameObject treasureTroveDisplay3;
    public GameObject resolveEncounterButton;
    public GameObject fightMonsterButton;
    public GameObject fightMonsterMiniButton;
    public GameObject ignoreMonsterMiniButton;
    public GameObject npc;
    public GameObject npcName;
    public GameObject store;
    public GameObject lackSufficientGoldMessage;
    public GameObject needMoreSpaceMessage;
    public GameObject battlegroundRollDisplay;
    public GameObject battlegroundRollNumber;
    public GameObject battlegroundRollButton;
    public GameObject endTurnButton;
    public GameObject discardDisplay;
    public GameObject mysteriousOrbRollDisplay;
    public GameObject mysteriousOrbRollNumber;
    public GameObject mysteriousOrbRollButton;
    public GameObject bossRevealScreen;
    public GameObject path1;
    public GameObject path2;
    public GameObject ominousCatRollDisplay;
    public GameObject ominousCatRollNumber;
    public GameObject ominousCatRollButton;
    public GameObject abilityChargeDiscardScreen;
    public GameObject error1;
    public GameObject error2;
    public GameObject mermaidScreen;
    public GameObject bossFightButtons;
    public int abilityChargesToDiscard = 0;
    public GameObject masterAdventurer;
    public GameObject masterAdventurerButton;
    public GameObject masterAdventurerNumber;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    public int phase;
    private GameObject player;
    private string[] unsearchedTroves = new string[9];
    private string position;
    private int branch;
    private string treasure1;
    private string treasure2;
    private string treasure3;
    private bool[] fails = new bool[2];
    private int finalPhase = 8;
    private string pathData1;
    private string pathData2;
    private bool ignoredBossFight;
    private bool noEncounter;
    private string encounterID;
    private bool buyingItem;

    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        string characterName = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
        phase = 0;
        endTurnButton.SetActive(false);
        combatScreen.SetActive(false);
        levelImprovementScreen.SetActive(false);
        burnedEncounterMessage.SetActive(false);
        continueButton.SetActive(false);
        yourTurnMessage.SetActive(false);
        rollForMovement.SetActive(false);
        movementPhaseMessage.SetActive(false);
        encounterPhaseMessage.SetActive(false);
        encounterDeck.SetActive(false);
        encounterDisplay.SetActive(false);
        rollForEncounter.SetActive(false);
        encounterButton.SetActive(false);
        treasureTrove.SetActive(false);
        treasureTroveDisplay1.SetActive(false);
        treasureTroveDisplay2.SetActive(false);
        treasureTroveDisplay3.SetActive(false);
        resolveEncounterButton.SetActive(false);
        fightMonsterButton.SetActive(false);
        fightMonsterMiniButton.SetActive(false);
        ignoreMonsterMiniButton.SetActive(false);
        burnCardButton.SetActive(false);
        npc.SetActive(false);
        store.SetActive(false);
        lackSufficientGoldMessage.SetActive(false);
        needMoreSpaceMessage.SetActive(false);
        encounterFails[0].SetActive(false);
        encounterFails[1].SetActive(false);
        guaranteedEncounterMessage.SetActive(false);
        battlegroundRollDisplay.SetActive(false);
        discardDisplay.SetActive(false);
        mysteriousOrbRollDisplay.SetActive(false);
        bossRevealScreen.SetActive(false);
        path1.SetActive(false);
        path2.SetActive(false);
        ominousCatRollDisplay.SetActive(false);
        abilityChargeDiscardScreen.SetActive(false);
        mermaidScreen.SetActive(false);
        bossFightButtons.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < unsearchedTroves.Length; i++)
        {
            unsearchedTroves[i] = trovesPieces[i].GetComponent<Text>().text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
        {
            if (phase == 0 && !yourTurnMessage.activeInHierarchy && (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[0] == null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[0] == ""))
            {
                ignoredBossFight = false;
                yourTurnMessage.SetActive(true);
                StartCoroutine(DecayTimer(yourTurnMessage));

                //notification
                GameObject nc = GameObject.Find("Notification Controller");
                nc.GetComponent<NotificationController>().CreateNotification("Your turn!");
                nc.GetComponent<NotificationController>().SendNotification("all",players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + "'s turn!");
            }
            else if (phase == 1 && !movementPhaseMessage.activeInHierarchy)
            {
                gameObject.GetComponent<gameMenuManager>().map();
                //player.GetComponent<movement>().calcMovement();
                if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    movementPhaseMessage.SetActive(true);
                    StartCoroutine(DecayTimer(movementPhaseMessage));
                }
                else
                {
                    phase = 3;
                    StartCoroutine(DecayTimer(burnedEncounterMessage));
                }
            }
            else if (phase == 2)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race != "Centaur")
                {
                    rollForMovement.SetActive(true);
                    movementButton.SetActive(true);
                }
                else
                {
                    player.GetComponent<movement>().moveRadius = 5;
                    player.GetComponent<movement>().calcMovement();
                }
                phase++;
            }
            else if (phase == 3)
            {
                //waiting to move
            }
            else if (phase == 4 && !encounterPhaseMessage.activeInHierarchy)
            {
                //Encounter phase
                if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    encounterPhaseMessage.SetActive(true);
                    StartCoroutine(DecayTimer(encounterPhaseMessage));
                }
                else
                {
                    phase = 7;
                    BurnEncounter();
                }
            }
            else if (phase == 5)
            {
                position = player.GetComponent<Text>().text;

                //quest completion of searching treasure trove location
                if (position == "10,18" || position == "21,13" || position == "20,6" || position == "16,11" || position == "10,7" || position == "6,12" || position == "8,0" || position == "15,5" || position == "23,4")
                    quest.GetComponent<questController>().treasureTrovesSearched++;

                //on treasure trove
                if (position == unsearchedTroves[0] || position == unsearchedTroves[1] || position == unsearchedTroves[2] || position == unsearchedTroves[3] || position == unsearchedTroves[4] || position == unsearchedTroves[5] || position == unsearchedTroves[6] || position == unsearchedTroves[7] || position == unsearchedTroves[8])
                {
                    branch = 1;
                    treasureTrove.SetActive(true);

                    //notification
                    GameObject nc = GameObject.Find("Notification Controller");
                    nc.GetComponent<NotificationController>().CreateNotification("You discovered a Treasure Trove!");
                    nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " discovered a Treasure Trove!");
                }//on npc
                else if ((position == "11,12" && !quest.GetComponent<questController>().visitedCastleGuard && (quest.GetComponent<questController>().quest == "q0" || quest.GetComponent<questController>().quest == "q2")) || (position == "22,17" && !quest.GetComponent<questController>().visitedLostAdventurer && (quest.GetComponent<questController>().quest == "q1" || quest.GetComponent<questController>().quest == "q3")) || (position == "18,10" && !quest.GetComponent<questController>().visitedElvenHunter && (quest.GetComponent<questController>().quest == "q8" || quest.GetComponent<questController>().quest == "q11")) || (position == "12,6" && !quest.GetComponent<questController>().visitedOminousCat && (quest.GetComponent<questController>().quest == "q8" || quest.GetComponent<questController>().quest == "q10" || quest.GetComponent<questController>().quest == "q15")) || (position == "0,11" && !quest.GetComponent<questController>().visitedLuckyFisherman && (quest.GetComponent<questController>().quest == "q9" || quest.GetComponent<questController>().quest == "q11" || quest.GetComponent<questController>().quest == "q12")) || (position == "6,1" && !quest.GetComponent<questController>().visitedCharmingBard && (quest.GetComponent<questController>().quest == "q16" || quest.GetComponent<questController>().quest == "q18" || quest.GetComponent<questController>().quest == "q19" || quest.GetComponent<questController>().quest == "q20")) || (position == "21,2" && !quest.GetComponent<questController>().visitedSleepyMermaid && (quest.GetComponent<questController>().quest == "q16" || quest.GetComponent<questController>().quest == "q17" || quest.GetComponent<questController>().quest == "q19" || quest.GetComponent<questController>().quest == "q23")))
                {
                    branch = 2;
                    if (position == "11,12")
                    {
                        npcName.GetComponent<Text>().text = "Castle Guard";
                        quest.GetComponent<questController>().visitedCastleGuard = true;
                    }
                    else if (position == "22,17")
                    {
                        npcName.GetComponent<Text>().text = "Lost Adventurer";
                        quest.GetComponent<questController>().visitedLostAdventurer = true;
                    }
                    else if (position == "18,10")
                    {
                        npcName.GetComponent<Text>().text = "Elven Hunter";
                        quest.GetComponent<questController>().visitedElvenHunter = true;
                    }
                    else if (position == "12,6")
                    {
                        npcName.GetComponent<Text>().text = "Ominous Cat";
                        quest.GetComponent<questController>().visitedOminousCat = true;
                    }
                    else if (position == "0,11")
                    {
                        npcName.GetComponent<Text>().text = "Lucky Fisherman";
                        quest.GetComponent<questController>().visitedLuckyFisherman = true;
                    }
                    else if (position == "6,1")
                    {
                        npcName.GetComponent<Text>().text = "Charming Bard";
                        quest.GetComponent<questController>().visitedCharmingBard = true;
                    }
                    else if (position == "21,2")
                    {
                        npcName.GetComponent<Text>().text = "Sleepy Mermaid";
                        quest.GetComponent<questController>().visitedSleepyMermaid = true;
                    }
                    npc.SetActive(true);

                    //notification
                    GameObject nc = GameObject.Find("Notification Controller");
                    nc.GetComponent<NotificationController>().CreateNotification("You encountered the " + npcName.GetComponent<Text>().text + "!");
                    nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " encountered the " + npcName.GetComponent<Text>().text + "!");
                }//on store
                else if (position == "26,7" || position == "1,1")
                {
                    branch = 3;
                    store.SetActive(true);

                    //notification
                    GameObject nc = GameObject.Find("Notification Controller");
                    nc.GetComponent<NotificationController>().CreateNotification("You entered the store!");
                    nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " has entered the store!");
                }//on boss tile
                else if (!ignoredBossFight && ((position == "26,16" && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 1) || (position == "1,8" && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 2) || (position == "26,2" && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 3)))
                {
                    bool canStartBossFight = true;
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (!players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location != players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location)
                            canStartBossFight = false;
                    }
                    if (canStartBossFight)
                    {
                        branch = 4;
                        bossFightButtons.SetActive(true);
                    }
                    else
                    {
                        branch = 0;
                        if (fails[0])
                            encounterFails[0].SetActive(true);
                        else
                            encounterFails[0].SetActive(false);
                        if (fails[1])
                            encounterFails[1].SetActive(true);
                        else
                            encounterFails[1].SetActive(false);

                        if (fails[0] && fails[1])
                            StartCoroutine(GuaranteedEncounter());
                        else
                        {
                            rollForEncounter.SetActive(true);
                            encounterButton.SetActive(true);
                        }
                    }
                }
                else
                {
                    branch = 0;
                    if (fails[0])
                        encounterFails[0].SetActive(true);
                    else
                        encounterFails[0].SetActive(false);
                    if (fails[1])
                        encounterFails[1].SetActive(true);
                    else
                        encounterFails[1].SetActive(false);

                    if (fails[0] && fails[1])
                        StartCoroutine(GuaranteedEncounter());
                    else
                    {
                        rollForEncounter.SetActive(true);
                        encounterButton.SetActive(true);
                    }
                }

                phase++;
            }
            else if (phase == 6)
            {
                //waiting for encounter deck draw
                //waiting for treasure trove draw
            }
            else if (phase == 7)
            {
                if (!(combatScreen.activeInHierarchy && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name))
                {
                    if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                    {
                        if (branch == 0)
                        {
                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat)
                            {
                                fightMonsterButton.SetActive(false);
                                resolveEncounterButton.SetActive(false);
                                fightMonsterMiniButton.SetActive(false);
                                ignoreMonsterMiniButton.SetActive(false);
                            }
                            else
                            {
                                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter.Substring(0, 1) == "m")
                                {
                                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CharmingBardBoon)
                                    {
                                        fightMonsterMiniButton.SetActive(true);
                                        ignoreMonsterMiniButton.SetActive(true);
                                    }
                                    else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter == "m3")//Carnivorous Plant Ability - It's a plant
                                    {
                                        fightMonsterMiniButton.SetActive(true);
                                        ignoreMonsterMiniButton.SetActive(true);
                                    }
                                    else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter == "m6" && int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges) > 2)//Juvenile Dragon Ability - Prey on the weak
                                    {
                                        fightMonsterMiniButton.SetActive(true);
                                        ignoreMonsterMiniButton.SetActive(true);
                                    }
                                    else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter == "m11")//Cynical Cyclops Ability - Self-Interests
                                    {
                                        int items = 0;
                                        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
                                        {
                                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
                                                items++;
                                        }
                                        if (items < 2)
                                        {
                                            fightMonsterMiniButton.SetActive(true);
                                            ignoreMonsterMiniButton.SetActive(true);
                                        }
                                        else
                                            fightMonsterButton.SetActive(true);
                                    }
                                    else
                                        fightMonsterButton.SetActive(true);
                                }
                                else
                                {
                                    if(!endTurnButton.activeInHierarchy && !battlegroundRollDisplay.activeInHierarchy && !mysteriousOrbRollDisplay.activeInHierarchy && !path1.activeInHierarchy && encounterDisplay.activeInHierarchy)
                                        resolveEncounterButton.SetActive(true);
                                }
                            }
                        }
                        else if (branch == 1)
                        {
                            //wait for treasure select
                        }
                    }
                    else
                    {
                        burnCardButton.SetActive(true);
                    }
                }
            }
            else if (phase == 8)
            {
                //rejuvenating mantra
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra > 0)
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra--;
                    Heal(8);
                }

                //second wind
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SecondWind && noEncounter)
                {
                    Heal(8);
                }
                noEncounter = false;

                //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedTurn = false;
                resolveEncounterButton.SetActive(false);
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedTurn = true;
                if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    int level = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level);
                    bool repeat = false;
                    level++;
                    if (player.GetComponent<movement>().area == 1 && level > 10)
                    {
                        level = 10;
                        repeat = true;
                    }
                    else if (player.GetComponent<movement>().area == 2 && level > 20)
                    {
                        level = 20;
                        repeat = true;
                    }
                    else if (player.GetComponent<movement>().area == 3 && level > 30)
                    {
                        level = 30;
                        repeat = true;
                    }
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level = level + "";

                    if (level % 5 == 0 && !repeat)
                    {
                        levelImprovementScreen.SetActive(true);
                    }
                }
                gameObject.GetComponent<gameMenuManager>().fullMap.transform.localPosition = new Vector2(140, 0);
                phase++;
            }
        }
        else
        {
            phase = 0;
        }

        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().SearchedTreasureTroves.Length; i++)
        {
            //for (int j = 0; j < unsearchedTroves.Length; j++)
            //{
            //    if (unsearchedTroves[j] == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().SearchedTreasureTroves[i])
            //    {
            //        unsearchedTroves[j] = null;
            //        trovesPieces[j].SetActive(false);
            //    }
            //}
            if (unsearchedTroves[i] == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().SearchedTreasureTroves[i])
            {
                unsearchedTroves[i] = null;
                trovesPieces[i].SetActive(false);
            }
        }

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedTurn = true;
        //    //serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator++;
        //}
    }

    public void IncrementPhase()
    {
        gameObject.GetComponent<gameMenuManager>().exit();
        gameObject.GetComponent<gameMenuManager>().map();
        phase++;
        continueButton.SetActive(false);
    }

    public void DrawEncounter()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter = serverInfo.GetComponent<BoltEntity>().GetComponent<serverInfoBehaviour>().drawEncounterCard();
        encounterDeck.SetActive(false);
        encounterDisplay.SetActive(true);
        encounterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter, typeof(Sprite));
        quest.GetComponent<questController>().encounterCardsDrawn++;
        phase++;

        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " encountered something!");
    }

    public void BurnEncounter()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter = true;
        encounterDisplay.SetActive(true);
        encounterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + serverInfo.GetComponent<BoltEntity>().GetComponent<serverInfoBehaviour>().drawEncounterCard(), typeof(Sprite));
    }

    public void DrawTreasureTrove()
    {
        treasureTrove.SetActive(false);
        treasure1 = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator];
        treasure2 = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + 1];
        treasure3 = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + 2];
        treasureTroveDisplay1.SetActive(true);
        treasureTroveDisplay2.SetActive(true);
        treasureTroveDisplay3.SetActive(true);
        treasureTroveDisplay1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + treasure1, typeof(Sprite));
        treasureTroveDisplay2.GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + treasure2, typeof(Sprite));
        treasureTroveDisplay3.GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + treasure3, typeof(Sprite));

        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SearchedTreasureTrove = true;
        phase++;
    }

    public void MovementRoll()
    {
        movementButton.SetActive(false);
        int rando = Random.Range(3, 6);
        player.GetComponent<movement>().moveRadius = rando;
        movementDisplay.GetComponent<Text>().text = rando + "";
        StartCoroutine(GenerateMoveArea());
    }

    public void EncounterRoll()
    {
        encounterButton.SetActive(false);
        int rando = Random.Range(1, 7);
        encounterRollDisplay.GetComponent<Text>().text = rando + "";
        if (rando % 2 == 1)
        {
            if (!fails[0])
                fails[0] = true;
            else if (!fails[1])
                fails[1] = true;

            if (fails[0])
                encounterFails[0].SetActive(true);
            else
                encounterFails[0].SetActive(false);
            if (fails[1])
                encounterFails[1].SetActive(true);
            else
                encounterFails[1].SetActive(false);
            StartCoroutine(NoEncounter());
        }
        else
        {
            fails[0] = false;
            fails[1] = false;
            StartCoroutine(YesEncounter());
        }
    }

    IEnumerator YesEncounter()
    {
        yield return new WaitForSeconds(1);
        encounterRollDisplay.GetComponent<Text>().text = "";
        rollForEncounter.SetActive(false);
        encounterDeck.SetActive(true);
    }

    IEnumerator NoEncounter()
    {
        yield return new WaitForSeconds(1);
        encounterRollDisplay.GetComponent<Text>().text = "";
        rollForEncounter.SetActive(false);
        phase = finalPhase;
        noEncounter = true;
    }

    IEnumerator GuaranteedEncounter()
    {
        fails[0] = false;
        fails[1] = false;
        guaranteedEncounterMessage.SetActive(true);
        yield return new WaitForSeconds(1);
        guaranteedEncounterMessage.SetActive(false);
        encounterDeck.SetActive(true);
    }

    IEnumerator GenerateMoveArea()
    {
        yield return new WaitForSeconds(1);
        movementDisplay.GetComponent<Text>().text = "";
        rollForMovement.SetActive(false);
        player.GetComponent<movement>().calcMovement();
    }

    IEnumerator DecayTimer(GameObject g)
    {
        yield return new WaitForSeconds(1);
        phase++;
        g.SetActive(false);
    }

    public void KeepTreasure1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[0] = treasure1;
        treasureTroveDisplay1.SetActive(false);
        treasureTroveDisplay2.SetActive(false);
        treasureTroveDisplay3.SetActive(false);
        quest.GetComponent<questController>().treasureCardsDrawn++;
        phase++;
    }

    public void KeepTreasure2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[0] = treasure2;
        treasureTroveDisplay1.SetActive(false);
        treasureTroveDisplay2.SetActive(false);
        treasureTroveDisplay3.SetActive(false);
        quest.GetComponent<questController>().treasureCardsDrawn++;
        phase++;
    }

    public void KeepTreasure3()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[0] = treasure3;
        treasureTroveDisplay1.SetActive(false);
        treasureTroveDisplay2.SetActive(false);
        treasureTroveDisplay3.SetActive(false);
        quest.GetComponent<questController>().treasureCardsDrawn++;
        phase++;
    }

    public void payout1()
    {
        //10 gold for each monster killed
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        int killed = quest.GetComponent<questController>().monstersKilled;
        gold += 10 * killed;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";

        npc.SetActive(false);
        endTurn();
    }
    public void payout2()
    {
        //discard card for 2 ability charges
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[0] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[0] != "")
        {
            discardDisplay.SetActive(true);
            int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
            ac += 2;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";

            npc.SetActive(false);
        }
        else
            error2.SetActive(true);
    }
    public void payout3()
    {
        //plus to to power rolls for next combat excluding boss fights
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ElvenHunterBoon = true;
        npc.SetActive(false);
        endTurn();
    }
    public void payout4()
    {
        //roll, and on 4 or higher reveal boss + gain ability charge, else discard 2 ability charges or take 9 damage for each missing one
        ominousCatRollNumber.GetComponent<Text>().text = "";
        ominousCatRollButton.SetActive(true);
        ominousCatRollDisplay.SetActive(true);
    }
    public void payout5()
    {
        //discard ability charges (max 7) to draw cards of same amount
        abilityChargesToDiscard = 0;
        abilityChargeDiscardScreen.SetActive(true);

        npc.SetActive(false);
    }
    public void payout6()
    {
        //pay 25 gold to heal to full health and to charm next non-boss npc
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        if (gold >= 25)
        {
            gold -= 25;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
            int maxHP = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
            Heal(maxHP);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CharmingBardBoon = true;
            npc.SetActive(false);
            endTurn();
        }
        else
            error1.SetActive(true);
    }
    public void payout7()
    {
        mermaidScreen.SetActive(true);   
        npc.SetActive(false);
    }
    public void payoutPass()
    {
        npc.SetActive(false);
        endTurn();
    }

    public void buyItem1()
    {
        if (!buyingItem && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[0] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[0] != "")
        {
            if (hasEnoughMoney(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[0]) && (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == ""))
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == "")
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[0];
                        //serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[0] = null;
                        StartCoroutine(BuyItem(0));
                        i = 100;
                    }
                }
            }
            else
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == "")
                    lackSufficientGoldMessage.SetActive(true);
                else
                    needMoreSpaceMessage.SetActive(true);
            }
        }
    }
    public void buyItem2()
    {
        if (!buyingItem && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[1] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[1] != "")
        {
            if (hasEnoughMoney(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[1]) && (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == ""))
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == "")
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[1];
                        StartCoroutine(BuyItem(1));
                        i = 100;
                    }
                }
            }
            else
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == "")
                    lackSufficientGoldMessage.SetActive(true);
                else
                    needMoreSpaceMessage.SetActive(true);
            }
        }
    }
    public void buyItem3()
    {
        if (!buyingItem && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[2] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[2] != "")
        {
            if (hasEnoughMoney(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[2]) && (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == ""))
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == "")
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[2];
                        StartCoroutine(BuyItem(2));
                        i = 100;
                    }
                }
            }
            else
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == "")
                    lackSufficientGoldMessage.SetActive(true);
                else
                    needMoreSpaceMessage.SetActive(true);
            }
        }
    }
    public void buyItem4()
    {
        if (!buyingItem && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[3] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[3] != "")
        {
            if (hasEnoughMoney(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[3]) && (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == ""))
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == "")
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[3];
                        StartCoroutine(BuyItem(3));
                        i = 100;
                    }
                }
            }
            else
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == "")
                    lackSufficientGoldMessage.SetActive(true);
                else
                    needMoreSpaceMessage.SetActive(true);
            }
        }
    }
    public void buyItem5()
    {
        if (!buyingItem && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[4] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[4] != "")
        {
            if (hasEnoughMoney(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[4]) && (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == ""))
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] == "")
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[4];
                        StartCoroutine(BuyItem(4));
                        i = 100;
                    }
                }
            }
            else
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] == "")
                    lackSufficientGoldMessage.SetActive(true);
                else
                    needMoreSpaceMessage.SetActive(true);
            }
        }
    }

    IEnumerator BuyItem(int id)
    {
        buyingItem = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BoughtItem = id;
        yield return new WaitForSeconds(1);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BoughtItem = -1;
        buyingItem = false;
    }

    public bool hasEnoughMoney(string card)
    {
        int price = 0;
        int playerMoney = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);

        price = priceIndex(card);

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Halfling")
        {
            price = (int)(price * 2f / 3f);
        }

        if (price > playerMoney)
        {
            return false;
        }
        int newTotal = playerMoney - price;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = newTotal + "";
        return true;
    }

    public void sellCard1()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[0]) * 2f/3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[0] = null;
    }
    public void sellCard2()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[1]) * 2f / 3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[1] = null;
    }
    public void sellCard3()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[2]) * 2f / 3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[2] = null;
    }
    public void sellCard4()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[3]) * 2f / 3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[3] = null;
    }
    public void sellCard5()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4]) * 2f / 3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = null;
    }
    public void sellCard6()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[5]) * 2f / 3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[5] = null;
    }
    public void sellCard7()
    {
        int gold = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold);
        gold += (int)(priceIndex(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6]) * 2f / 3f);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold = gold + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = null;
    }

    public int priceIndex(string card)
    {
        int price;
        switch (card)
        {
            case "t0": price = 10; break;
            case "t1": price = 15; break;
            case "t2": price = 20; break;
            case "t3": price = 30; break;
            case "t4": price = 10; break;
            case "t5": price = 20; break;
            case "t6": price = 30; break;
            case "t7": price = 20; break;
            case "t8": price = 15; break;
            case "t9": price = 10; break;
            case "t10": price = 10; break;
            case "t11": price = 10; break;
            case "a0": price = 50; break;
            case "a1": price = 90; break;
            case "a2": price = 70; break;
            case "a3": price = 40; break;
            case "a4": price = 50; break;
            case "a5": price = 90; break;
            case "a6": price = 70; break;
            case "a7": price = 40; break;
            case "a8": price = 50; break;
            case "a9": price = 90; break;
            case "a10": price = 70; break;
            case "a11": price = 40; break;
            case "a12": price = 110; break;
            case "a13": price = 150; break;
            case "a14": price = 130; break;
            case "a15": price = 100; break;
            case "a16": price = 110; break;
            case "a17": price = 150; break;
            case "a18": price = 130; break;
            case "a19": price = 100; break;
            case "a20": price = 110; break;
            case "a21": price = 150; break;
            case "a22": price = 130; break;
            case "a23": price = 100; break;
            case "w0": price = 60; break;
            case "w1": price = 30; break;
            case "w2": price = 30; break;
            case "w3": price = 60; break;
            case "w4": price = 60; break;
            case "w5": price = 30; break;
            case "w6": price = 60; break;
            case "w7": price = 120; break;
            case "w8": price = 90; break;
            case "w9": price = 90; break;
            case "w10": price = 120; break;
            case "w11": price = 120; break;
            case "w12": price = 90; break;
            case "w13": price = 120; break;
            default: price = 1000; break;
        }
        return price;
    }

    public void leaveStore()
    {
        store.SetActive(false);
        phase = finalPhase;
    }

    public void burnCard()
    {
        encounterDisplay.SetActive(false);
        burnCardButton.SetActive(false);
        phase++;
    }

    public void ResolveEncounter()
    {
        resolveEncounterButton.SetActive(false);
        string encounter = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter;

        switch (encounter)
        {
            case "e0"://blocked path
                encounterDisplay.SetActive(false);
                BlockedPath();
                break;
            case "e1"://puzzling door
                encounterDisplay.SetActive(false);
                PuzzlingDoor();
                break;
            case "e2"://winding river
                encounterDisplay.SetActive(false);
                WindingRiver();
                break;
            case "e3"://fork in the road
                encounterDisplay.SetActive(false);
                ForkInTheRoad();
                break;
            case "e4"://abandoned chest
                encounterDisplay.SetActive(false);
                AbandonedChest();
                break;
            case "e5"://nature shrine
                encounterDisplay.SetActive(false);
                NatureShrine();
                break;
            case "e6"://rickety bridge
                encounterDisplay.SetActive(false);
                RicketyBridge();
                break;
            case "e7"://quicksand
                encounterDisplay.SetActive(false);
                Quicksand();
                break;
            case "e8"://mysterious orb
                MysteriousOrb();
                break;
            case "e9"://collapsed building
                encounterDisplay.SetActive(false);
                CollapsedBuilding();
                break;
            case "e10"://trading post
                TradingPost();
                break;
            case "e11"://portal system
                encounterDisplay.SetActive(false);
                PortalSystem();
                break;
            case "e12"://campsite
                encounterDisplay.SetActive(false);
                Campsite();
                break;
            case "e13"://cathedral
                encounterDisplay.SetActive(false);
                Cathedral();
                break;
            case "e14"://sacred forest
                encounterDisplay.SetActive(false);
                SacredForest();
                break;
            case "e15"://combat arena
                encounterDisplay.SetActive(false);
                CombatArena();
                break;
            case "e16"://great library
                encounterDisplay.SetActive(false);
                GreatLibrary();
                break;
            case "e17"://atherwyn town
                encounterDisplay.SetActive(false);
                AtherwynTown();
                break;
            case "e18"://dorrenik town
                encounterDisplay.SetActive(false);
                DorrenikTown();
                break;
            case "e19"://battleground
                Battleground();
                break;
        }
    }

    public void FightMonster()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CharmingBardBoon = false;
        encounterDisplay.SetActive(false);
        fightMonsterButton.SetActive(false);
        fightMonsterMiniButton.SetActive(false);
        ignoreMonsterMiniButton.SetActive(false);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 1;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = true;
        combatScreen.SetActive(true);
        //phase++;

        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " is entering combat. Open your map!");
    }

    public void IgnoreMonster()
    {
        fightMonsterMiniButton.SetActive(false);
        ignoreMonsterMiniButton.SetActive(false);
        encounterDisplay.SetActive(false);
        endTurn();
    }

    public void endTurn()
    {
        phase = finalPhase;
    }

    void BlockedPath()
    {
        if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod) >= 6)
        {
            DrawCards(1);
            endTurn();
        }
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
        {
            encounterID = "blockedPath";
            masterAdventurer.SetActive(true);
        }
        else
            endTurn();
    }
    void PuzzlingDoor()
    {
        if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod) > 10)
        {
            DrawCards(2);
            endTurn();
        }
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
        {
            encounterID = "puzzlingDoor";
            masterAdventurer.SetActive(true);
        }
        else
            endTurn();
    }
    void WindingRiver()
    {
        if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 5)
        {
            DrawCards(1);
            endTurn();
        }
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
        {
            encounterID = "windingRiver";
            masterAdventurer.SetActive(true);
        }
        else
            endTurn();
    }
    void ForkInTheRoad()
    {
        path1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterIterator], typeof(Sprite));
        path2.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterIterator+1], typeof(Sprite));
        pathData1 = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterIterator];
        pathData2 = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterIterator + 1];
        path1.SetActive(true);
        path2.SetActive(true);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ForkInTheRoadActive = true;
    }
    void AbandonedChest()
    {
        DrawCards(2);
        endTurn();
    }
    void NatureShrine()
    {
        TextAsset textfile = (TextAsset)Resources.Load("Class Info/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class.ToLower() + "_info", typeof(TextAsset));
        List<string> classInfo = new List<string>(textfile.text.Split('\n'));

        int ac = int.Parse(classInfo[1]);
        int playerAC = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);

        while (playerAC < ac)
        {
            playerAC++;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = playerAC + "";
        }
        endTurn();
    }
    void RicketyBridge()
    {
        if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) < 4)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
            {
                encounterID = "ricketyBridge";
                masterAdventurer.SetActive(true);
            }
            else
            {
                TakeDamage(11);
                endTurn();
            }
        }
        else
            endTurn();
    }
    void Quicksand()
    {
        if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod) <= 2)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
            {
                encounterID = "quicksand";
                masterAdventurer.SetActive(true);
            }
            else
            {
                TakeDamage(24);
                endTurn();
            }
        }
        else
            endTurn();
    }
    void MysteriousOrb()
    {
        mysteriousOrbRollNumber.GetComponent<Text>().text = "";
        mysteriousOrbRollButton.SetActive(true);
        mysteriousOrbRollDisplay.SetActive(true);
    }
    void CollapsedBuilding()
    {
        if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod) < 5)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
            {
                encounterID = "collapsedBuilding";
                masterAdventurer.SetActive(true);
            }
            else
            {
                TakeDamage(12);
                endTurn();
            }
        }
        else
            endTurn();
    }
    void TradingPost()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive = true;
        resolveEncounterButton.SetActive(false);
        endTurnButton.SetActive(true);
    }
    void PortalSystem()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int k = 0; k < playerPieces.Length; k++)
            {
                if (tiles[i].GetComponent<Text>().text == player.GetComponent<Text>().text || tiles[i].GetComponent<Text>().text == playerPieces[k].GetComponent<Text>().text)
                    tiles[i].GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
    void Campsite()
    {
        Heal(int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth));
        endTurn();
    }
    void Cathedral()
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Paladin" || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Cleric")
            DrawCards(1);
        endTurn();
    }
    void SacredForest()
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Druid" || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Hunter")
            DrawCards(1);
        endTurn();
    }
    void CombatArena()
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Warrior" || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Rogue")
            DrawCards(1);
        endTurn();
    }
    void GreatLibrary()
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Sorcerer" || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Necromancer")
            DrawCards(1);
        endTurn();
    }
    void AtherwynTown()
    {
        string race = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race;
        if (race == "Human" || race == "High Elf" || race == "Dwarf" || race == "Halfling")
        {
            GainAbilityCharge(2);
            endTurn();
        }
        else
        {
            bool hasCards = false;
            for (int i = 0; i < 7; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
                    hasCards = true;
            }
            if (hasCards)
                discardDisplay.SetActive(true);
            else
                endTurn();
        }
    }
    void DorrenikTown()
    {
        string race = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race;
        if (race == "Orc" || race == "Centaur" || race == "Fairy" || race == "Night Elf")
        {
            GainAbilityCharge(2);
            endTurn();
        }
        else
        {
            bool hasCards = false;
            for (int i = 0; i < 7; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
                    hasCards = true;
            }
            if (hasCards)
                discardDisplay.SetActive(true);
            else
                endTurn();
        }
    }
    void Battleground()
    {
        battlegroundRollNumber.GetComponent<Text>().text = "";
        battlegroundRollButton.SetActive(true);
        battlegroundRollDisplay.SetActive(true);
    }

    public void EndTradingPost()
    {
        encounterDisplay.SetActive(false);
        resolveEncounterButton.SetActive(false);
        endTurnButton.SetActive(false);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive = false;
        endTurn();
    }

    public void RollForBattleground()
    {
        battlegroundRollButton.SetActive(false);
        int rando = Random.Range(1, 7);
        battlegroundRollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(BattlegroundLinger(rando));
    }

    IEnumerator BattlegroundLinger(int roll)
    {
        yield return new WaitForSeconds(1);
        switch (roll)
        {
            case 1:
                TakeDamage(15);
                break;
            case 2:
                GainAbilityCharge(1);
                break;
            case 3:
                DrawCards(1);
                break;
            case 4:
                GainAbilityCharge(-1);
                break;
            case 5:
                discardDisplay.SetActive(true);
                break;
            case 6:
                Heal(15);
                break;
        }
        battlegroundRollDisplay.SetActive(false);
        encounterDisplay.SetActive(false);
        if (roll != 5)
            endTurn();
    }

    public void RollForMysteriousOrb()
    {
        mysteriousOrbRollButton.SetActive(false);
        int rando = Random.Range(1, 7);
        mysteriousOrbRollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(MysteriousOrbLinger(rando));
    }

    IEnumerator MysteriousOrbLinger(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll >= 4)
        {
            bossRevealScreen.SetActive(true);
            mysteriousOrbRollDisplay.SetActive(false);
            encounterDisplay.SetActive(false);
        }
        else
        {
            TakeDamage(9);
            mysteriousOrbRollDisplay.SetActive(false);
            encounterDisplay.SetActive(false);
            endTurn();
        }
    }

    public void RollForOminousCat()
    {
        ominousCatRollButton.SetActive(false);
        int rando = Random.Range(1, 7);
        ominousCatRollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(OminousCatLinger(rando));
    }

    IEnumerator OminousCatLinger(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll >= 4)
        {
            bossRevealScreen.SetActive(true);
            int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
            ac += 1;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";

            ominousCatRollDisplay.SetActive(false);
            npc.SetActive(false);
        }
        else
        {
            int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
            if (ac > 0)
                ac -= 1;
            else
                TakeDamage(9);
            if (ac > 0)
                ac -= 1;
            else
                TakeDamage(9);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
            ominousCatRollDisplay.SetActive(false);
            npc.SetActive(false);
            endTurn();
        }
    }

    void DrawCards(int cards)
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CardsDrawing = cards;
        for (int i = 0; i < cards; i++)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + i];
        }
        quest.GetComponent<questController>().treasureCardsDrawn += cards;
    }

    public void TakeDamage(int amount)
    {
        //divine protection
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection)
        {
            amount = 0;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection = false;
        }

        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
        amount -= int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction);
        if (amount < 0)
            amount = 0;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Human")
        {
            if (health >= maxHealth / 2f && health - amount < maxHealth / 2f)
            {
                int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
                ac++;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
            }
        }
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Djinni" && amount > 0)
        {
            int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
            ac--;
            if (ac < 0)
                ac = 0;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
        }
        health -= amount;
        if (health < 0)
            health = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
    }

    void Heal(int amount)
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
    }

    void GainAbilityCharge(int amount)
    {
        int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
        ac += amount;
        if (ac < 0)
            ac = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
    }

    public void Discard1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[0] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }
    public void Discard2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[1] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }
    public void Discard3()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[2] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }
    public void Discard4()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[3] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }
    public void Discard5()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }
    public void Discard6()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[5] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }
    public void Discard7()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = null;
        discardDisplay.SetActive(false);
        endTurn();
    }

    public void ChoosePath1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter = pathData1;
        path1.SetActive(false);
        path2.SetActive(false);
        encounterDisplay.SetActive(true);
        encounterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter, typeof(Sprite));
        quest.GetComponent<questController>().encounterCardsDrawn++;
        branch = 0;
        phase = 7;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ForkInTheRoadActive = false;
    }
    public void ChoosePath2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter = pathData2;
        path1.SetActive(false);
        path2.SetActive(false);
        encounterDisplay.SetActive(true);
        encounterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter, typeof(Sprite));
        quest.GetComponent<questController>().encounterCardsDrawn++;
        branch = 0;
        phase = 7;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ForkInTheRoadActive = false;
    }

    public void IncrementAbilityChargesToDiscard()
    {
        abilityChargesToDiscard++;
    }
    public void DecrementAbilityChargesToDiscard()
    {
        abilityChargesToDiscard--;
    }
    public void DiscardAbilityCharges()
    {
        abilityChargeDiscardScreen.SetActive(false);
        int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
        ac -= abilityChargesToDiscard;
        //if (ac < 0)
        //    ac = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
        DrawCards(abilityChargesToDiscard);
        endTurn();
    }

    public void StartBossFight()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight = true;
        bossFightButtons.SetActive(false);
        phase = 7;

        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 1;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = true;
        combatScreen.SetActive(true);

        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " is starting a Boss Fight. Open your map!");
    }

    public void WaitOnBossFight()
    {
        ignoredBossFight = true;
        bossFightButtons.SetActive(false);
        phase--;
    }

    public void MasterAdventurerRoll()
    {
        masterAdventurerButton.SetActive(false);
        int rando = Random.Range(1, 7);
        masterAdventurerNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(ProcessLag(rando));
    }

    IEnumerator ProcessLag(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll >= 4)
        {
            switch (encounterID)
            {
                case "blockedPath":
                    DrawCards(1);
                    break;
                case "puzzlingDoor":
                    DrawCards(2);
                    break;
                case "windingRiver":
                    DrawCards(1);
                    break;
                case "ricketyBridge":
                    break;
                case "quicksand":
                    break;
                case "collapsedBuilding":
                    break;
            }
        }
        else
        {
            switch (encounterID)
            {
                case "blockedPath":
                    break;
                case "puzzlingDoor":
                    break;
                case "windingRiver":
                    break;
                case "ricketyBridge":
                    TakeDamage(11);
                    break;
                case "quicksand":
                    TakeDamage(24);
                    break;
                case "collapsedBuilding":
                    TakeDamage(12);
                    break;
            }
        }

        masterAdventurer.SetActive(false);
        masterAdventurerButton.SetActive(true);
        masterAdventurerNumber.GetComponent<Text>().text = "";
        endTurn();
    }
}
