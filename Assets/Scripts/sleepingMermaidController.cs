using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class sleepingMermaidController : MonoBehaviour
{
    public GameObject generalScripts;
    public GameObject[] playerDisplays;
    public GameObject[] names;
    public GameObject error;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private int deadPeople;
    private int[] amounts = new int[5];
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
        error.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        deadPeople = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            {
                playerDisplays[i].SetActive(true);
                playerDisplays[i].GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                names[i].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Life and Death")
                    amounts[deadPeople] = int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                else
                    amounts[deadPeople] = 15;
                deadPeople++;
            }
        }
        for (int i = deadPeople; i < 5; i++)
        {
            playerDisplays[i].SetActive(false);
            playerDisplays[i].GetComponent<Image>().sprite = null;
            names[i].GetComponent<Text>().text = "";
        }
    }

    public void Done()
    {
        generalScripts.GetComponent<turnManager>().endTurn();
        gameObject.SetActive(false);
    }

    public void Resurrect1()
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        if (health <= 15)
            error.SetActive(true);
        else
        {
            health -= 15;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[0].GetComponent<Text>().text;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[0];
        }
    }

    public void Resurrect2()
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        if (health <= 15)
            error.SetActive(true);
        else
        {
            health -= 15;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[1].GetComponent<Text>().text;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[1];
        }
    }

    public void Resurrect3()
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        if (health <= 15)
            error.SetActive(true);
        else
        {
            health -= 15;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[2].GetComponent<Text>().text;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[2];
        }
    }

    public void Resurrect4()
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        if (health <= 15)
            error.SetActive(true);
        else
        {
            health -= 15;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[3].GetComponent<Text>().text;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[3];
        }
    }

    public void Resurrect5()
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        if (health <= 15)
            error.SetActive(true);
        else
        {
            health -= 15;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[4].GetComponent<Text>().text;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[4];
        }
    }
}
