using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class rollForTurnOrder : MonoBehaviour
{
    public GameObject quest;
    public GameObject turnOrderObject;
    public GameObject turnOrderButton;
    public GameObject turnOrderDisplay;
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private int roll;
    // Start is called before the first frame update
    void Start()
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
        turnOrderObject.SetActive(true);
        StartCoroutine(flashQuest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rollTurnOrder()
    {
        roll = Random.Range(1, 7);
        turnOrderDisplay.GetComponent<Text>().text = roll + "";
        turnOrderButton.SetActive(false);
        StartCoroutine(decayTimer());
    }

    IEnumerator decayTimer()
    {
        yield return new WaitForSeconds(1);
        turnOrderObject.SetActive(false);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnRoll = roll;
    }

    IEnumerator flashQuest()
    {
        yield return new WaitForSeconds(.5f);
        quest.SetActive(true);
        yield return new WaitForSeconds(3);
        quest.SetActive(false);
    }
}
