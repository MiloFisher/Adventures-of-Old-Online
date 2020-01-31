using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerNamesList : Bolt.GlobalEventListener
{
    public GameObject serverType;
    private GameObject[] players;
    private GameObject amount;
    private string usernames;
    private int total = 0;
    // Start is called before the first frame update
    void Start()
    {
        amount = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
    }

    // Update is called once per frame
    void Update()
    {
        amount = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        usernames = "";
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == null)
            {
                players[i].GetComponent<playerInfoBehaviour>().updateInfo();
            }
            usernames += players[i].GetComponent<BoltEntity>().GetState <IPlayerInfo>().Username + "\n";
            //if (players[i].transform.position.x != 100)
            //{
            //    players[i].transform.position = new Vector2(100, 100);
            //}
        }
        if(amount != null && amount.GetComponent<BoltEntity>().IsAttached)
        {
            //Debug.Log(amount[i].GetComponent<BoltEntity>().GetState<IPlayersInGame>().Amount);
            if (amount.GetComponent<BoltEntity>().GetState <IServerInfo>().Players == "1" || amount.GetComponent<BoltEntity>().GetState<IServerInfo>().Players == "2" || amount.GetComponent<BoltEntity>().GetState<IServerInfo>().Players == "3" || amount.GetComponent<BoltEntity>().GetState<IServerInfo>().Players == "4" || amount.GetComponent<BoltEntity>().GetState<IServerInfo>().Players == "5" || amount.GetComponent<BoltEntity>().GetState<IServerInfo>().Players == "6")
            {
                //if (amount[i].GetComponent<Text>().text == "3")
                //    total = 3;
                //else if (amount[i].GetComponent<Text>().text == "4")
                //    total = 4;
                //else if (amount[i].GetComponent<Text>().text == "5")
                //    total = 5;
                //else if (amount[i].GetComponent<Text>().text == "6")
                //    total = 6;
                total = int.Parse(amount.GetComponent<BoltEntity>().GetState <IServerInfo>().Players);
            }
            //if (amount[i].transform.position.x != 100)
            //{
            //    amount[i].transform.position = new Vector2(100, 100);
            //}
        }
        gameObject.GetComponent<Text>().text = "Waiting on " + (total - players.Length) + " players...\n" + usernames;

        if (total - players.Length == 0 && players.Length > 0)
        {
            if (serverType.GetComponent<Text>().text == "Game Lobby")
                SceneManager.LoadScene("Character Creation");
            else if (serverType.GetComponent<Text>().text == "Load Game Lobby")
                SceneManager.LoadScene("Game Running");
        }
    }
}

