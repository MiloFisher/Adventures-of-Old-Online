using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class actionPhaseController : MonoBehaviour
{
    public string monster;
    public GameObject generalScripts;
    public GameObject fleeRoll;
    public GameObject fleeRollNumber;
    public GameObject fleeRollButton;
    public GameObject monsterDisplay;
    public GameObject monsterHealthDisplay;
    public GameObject monsterMaxHealthDisplay;
    public GameObject monsterHealthBar;
    public GameObject fightButton;
    public GameObject fleeButton;
    public GameObject[] playerDisplays;
    public GameObject[] playerFrames;
    public GameObject[] playerHealths;
    public GameObject[] playerMaxHealths;
    public GameObject[] backs;
    public GameObject turnMarker;
    public GameObject playerDisplay;
    public GameObject playerHealthDisplay;
    public GameObject playerMaxHealthDisplay;
    public GameObject playerHealthBar;
    public GameObject combatCycle;
    public GameObject[] deadCovers;
    public GameObject playerDeadCover;
    public GameObject[] stoneCovers;
    public GameObject playerStoneCover;
    public GameObject[] dominatedCovers;
    public GameObject playerDominatedCover;
    public GameObject resolution;
    public GameObject levelImprovementScreen;
    public GameObject inventoryDisplay;
    public GameObject abilityWindow;
    public GameObject preview;

    private GameObject player;
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private float length;
    private float offset;
    private int combatants;
    private bool fleeing;
    private bool alreadyFleed;
    private int pastStreak = 0;
    private bool alreadyHit;
    private int lastTick = 0;
    private bool dominationHit;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        inventoryDisplay.SetActive(false);
    }

    private void OnEnable()
    {
        dominationHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        //taunt
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt)
        {
            preview.GetComponent<previewPhaseController>().succeeded = false;
            combatCycle.GetComponent<combatCycle>().phase += 2;
        }

        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator] == characterName)
        {
            //Magistrax the Gorgon Ability - Turned to Stone
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone)
            {
                generalScripts.GetComponent<turnManager>().TakeDamage(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterDamage);
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone = false;
                //combatCycle.GetComponent<combatCycle>().ResetPhase();
            }

            //Chel'xith, Hell Lord Ability - Domination
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().DominatedPlayer && !dominationHit)
            {
                generalScripts.GetComponent<turnManager>().TakeDamage(5 + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction));
                dominationHit = true;
                //combatCycle.GetComponent<combatCycle>().ResetPhase();
            }
        }

        //Spider Queen Aranne Ability - Venomous Bite
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnTick != lastTick)
        {
            generalScripts.GetComponent<turnManager>().TakeDamage(3 + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction));
            lastTick = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnTick;
        }
        if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned)
            lastTick = 0;

        //Goblin Horde Ability - We've got you surrounded
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 14)
            alreadyFleed = true;

        //Cinderosa Ability - Punishing Flames
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 20)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CinderosaStreak > 1 && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CinderosaStreak != pastStreak)
            {
                pastStreak = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CinderosaStreak;
                generalScripts.GetComponent<turnManager>().TakeDamage(pastStreak * 3 + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction));
                Debug.Log("Cinderosa's effect went off");
            }
            else if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CinderosaStreak == 0)
            {
                pastStreak = 0;
            }
        }

        ////Ophretes the Kraken Ability - Thrashing Waves
        //if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 25)
        //{
        //    bool someoneHit = false;
        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitOphretes)
        //        {
        //            someoneHit = true;
        //        }
        //    }

        //    if(someoneHit && !alreadyHit)
        //    {
        //        generalScripts.GetComponent<turnManager>().TakeDamage(3 + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction));
        //        alreadyHit = true;
        //    }

        //    if (!someoneHit)
        //        alreadyHit = false;
        //}

        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth <= 0)
        {
            resolution.GetComponent<combatResolution>().victory = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 3;
        }

        //clears monster abilities
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BitByCrocodile = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WolfHeal = false;

        //clear outgoing damage
        if (!inventoryDisplay.activeInHierarchy && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = 0;

            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator] != characterName)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedCombatTurn = false;
            }

        
            //monster display
            monster = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Monster;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
            monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + monster, typeof(Sprite));
        else
            monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + monster, typeof(Sprite));

        //monster health display
        monsterHealthDisplay.GetComponent<Text>().text = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth + "/";
            monsterMaxHealthDisplay.GetComponent<Text>().text = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterMaxHealth + "";
            length = (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth / (float)serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterMaxHealth) * 350;
            monsterHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(length, 12);
            offset = (length - 350) / 2f;
            monsterHealthBar.transform.localPosition = new Vector2(offset, 0);
            if (length / 350f > .67f)
                monsterHealthBar.GetComponent<Image>().color = Color.green;
            else if (length / 350f > .33f)
                monsterHealthBar.GetComponent<Image>().color = Color.yellow;
            else
                monsterHealthBar.GetComponent<Image>().color = Color.red;

            //player health display
            playerHealthDisplay.GetComponent<Text>().text = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health + "/";
            playerMaxHealthDisplay.GetComponent<Text>().text = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth;
            length = (float.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health) / float.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth)) * 270;
            playerHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(length, 12);
            offset = (length - 270) / 2f;
            playerHealthBar.transform.localPosition = new Vector2(offset, 0);
            if (length / 270f > .67f)
                playerHealthBar.GetComponent<Image>().color = Color.green;
            else if (length / 270f > .33f)
                playerHealthBar.GetComponent<Image>().color = Color.yellow;
            else
                playerHealthBar.GetComponent<Image>().color = Color.red;


        //dead covers
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
        {
            playerDeadCover.SetActive(true);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone = false;
        }
        else
            playerDeadCover.SetActive(false);

            for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
            {
                for (int k = 0; k < players.Length; k++)
                {
                    if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i])
                    {
                        if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                            deadCovers[i].SetActive(true);
                        else
                            deadCovers[i].SetActive(false);
                    }
                }
            }

        //stone covers
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone)
            playerStoneCover.SetActive(true);
        else
            playerStoneCover.SetActive(false);

        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
        {
            for (int k = 0; k < players.Length; k++)
            {
                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i])
                {
                    if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone)
                        stoneCovers[i].SetActive(true);
                    else
                        stoneCovers[i].SetActive(false);
                }
            }
        }

        //dominated covers
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().DominatedPlayer)
            playerDominatedCover.SetActive(true);
        else
            playerDominatedCover.SetActive(false);

        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
        {
            if(serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().DominatedPlayer)
                dominatedCovers[i].SetActive(true);
            else
                dominatedCovers[i].SetActive(false);
        }

        //player controls based on turn
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator] == characterName)
            {
                fightButton.SetActive(true);
                if (!alreadyFleed && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                    fleeButton.SetActive(true);
                else
                    fleeButton.SetActive(false);
            }
            else
            {
                fightButton.SetActive(false);
                fleeButton.SetActive(false);
            }

            //player display
            combatants = 0;
            for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
            {
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != "")
                    combatants++;
            }


            if (combatants < 2)
                playerDisplay.SetActive(false);
            else
            {
                playerDisplay.SetActive(true);
                for (int k = 0; k < combatants; k++)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[k])
                        {
                            playerDisplays[k].GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                            playerHealths[k].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health;
                            playerMaxHealths[k].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth;
                            playerFrames[k].SetActive(true);
                            i = 100;
                        }
                    }
                }
                for (int i = combatants; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
                {
                    playerFrames[i].SetActive(false);
                    playerDisplays[i].GetComponent<Image>().sprite = null;
                    playerHealths[i].GetComponent<Text>().text = null;
                    playerMaxHealths[i].GetComponent<Text>().text = null;
                }

                if (combatants == 2)
                    backs[0].SetActive(true);
                else
                    backs[0].SetActive(false);
                if (combatants == 3)
                    backs[1].SetActive(true);
                else
                    backs[1].SetActive(false);
                if (combatants == 4)
                    backs[2].SetActive(true);
                else
                    backs[2].SetActive(false);
                if (combatants == 5)
                    backs[3].SetActive(true);
                else
                    backs[3].SetActive(false);
                if (combatants == 6)
                    backs[4].SetActive(true);
                else
                    backs[4].SetActive(false);

                switch (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator)
                {
                    case 0:
                        turnMarker.transform.localPosition = new Vector2(-20.5f, 7);
                        break;
                    case 1:
                        turnMarker.transform.localPosition = new Vector2(-20.5f, 3);
                        break;
                    case 2:
                        turnMarker.transform.localPosition = new Vector2(-20.5f, -1);
                        break;
                    case 3:
                        turnMarker.transform.localPosition = new Vector2(-20.5f, -5);
                        break;
                    case 4:
                        turnMarker.transform.localPosition = new Vector2(-20.5f, -9);
                        break;
                    case 5:
                        turnMarker.transform.localPosition = new Vector2(-20.5f, -13);
                        break;
                }

            }

        if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Evade)
            StartCoroutine(fleeScreenLinger(10));

        //fleeing control
        if (fleeing)
            fleeRoll.SetActive(true);
        else
            fleeRoll.SetActive(false);
        //}
    }

    public void basicAttack()
    {
        inventoryDisplay.SetActive(false);
        alreadyFleed = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Action = "basicAttack";
        combatCycle.GetComponent<combatCycle>().IncrementPhase();
    }

    public void spellAttack()
    {
        inventoryDisplay.SetActive(false);
        alreadyFleed = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Action = "spellAttack";
        combatCycle.GetComponent<combatCycle>().IncrementPhase();
    }

    public void flee()
    {
        inventoryDisplay.SetActive(false);
        fleeing = true;
        fleeRollNumber.GetComponent<Text>().text = "";
        alreadyFleed = true;
    }

    public void rollForFlee()
    {
        int roll = Random.Range(1, 7);
        fleeRollNumber.GetComponent<Text>().text = roll + "";
        fleeRollButton.SetActive(false);
        StartCoroutine(fleeScreenLinger(roll));
    }

    IEnumerator fleeScreenLinger(int roll)
    {
        yield return new WaitForSeconds(1);
        fleeing = false;
        fleeRollButton.SetActive(true);

        if (roll >= 4)
        {
            alreadyFleed = false;
            //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedCombatTurn = true;
            //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LeftGroup = true;
            //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat = false;
            resolution.GetComponent<combatResolution>().victory = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 3;
            //if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] && generalScripts.GetComponent<turnManager>().phase > 0)
            //    generalScripts.GetComponent<turnManager>().endTurn();
            if (roll == 10)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Evade = false;
        }
    }

    public void Items()
    {
        inventoryDisplay.SetActive(true);
    }

    public void Abilities()
    {
        abilityWindow.SetActive(true);
    }
}
