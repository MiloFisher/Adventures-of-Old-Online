using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class bossEncounterDisplay : MonoBehaviour
{
    public Sprite cardBack;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
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
        switch (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area)
        {
            case 1:
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss1)
                    gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[0], typeof(Sprite));
                else
                    gameObject.GetComponent<Image>().sprite = cardBack;
                break;
            case 2:
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss2)
                    gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[1], typeof(Sprite));
                else
                    gameObject.GetComponent<Image>().sprite = cardBack;
                break;
            case 3:
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss3)
                    gameObject.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[2], typeof(Sprite));
                else
                    gameObject.GetComponent<Image>().sprite = cardBack;
                break;
        }
    }
}
