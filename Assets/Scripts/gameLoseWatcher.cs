using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameLoseWatcher : MonoBehaviour
{
    public GameObject loseMessage;
    public GameObject description;
    public GameObject winMessage;

    private bool gameOver = false;
    private GameObject serverInfo;
    private GameObject[] players;
    private bool someoneIsAlive;
    private bool startChecking = false;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        StartCoroutine(StartChecking());
    }

    // Update is called once per frame
    void Update()
    {
        if (startChecking)
        {
            someoneIsAlive = false;
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                    someoneIsAlive = true;
            }

            if (!someoneIsAlive && !gameOver)
            {
                description.GetComponent<Text>().text = "All players are dead.\nYou killed " + (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area - 1) + " / 3 Bosses";
                StartCoroutine(Defeat());
            }

            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterDeck[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().EncounterIterator] == null && !gameOver)
            {
                description.GetComponent<Text>().text = "All encounters are gone.\nYou killed " + (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area - 1) + " / 3 Bosses";
                StartCoroutine(Defeat());
            }

            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area == 4)
            {
                Debug.Log("Area: " + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area);
                for (int i = 0; i < players.Length; i++)
                {
                    if(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().KilledBoss)
                        Debug.Log("Killed Boss: " + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name);
                }
                StartCoroutine(Victory());
            }
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area >= 4)
            {
                Debug.Log("Area: " + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area);
            }
        }
    }

    IEnumerator StartChecking()
    {
        yield return new WaitForSeconds(10);
        startChecking = true;
    }

    IEnumerator Defeat()
    {
        gameOver = true;
        loseMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<leaveServer>().disconnectServer();
    }

    IEnumerator Victory()
    {
        gameOver = true;
        winMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<leaveServer>().disconnectServer();
    }
}
