using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class tradeController : MonoBehaviour
{
    public GameObject menu;
    public GameObject[] names;
    public GameObject[] greyedOut;
    public GameObject[] playerInventory;
    public GameObject[] playerTrades;
    public GameObject[] targetTrades;
    public GameObject tradeButton;
    public GameObject pendingButton;
    public GameObject waitingMessage;
    public GameObject successMessage;
    public GameObject failMessage;
    public GameObject selector;
    private GameObject[] players;
    private GameObject serverInfo;
    private int playerID;
    private int targetID;
    private bool connectFail;
    private int[] selectedList = new int[5];
    private string[] tradeList = new string[5];
    private bool shouldQuit;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        string characterName = sr.ReadLine();
        sr.Close();
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
        Cleanse();
    }

    void Update()
    {
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget == null)
        {
            selector.SetActive(false);
            Cleanse();
        }
        else
        {
            connectFail = false;
            for (int i = 0; i < greyedOut.Length; i++)
            {
                if (greyedOut[i].activeInHierarchy)
                {
                    if (names[i].GetComponent<Text>().text == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget)
                    {
                        connectFail = true;
                    }
                }
            }
            if (!connectFail)
            {
                AssignTrade(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget);
            }
            else
            {
                Cleanse();
            }

            //for (int i = 0; i < selectedList.Length; i++)
            //{
            //    if (selectedList[i] != -1)
            //    {
            //        if(!pendingButton.activeInHierarchy)
            //            tradeButton.SetActive(true);
            //    }
            //}

            //if (pendingButton.activeInHierarchy && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady)
            //{
            //    gameObject.SetActive(false);
            //}

            shouldQuit = false;
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] != "")
                {
                    shouldQuit = true;
                }
            }
            if (shouldQuit)
                gameObject.SetActive(false);
        }

        //trade completions
        //if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget == players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name && players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
        //{
        //    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady && players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady)
        //    {
        //        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady = false;
        //        players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady = false;
        //        exchangeItems(players[playerID], players[targetID]);
        //    }
        //}
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanResolveTrade)
        {
            bool recievedSomething = false;
            for (int i = 0; i < tradeList.Length; i++)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = tradeList[i];
                if (tradeList[i] != null && tradeList[i] != "")
                    recievedSomething = true;
            }
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] > -1)
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[selectedList[i]] = null;
                }
            }
            for (int i = 0; i < tradeList.Length; i++)
                tradeList[i] = null;
            for (int i = 0; i < selectedList.Length; i++)
                selectedList[i] = -1;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanResolveTrade = false;
            //limbo.SetActive(true);
            if(!recievedSomething)
                gameObject.SetActive(false);
        }

        //shouldQuit = false;
        //if (limbo.activeInHierarchy)
        //    shouldQuit = true;
        //if (shouldQuit)
        //    gameObject.SetActive(false);
    }

    public void player1()
    {
        if (names[0].GetComponent<Text>().text != null)
        {
            selector.SetActive(true);
            selector.transform.localPosition = new Vector2(-164,7.5f);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget = names[0].GetComponent<Text>().text;

            //notification
            GameObject nc = GameObject.Find("Notification Controller");
            nc.GetComponent<NotificationController>().SendNotification(names[0].GetComponent<Text>().text, players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " want to trade!");
        }
    }
    public void player2()
    {
        if (names[1].GetComponent<Text>().text != null)
        {
            selector.SetActive(true);
            selector.transform.localPosition = new Vector2(-82, 7.5f);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget = names[1].GetComponent<Text>().text;

            //notification
            GameObject nc = GameObject.Find("Notification Controller");
            nc.GetComponent<NotificationController>().SendNotification(names[1].GetComponent<Text>().text, players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " want to trade!");
        }
    }
    public void player3()
    {
        if (names[2].GetComponent<Text>().text != null)
        {
            selector.SetActive(true);
            selector.transform.localPosition = new Vector2(0, 7.5f);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget = names[2].GetComponent<Text>().text;

            //notification
            GameObject nc = GameObject.Find("Notification Controller");
            nc.GetComponent<NotificationController>().SendNotification(names[2].GetComponent<Text>().text, players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " want to trade!");
        }
    }
    public void player4()
    {
        if (names[3].GetComponent<Text>().text != null)
        {
            selector.SetActive(true);
            selector.transform.localPosition = new Vector2(82, 7.5f);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget = names[3].GetComponent<Text>().text;

            //notification
            GameObject nc = GameObject.Find("Notification Controller");
            nc.GetComponent<NotificationController>().SendNotification(names[3].GetComponent<Text>().text, players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " want to trade!");
        }
    }
    public void player5()
    {
        if (names[4].GetComponent<Text>().text != null)
        {
            selector.SetActive(true);
            selector.transform.localPosition = new Vector2(164, 7.5f);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget = names[4].GetComponent<Text>().text;

            //notification
            GameObject nc = GameObject.Find("Notification Controller");
            nc.GetComponent<NotificationController>().SendNotification(names[4].GetComponent<Text>().text, players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name + " want to trade!");
        }
    }
    public void revertPlayer()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget = null;
    }

    public void Trade()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady = true;
        tradeButton.SetActive(false);
        pendingButton.SetActive(true);
    }

    public void CancelTrade()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady = false;
        pendingButton.SetActive(false);
        tradeButton.SetActive(true);
    }

    public void Cleanse()
    {
        for (int i = 0; i < playerInventory.Length; i++)
            playerInventory[i].SetActive(false);
        for (int i = 0; i < playerTrades.Length; i++)
            playerTrades[i].SetActive(false);
        for (int i = 0; i < targetTrades.Length; i++)
            targetTrades[i].SetActive(false);
        tradeButton.SetActive(false);
        pendingButton.SetActive(false);
        successMessage.SetActive(false);
        failMessage.SetActive(false);
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = null;
        for (int i = 0; i < tradeList.Length; i++)
            tradeList[i] = null;
        for (int i = 0; i < selectedList.Length; i++)
            selectedList[i] = -1;
    }

    public void AssignTrade(string target)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == target)
            {
                targetID = i;
            }
        }
        if (players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
        {
            bool canCopy = false;
            for (int i = 0; i < players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
            {
                if (players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] != null && players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] != "")
                    canCopy = true;
            }
            if (canCopy)
            {
                for (int i = 0; i < players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    tradeList[i] = players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i];
                }
            }

            waitingMessage.SetActive(false);
            if (!pendingButton.activeInHierarchy)
                tradeButton.SetActive(true);

            for (int i = 0; i < targetTrades.Length; i++)
            {
                if (players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null || players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == "")
                {
                    targetTrades[i].SetActive(false);
                }
                else
                {
                    targetTrades[i].SetActive(true);
                    Sprite image = (Sprite)Resources.Load("Treasure Cards/" + players[targetID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i], typeof(Sprite));
                    targetTrades[i].GetComponent<Image>().sprite = image;
                }
            }
            for (int i = 0; i < playerTrades.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                {
                    playerTrades[i].SetActive(false);
                }
                else
                {
                    playerTrades[i].SetActive(true);
                    Sprite image = (Sprite)Resources.Load("Treasure Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i], typeof(Sprite));
                    playerTrades[i].GetComponent<Image>().sprite = image;
                }
            }
            bool fail;
            for (int i = 0; i < playerInventory.Length; i++)
            {
                fail = false;
                for (int j = 0; j < selectedList.Length; j++)
                {
                    if (selectedList[j] == i)
                        fail = true;
                }
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                    fail = true;
                if (!fail)
                {
                    playerInventory[i].SetActive(true);
                    Sprite image = (Sprite)Resources.Load("Treasure Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i], typeof(Sprite));
                    playerInventory[i].GetComponent<Image>().sprite = image;
                }
                else
                    playerInventory[i].SetActive(false);
            }
        }
        else
        {
            waitingMessage.SetActive(true);
            Cleanse();
        }
    }

    public void AddTrade1()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 0;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }
    public void AddTrade2()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 1;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }
    public void AddTrade3()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 2;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }
    public void AddTrade4()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 3;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }
    public void AddTrade5()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 4;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }
    public void AddTrade6()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 5;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }
    public void AddTrade7()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 6;
            bool fail = false;
            for (int i = 0; i < selectedList.Length; i++)
            {
                if (selectedList[i] == identification)
                    fail = true;
            }
            if (!fail)
            {
                for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems.Length; i++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] == null)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[identification];
                        selectedList[i] = identification;
                        i = 100;
                    }
                }
            }
        }
    }

    public void RemoveTrade1()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 0;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[identification] = null;
            selectedList[identification] = -1;
        }
    }
    public void RemoveTrade2()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 1;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[identification] = null;
            selectedList[identification] = -1;
        }
    }
    public void RemoveTrade3()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 2;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[identification] = null;
            selectedList[identification] = -1;
        }
    }
    public void RemoveTrade4()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 3;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[identification] = null;
            selectedList[identification] = -1;
        }
    }
    public void RemoveTrade5()
    {
        if (!pendingButton.activeInHierarchy)
        {
            int identification = 4;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeItems[identification] = null;
            selectedList[identification] = -1;
        }
    }
}