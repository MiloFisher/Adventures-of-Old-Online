using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class heal : MonoBehaviour
{
    public GameObject[] playerDisplays;
    public GameObject[] names;

    private GameObject player;
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private bool isCombatant;
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
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            {
                playerDisplays[i].SetActive(true);
                playerDisplays[i].GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                names[i].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
            else
            {
                playerDisplays[i].SetActive(false);
                playerDisplays[i].GetComponent<Image>().sprite = null;
                names[i].GetComponent<Text>().text = "";
            }
        }
        for (int i = players.Length; i < 6; i++)
        {
            playerDisplays[i].SetActive(false);
            playerDisplays[i].GetComponent<Image>().sprite = null;
            names[i].GetComponent<Text>().text = "";
        }
    }

    public void Heal1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 18;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Light")
            HealSelf(18);
        gameObject.SetActive(false);
    }

    public void Heal2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = players[1].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 18;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Light")
            HealSelf(18);
        gameObject.SetActive(false);
    }

    public void Heal3()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = players[2].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 18;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Light")
            HealSelf(18);
        gameObject.SetActive(false);
    }

    public void Heal4()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = players[3].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 18;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Light")
            HealSelf(18);
        gameObject.SetActive(false);
    }

    public void Heal5()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = players[4].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 18;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Light")
            HealSelf(18);
        gameObject.SetActive(false);
    }

    public void Heal6()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = players[5].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 18;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Light")
            HealSelf(18);
        gameObject.SetActive(false);
    }

    void HealSelf(int amount)
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);

        //The Shadow Ability - Lightless Void
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 24)
            health += amount;

        //Spider Queen Aranne Ability - Venomous Bite
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;
        
        if (health > maxHealth)
            health = maxHealth;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
    }
}
