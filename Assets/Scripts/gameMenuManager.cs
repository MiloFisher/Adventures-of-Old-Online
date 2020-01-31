using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject fullMap;
    public GameObject tradeWindow;
    public GameObject questWindow;
    public GameObject abilitiesWindow;
    public GameObject storeWindow;
    public GameObject confirmation;
    public GameObject confirmation2;
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private bool inCombat;
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

        menu.SetActive(false);
        tradeWindow.SetActive(false);
        questWindow.SetActive(false);
        abilitiesWindow.SetActive(false);
        storeWindow.SetActive(false);
        confirmation.SetActive(false);
        confirmation2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        inCombat = false;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == characterName )
                inCombat = true;
        }

        if (!inCombat)
        {
            for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators.Length; i++)
            {
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == characterName)
                    inCombat = true;
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (tradeWindow.activeInHierarchy && (gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive))
                    quitTrade();
                else
                {
                    if (fullMap.transform.position.x < 140 && (gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive))
                        exit();
                    else
                    {
                        if (questWindow.activeInHierarchy)
                            exit();
                        else
                        {
                            if (abilitiesWindow.activeInHierarchy)
                                exit();
                            else
                            {
                                if (storeWindow.activeInHierarchy)
                                    exit();
                                else
                                {
                                    if (menu.activeInHierarchy)
                                        menu.SetActive(false);
                                    else
                                        menu.SetActive(true);
                                }
                            }
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (questWindow.activeInHierarchy)
                    exit();
                else
                    quest();
            }

            if ((gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive) && !inCombat)
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    if (fullMap.transform.position.x < 140)
                        exit();
                    else
                        map();
                }
            }
            if (gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    if (tradeWindow.activeInHierarchy)
                        quitTrade();
                    else
                        trade();
                }
            }

            

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (storeWindow.activeInHierarchy)
                    exit();
                else
                    store();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (abilitiesWindow.activeInHierarchy)
                exit();
            else
                abilities();
        }
    }

    public void exit()
    {
        confirmation.SetActive(false);
        confirmation2.SetActive(false);
        inCombat = false;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i] == characterName)
                inCombat = true;
        }
        if (!inCombat && (gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive) && !questWindow.activeInHierarchy && !abilitiesWindow.activeInHierarchy && !storeWindow.activeInHierarchy && !tradeWindow.activeInHierarchy)
            fullMap.transform.localPosition = new Vector2(140, 0);
        questWindow.SetActive(false);
        abilitiesWindow.SetActive(false);
        storeWindow.SetActive(false);
        menu.SetActive(false);
        if (tradeWindow.activeInHierarchy)
        {
            tradeWindow.GetComponent<tradeController>().revertPlayer();
            tradeWindow.GetComponent<tradeController>().Cleanse();
            tradeWindow.GetComponent<tradeController>().waitingMessage.SetActive(false);
            tradeWindow.SetActive(false);
        }
    }

    public void quit()
    {
        confirmation.SetActive(true);
    }

    public void map()
    {
        if (tradeWindow.activeInHierarchy == false && abilitiesWindow.activeInHierarchy == false && questWindow.activeInHierarchy == false && fullMap.transform.position.x > 0 && storeWindow.activeInHierarchy == false && (gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive))
        {
            fullMap.transform.localPosition = new Vector2(0, 0);
            menu.SetActive(false);
        }
    }

    public void trade()
    {
        if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead && tradeWindow.activeInHierarchy == false && abilitiesWindow.activeInHierarchy == false && questWindow.activeInHierarchy == false && storeWindow.activeInHierarchy == false && (gameObject.GetComponent<turnManager>().phase < 4 || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive))
        {
            tradeWindow.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void quest()
    {
        if (tradeWindow.activeInHierarchy == false && abilitiesWindow.activeInHierarchy == false && questWindow.activeInHierarchy == false && storeWindow.activeInHierarchy == false)
        {
            questWindow.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void abilities()
    {
        if (tradeWindow.activeInHierarchy == false && abilitiesWindow.activeInHierarchy == false && questWindow.activeInHierarchy == false && storeWindow.activeInHierarchy == false)
        {
            abilitiesWindow.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void store()
    {
        if (tradeWindow.activeInHierarchy == false && abilitiesWindow.activeInHierarchy == false && questWindow.activeInHierarchy == false && storeWindow.activeInHierarchy == false)
        {
            storeWindow.SetActive(true);
            menu.SetActive(false);
        }
    }

    public void quitTrade()
    {
        confirmation2.SetActive(true);
    }
}
