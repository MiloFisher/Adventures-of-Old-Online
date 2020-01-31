using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class combatResolution : MonoBehaviour
{
    public GameObject combatScreen;
    public GameObject combatCycle;
    public GameObject previewPhase;
    public bool victory;
    public GameObject monsterDisplay;
    public GameObject successBox;
    public GameObject failBox;
    public GameObject drawTreasureButton;
    public GameObject leaveCombatButton;
    public GameObject rollForLootButton;
    public GameObject lootRollDisplay;
    public GameObject lootRollNumber;
    public GameObject drawLegendaryButton;
    public GameObject generalScripts;
    public GameObject quest;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private string monster;
    private bool isSpectating;
    private bool left;
    private bool everyoneDone;
    private List<int> rolls = new List<int>();
    private bool pickedWinner;
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
        lootRollDisplay.SetActive(false);
        drawLegendaryButton.SetActive(false);
        drawTreasureButton.SetActive(false);
        leaveCombatButton.SetActive(false);
    }

    private void OnEnable()
    {
        pickedWinner = false;
    }

    // Update is called once per frame
    void Update()
    {
        previewPhase.GetComponent<previewPhaseController>().orcDamageBuff = 0;
        previewPhase.GetComponent<previewPhaseController>().bonusDamage = 0;

        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth <= 0)
            victory = true;
        else
            victory = false;

        combatCycle.GetComponent<combatCycle>().phase = 0;
        isSpectating = false;
        for (int i = 0; i < 5; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == characterName)
            {
                isSpectating = true;
            }
        }

        if (isSpectating)
        {
            LeaveCombat();
        }
        else
        {
            //monster display
            monster = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Monster;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + monster, typeof(Sprite));
            else
                monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + monster, typeof(Sprite));

            //victory and defeat display
            if (victory)
            {
                successBox.SetActive(true);
                failBox.SetActive(false);

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Fairy" && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth;

                //==============
                int treasureReciever = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[treasureReciever] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                    {
                        treasureReciever++;
                        i = 0;
                    }
                }
                //==============
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    rollForLootButton.SetActive(true);
                }
                else if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 19 && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)//Enraged Goliath Ability - Mini Boss
                {
                    rollForLootButton.SetActive(true);
                }
                else
                {
                    if (characterName == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[treasureReciever] && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                    {
                        drawTreasureButton.SetActive(true);
                        leaveCombatButton.SetActive(false);
                        rollForLootButton.SetActive(false);
                    }
                    else
                    {
                        drawTreasureButton.SetActive(false);
                        leaveCombatButton.SetActive(false);
                        rollForLootButton.SetActive(false);
                        if (!left)
                            StartCoroutine(waitThenLeave());
                    }
                }
            }
            else
            {
                rollForLootButton.SetActive(false);
                drawTreasureButton.SetActive(false);
                successBox.SetActive(false);
                failBox.SetActive(true);
                leaveCombatButton.SetActive(false);
                if (!left)
                    StartCoroutine(waitThenLeave());
            }
        }

        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().LegendaryReceiver != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().LegendaryReceiver != "" && !pickedWinner)
        {
            pickedWinner = true;
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().LegendaryReceiver == characterName)
            {
                RegenerateAC();
                drawLegendaryButton.SetActive(true);
                //increment server area
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().KilledBoss = true;
                Debug.Log("Increment Server Area");
            }
            else
            {
                RegenerateAC();
                drawTreasureButton.SetActive(true);
            }
        }
        /*
        //check if everyone has rolled for loot
        rolls.Clear();
        everyoneDone = true;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
        {
            for (int k = 0; k < players.Length; k++)
            {
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name && players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RolledForLoot == false && !players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll == 0)
                        everyoneDone = false;
                    else
                        rolls.Add(k);
                }
            }
        }

        if(everyoneDone && rolls.Count > 0 && !pickedWinner)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RolledForLoot = true;
            int highest = 0;
            int highestPlayer = 0;
            List<int> tiedPlayers = new List<int>();
            for (int i = 0; i < rolls.Count; i++)
            {
                Debug.Log(players[rolls[i]].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " rolled a " + players[rolls[i]].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll);
                if (players[rolls[i]].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll > highest)
                {
                    highest = players[rolls[i]].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll;
                    highestPlayer = rolls[i];
                }
                else if (players[rolls[i]].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll == highest)
                {
                    tiedPlayers.Add(rolls[i]);
                }
            }
            Debug.Log(players[highestPlayer].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " rolled the highest with " + highest);

            lootRollDisplay.SetActive(false);
            if (tiedPlayers.Count == 0)
            {
                pickedWinner = true;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == players[highestPlayer].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    RegenerateAC();
                    drawLegendaryButton.SetActive(true);
                    //increment server area
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().KilledBoss = true;
                    Debug.Log("Increment Server Area");
                }
                else if(!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    RegenerateAC();
                    drawTreasureButton.SetActive(true);
                }
                else
                {
                    
                }
            }
            else
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll < highest)
                {
                    drawTreasureButton.SetActive(true);
                }
                else
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RolledForLoot = false;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll = 0;
                }
            }
        }
        */
    }

    void RegenerateAC()
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
    }

    public void RollForLoot()
    {
        int rando = Random.Range(1, 7);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll = rando;
        lootRollNumber.GetComponent<Text>().text = rando + "";
        lootRollDisplay.SetActive(true);
    }

    public void DrawLegendary()
    {
        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " found a Legendary Item!");

        drawLegendaryButton.SetActive(false);
        string legendary = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().LegendaryDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area - 1];
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] = legendary;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] == null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] == "")
            {
                serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] = legendary;
                i = 100;
            }
        }
        lootRollNumber.GetComponent<Text>().text = "";
        lootRollDisplay.SetActive(false);


        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth <= 0 && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth)
            quest.GetComponent<questController>().monstersKilled++;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ElvenHunterBoon = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastSalvation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastDivineProtection = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedicCast = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanRaiseTheDead = true;
        if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "Undead Legion")
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ThickHide = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasUsedMasterCombatant = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RelentlessAssault = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming = false;
        LeaveCombat();
    }

    public void DrawTreasures()
    {
        lootRollNumber.GetComponent<Text>().text = "";
        lootRollDisplay.SetActive(false);

        drawTreasureButton.SetActive(false);
        int drawnCards = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterTreasures;

        //Scavenging Looter Ability - Treasure Hoarder
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 10)
        {
            int items = 0;
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
                    items++;
            }
            drawnCards = items;
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth)
            drawnCards--;
        if (drawnCards < 0)
            drawnCards = 0;

        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CardsDrawing = drawnCards;
        for (int i = 0; i < drawnCards; i++)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + i];
        }
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth <= 0 && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth)
            quest.GetComponent<questController>().monstersKilled++;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ElvenHunterBoon = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastSalvation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastDivineProtection = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedicCast = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanRaiseTheDead = true;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "Undead Legion")
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ThickHide = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasUsedMasterCombatant = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RelentlessAssault = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming = false;
        LeaveCombat();
    }

    public void LeaveCombat()
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = 1 + "";
        }
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().KilledBoss = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RolledForLoot = false;
        leaveCombatButton.SetActive(false);
        //leave boss fight
        
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll = 0;

        //==== runes expire ====
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ValorRunesActive; i++)
        {
            int mod = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);
            mod -= 5;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod = mod + "";
        }
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SwiftnessRunesActive; i++)
        {
            int mod = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
            mod -= 5;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod = mod + "";
        }
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WisdomRunesActive; i++)
        {
            int mod = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
            mod -= 5;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod = mod + "";
        }
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ValorRunesActive = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SwiftnessRunesActive = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WisdomRunesActive = 0;
        //======================

        combatScreen.SetActive(false);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedCombatTurn = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LeftGroup = true;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 0;
        
        if (generalScripts.GetComponent<turnManager>().phase == 7 && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator])
        {
            generalScripts.GetComponent<turnManager>().endTurn();
        }
        generalScripts.GetComponent<gameMenuManager>().exit();
        if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
            generalScripts.GetComponent<gameMenuManager>().quest();
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight = false;
    }

    IEnumerator waitThenLeave()
    {
        left = true;
        yield return new WaitForSeconds(1);
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth <= 0 && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth)
            quest.GetComponent<questController>().monstersKilled++;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ElvenHunterBoon = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastSalvation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastDivineProtection = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedicCast = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanRaiseTheDead = true;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "Undead Legion")
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ThickHide = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasUsedMasterCombatant = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RelentlessAssault = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming = false;
        LeaveCombat();
        left = false;
    }
}
