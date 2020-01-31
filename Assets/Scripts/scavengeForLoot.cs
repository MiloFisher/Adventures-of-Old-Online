using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class scavengeForLoot : MonoBehaviour
{
    public GameObject rollNumber;
    public GameObject rollButton;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    void Start()
    {
        string fileName = "playerInfo";
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        sr.Close();
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
    }

    void OnEnable()
    {
        rollButton.SetActive(true);
        rollNumber.GetComponent<Text>().text = "";
    }

    public void Roll()
    {
        rollButton.SetActive(false);
        int rando = Random.Range(1, 7);
        rollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(ProcessLag(rando));
    }

    IEnumerator ProcessLag(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll >= 4)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CardsDrawing = 1;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[6] = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TreasureIterator + 1];
        }
        gameObject.SetActive(false);
    }
}
