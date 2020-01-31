using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class combatSpectatorController : MonoBehaviour
{
    public string monster;
    public GameObject monsterDisplay;
    public GameObject monsterHealthDisplay;
    public GameObject monsterMaxHealthDisplay;
    public GameObject monsterHealthBar;
    public GameObject[] playerDisplays;
    public GameObject[] playerFrames;
    public GameObject[] playerHealths;
    public GameObject[] playerMaxHealths;
    public GameObject turnMarker;
    public GameObject[] deadCovers;
    public GameObject resolution;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private float length;
    private float offset;
    private int combatants;
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

        //player display
        combatants = 0;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] != "")
                combatants++;
        }

        if (combatants == 0)// && (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[0] != null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[0] != ""))
        {
            resolution.GetComponent<combatResolution>().victory = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombatPhase = 3;
        }

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

        switch (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator)
        {
            case 0:
                turnMarker.transform.localPosition = new Vector2(-10, -14);
                break;
            case 1:
                turnMarker.transform.localPosition = new Vector2(-6, -14);
                break;
            case 2:
                turnMarker.transform.localPosition = new Vector2(-2, -14);
                break;
            case 3:
                turnMarker.transform.localPosition = new Vector2(2, -14);
                break;
            case 4:
                turnMarker.transform.localPosition = new Vector2(6, -14);
                break;
            case 5:
                turnMarker.transform.localPosition = new Vector2(10, -14);
                break;
        }

        //dead covers
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
    }
}
