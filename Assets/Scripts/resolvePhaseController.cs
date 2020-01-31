using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class resolvePhaseController : MonoBehaviour
{
    public GameObject combatCycle;
    public GameObject resolution;
    public GameObject levelImprovementScreen;
    public string monster;
    public GameObject monsterDisplay;
    public GameObject monsterHealthDisplay;
    public GameObject monsterMaxHealthDisplay;
    public GameObject monsterHealthBar;
    public GameObject playerHealthDisplay;
    public GameObject playerMaxHealthDisplay;
    public GameObject playerHealthBar;
    public GameObject inventoryDisplay;
    public GameObject abilityWindow;
    public GameObject arrowVolley;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private float length;
    private float offset;
    private int combatants;
    private GameObject player;
    
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

        //ability
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArrowVolley)
            arrowVolley.SetActive(true);
        else
            arrowVolley.SetActive(false);

    }

    public void EndTurn()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = 0;
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth <= 0)
        {
            if (characterName == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatStarter)
            {
                int level = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level);
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterLevel >= level)
                {
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
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DeathBlast)
            {
                int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
                ac += 2;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
            }
        }
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DeathBlast = false;

        if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack = false;

        //Ophretes the Kraken Ability - Thrashing Waves
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 25)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitOphretes = false;

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra > 0)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra--;
            int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
            int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);

            //The Shadow Ability - Lightless Void
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 24)
                hp += 4;

            //Spider Queen Aranne Ability - Venomous Bite
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;
            
            if (hp > mxhp)
                hp = mxhp;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
        }

        inventoryDisplay.SetActive(false);
        combatCycle.GetComponent<combatCycle>().ResetPhase();
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
