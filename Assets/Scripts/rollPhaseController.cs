using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class rollPhaseController : MonoBehaviour
{
    public GameObject combatCycle;
    public GameObject preview;
    public string monster;
    public GameObject monsterDisplay;
    public GameObject monsterPowerDisplay;
    public GameObject playerPower;
    public GameObject[] rollDisplays;
    public GameObject[] rollButtons;
    public GameObject rollAtLeast;
    public GameObject successChance;
    public GameObject continueButton;
    public GameObject successBox;
    public GameObject failBox;
    public GameObject tieBox;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private int monsterPower;
    private int level;
    private int physicalPower;
    private int spellPower;
    private int bonusPower;
    private int power;
    private int[] rolls = new int[2];
    private int minimumRoll;
    private string likelihood;
    private bool succeeded;
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
        tieBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        //monster display
        monster = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Monster;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
            monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + monster, typeof(Sprite));
        else
            monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + monster, typeof(Sprite));
        monsterPower = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterPower + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterBonusPower;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack && !players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                monsterPower -= 2;
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intimidation)
            monsterPower -= 4;

        if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack)
        {
            //Ugly Succubus Ability - Poorly Seduce
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 1)
            {
                if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod) < 3)
                    monsterPower *= 2;
            }

            //player display
            level = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level);
            physicalPower = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PhysicalPower);
            spellPower = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower);

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Action == "basicAttack")
                power = level + physicalPower + bonusPower;
            else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Action == "spellAttack")
                power = level + spellPower + bonusPower;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
                power = level + spellPower + bonusPower;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ElvenHunterBoon)
                power += 2;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Rally && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ElvenHunterBoon && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner)
                power += 2;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark)
                power += 3;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner)
                power += 5;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus)
                power += 2;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CrashingWaves > 0)
                power += 4;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Night Elf" && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[1] == null)
                power += 2;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Preparation)
            {
                int dif = 0;
                int dex = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
                power += dex;
                if (power > monsterPower)
                {
                    dif = power - monsterPower;
                    power = monsterPower;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PreparationAmount = dif;
                }
                else
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PreparationAmount = 0;
                }
            }
        }
        else
        {
            //skeleton display
            spellPower = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower);
            power = 3 * spellPower;
        }

        monsterPowerDisplay.GetComponent<Text>().text = monsterPower + "";
        playerPower.GetComponent<Text>().text = power + "";

        //roll display
        rollDisplays[0].GetComponent<Text>().text = rolls[0] + "";
        rollDisplays[1].GetComponent<Text>().text = rolls[1] + "";
        for (int i = 0; i < 2; i++)
        {
            if (rolls[i] > 0)
                rollButtons[i].SetActive(false);
            else
                rollButtons[i].SetActive(true);
        }

        //description display
        if (monsterPower - power >= 11)
            minimumRoll = 12;
        else if(monsterPower - power == 10)
            minimumRoll = 11;
        else if (monsterPower - power == 9)
            minimumRoll = 10;
        else if (monsterPower - power == 8)
            minimumRoll = 9;
        else if (monsterPower - power == 7)
            minimumRoll = 8;
        else if (monsterPower - power == 6)
            minimumRoll = 7;
        else if (monsterPower - power == 5)
            minimumRoll = 6;
        else if (monsterPower - power == 4)
            minimumRoll = 5;
        else if (monsterPower - power == 3)
            minimumRoll = 4;
        else if (monsterPower - power <= 2)
            minimumRoll = 3;

        rollAtLeast.GetComponent<Text>().text = minimumRoll + "";

        switch (minimumRoll)
        {
            case 3: likelihood = "Very High"; successChance.GetComponent<Text>().color = Color.blue; break;
            case 4: likelihood = "Very High"; successChance.GetComponent<Text>().color = Color.blue; break;
            case 5: likelihood = "High"; successChance.GetComponent<Text>().color = Color.cyan; break;
            case 6: likelihood = "High"; successChance.GetComponent<Text>().color = Color.cyan; break;
            case 7: likelihood = "Average"; successChance.GetComponent<Text>().color = Color.green; break;
            case 8: likelihood = "Average"; successChance.GetComponent<Text>().color = Color.green; break;
            case 9: likelihood = "Low"; successChance.GetComponent<Text>().color = Color.yellow; break;
            case 10: likelihood = "Low"; successChance.GetComponent<Text>().color = Color.yellow; break;
            case 11: likelihood = "Very Low"; successChance.GetComponent<Text>().color = Color.red; break;
            case 12: likelihood = "Very Low"; successChance.GetComponent<Text>().color = Color.red; break;
        }
        successChance.GetComponent<Text>().text = likelihood;

        //move on
        if (rolls[0] > 0 || rolls[1] > 0)
            tieBox.SetActive(false);

        if (rolls[0] > 0 && rolls[1] > 0)
        {
            if (rolls[0] == 1 && rolls[1] == 1)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
                succeeded = false;
                failBox.SetActive(true);
                continueButton.SetActive(true);
            }
            else if (rolls[0] == 6 && rolls[1] == 6)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
                succeeded = true;
                successBox.SetActive(true);
                continueButton.SetActive(true);
            }
            else if (power + rolls[0] + rolls[1] > monsterPower)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
                succeeded = true;
                successBox.SetActive(true);
                continueButton.SetActive(true);
            }
            else if (power + rolls[0] + rolls[1] < monsterPower)
            {
                if(rolls[0] == 1 || rolls[1] == 1)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = true;
                else
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
                succeeded = false;
                failBox.SetActive(true);
                continueButton.SetActive(true);
            }
            else
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
                tieBox.SetActive(true);
                rolls[0] = 0;
                rolls[1] = 0;
            }
        }
        else
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
            successBox.SetActive(false);
            failBox.SetActive(false);
            continueButton.SetActive(false);
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Focus)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Focus = false;
            if (rolls[0] == 1)
                rolls[0] = 0;
            else
                rolls[1] = 0;
        }
    }

    public void Roll1()
    {
        rolls[0] = Random.Range(1, 7);
    }

    public void Roll2()
    {
        rolls[1] = Random.Range(1, 7);
    }

    public void Back()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Action = null;
        combatCycle.GetComponent<combatCycle>().DecrementPhase();
    }

    public void Continue()
    {
        //deactivate 1 turn effects
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastFocus = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Rally = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intimidation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = 0;

        preview.GetComponent<previewPhaseController>().succeeded = succeeded;
        combatCycle.GetComponent<combatCycle>().IncrementPhase();

        if (rolls[0] == 6 && rolls[1] == 6)
            preview.GetComponent<previewPhaseController>().crit = true;
        else
            preview.GetComponent<previewPhaseController>().crit = false;
        rolls[0] = 0;
        rolls[1] = 0;
    }
}
