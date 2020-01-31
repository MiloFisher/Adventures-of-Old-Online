using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class bossRevealController : MonoBehaviour
{
    public Sprite cardBack;
    public GameObject[] bosses;
    public GameObject[] revealButtons;
    public GameObject generalScripts;

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
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss1)
        {
            bosses[0].GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[0], typeof(Sprite));
            revealButtons[0].SetActive(false);
        }
        else
        {
            bosses[0].GetComponent<Image>().sprite = cardBack;
            revealButtons[0].SetActive(true);
        }
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss2)
        {
            bosses[1].GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[1], typeof(Sprite));
            revealButtons[1].SetActive(false);
        }
        else
        {
            bosses[1].GetComponent<Image>().sprite = cardBack;
            revealButtons[1].SetActive(true);
        }
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss3)
        {
            bosses[2].GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[2], typeof(Sprite));
            revealButtons[2].SetActive(false);
        }
        else
        {
            bosses[2].GetComponent<Image>().sprite = cardBack;
            revealButtons[2].SetActive(true);
        }

        if (!revealButtons[0].activeInHierarchy && !revealButtons[1].activeInHierarchy && !revealButtons[2].activeInHierarchy)
            AutoEnd();
    }

    public void Reveal1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss1 = true;
        generalScripts.GetComponent<turnManager>().endTurn();
        gameObject.SetActive(false);
    }

    public void Reveal2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss2 = true;
        generalScripts.GetComponent<turnManager>().endTurn();
        gameObject.SetActive(false);
    }

    public void Reveal3()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss3 = true;
        generalScripts.GetComponent<turnManager>().endTurn();
        gameObject.SetActive(false);
    }

    public void AutoEnd()
    {
        generalScripts.GetComponent<turnManager>().endTurn();
        gameObject.SetActive(false);
    }
}
