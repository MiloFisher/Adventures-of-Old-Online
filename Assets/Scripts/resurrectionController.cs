using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class resurrectionController : MonoBehaviour
{
    public GameObject[] playerDisplays;
    public GameObject[] names;

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
    }

    // Update is called once per frame
    void Update()
    {
        deadPeople = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            {
                playerDisplays[deadPeople].SetActive(true);
                playerDisplays[deadPeople].GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                names[deadPeople].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Life and Death")
                    amounts[deadPeople] = int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                else
                    amounts[deadPeople] = 1;
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

    public void Resurrect1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[0].GetComponent<Text>().text;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[0];
        gameObject.SetActive(false);
    }

    public void Resurrect2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[1].GetComponent<Text>().text;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[1];
        gameObject.SetActive(false);
    }

    public void Resurrect3()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[2].GetComponent<Text>().text;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[2];
        gameObject.SetActive(false);
    }

    public void Resurrect4()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[3].GetComponent<Text>().text;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[3];
        gameObject.SetActive(false);
    }

    public void Resurrect5()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = names[4].GetComponent<Text>().text;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = amounts[4];
        gameObject.SetActive(false);
    }
}
