using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class pickLegendaryCardController : MonoBehaviour
{
    public GameObject card1;
    public GameObject card2;
    public GameObject error;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string legendary1;
    private string legendary2;

    void Update()
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

        switch (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class)
        {
            case "Warrior": legendary1 = "a30"; legendary2 = "w20"; break;
            case "Hunter": legendary1 = "a25"; legendary2 = "w15"; break;
            case "Sorcerer": legendary1 = "a24"; legendary2 = "w14"; break;
            case "Cleric": legendary1 = "a26"; legendary2 = "w16"; break;
            case "Paladin": legendary1 = "a28"; legendary2 = "w18"; break;
            case "Necromancer": legendary1 = "a29"; legendary2 = "w19"; break;
            case "Rogue": legendary1 = "a27"; legendary2 = "w17"; break;
            case "Druid": legendary1 = "a31"; legendary2 = "w21"; break;
            case "Monk": legendary1 = "a32"; legendary2 = "w33"; break;
        }

        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries.Length; i++)
        {
            if (legendary1 == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i])
                legendary1 = null;
            if (legendary2 == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i])
                legendary2 = null;
        }

        if (legendary1 == null && legendary2 == null)
        {
            card1.SetActive(false);
            card2.SetActive(false);
            error.SetActive(true);
        }
        else
        {
            if (legendary1 != null)
            {
                card1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + legendary1, typeof(Sprite));
                card1.SetActive(true);
            }
            else
            {
                card1.SetActive(false);
            }
            if (legendary2 != null)
            {
                card2.GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + legendary2, typeof(Sprite));
                card2.SetActive(true);
            }
            else
            {
                card2.SetActive(false);
            }
        }
    }

    public void exit()
    {
        error.SetActive(false);
        gameObject.SetActive(false);
    }

    public void pickCard1()
    {
        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " found a Legendary Item!");

        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] = legendary1;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] == null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] == "")
            {
                serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] = legendary1;
                i = 100;
            }
        }
        exit();
    }

    public void pickCard2()
    {
        //notification
        GameObject nc = GameObject.Find("Notification Controller");
        nc.GetComponent<NotificationController>().SendNotification("all", players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " found a Legendary Item!");

        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] = legendary2;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] == null || serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] == "")
            {
                serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TakenLegendaries[i] = legendary2;
                i = 100;
            }
        }
        exit();
    }
}
