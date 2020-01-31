using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class limboTreasureCardsController : MonoBehaviour
{
    public GameObject limbo;
    public GameObject confirmation;
    public GameObject[] cards;
    public GameObject errorMessage;
    private GameObject[] players;
    private int activeTotal;
    private int playerID;
    private string temp;
    // Start is called before the first frame update
    void Start()
    {
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
        errorMessage.SetActive(false);
        confirmation.SetActive(false);
        limbo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] != "")
            {
                for (int j = 0; j < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; j++)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[j] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[j] == "")
                    {
                        if (j < i)
                        {
                            temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[j];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[j] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = temp;
                            j = 100;
                        }
                    }
                }
            }
        }

        activeTotal = 0;
        //for (int i = 0; i < cards.Length; i++)
        //{
        //    if (cards[i].activeInHierarchy)
        //    {
        //        activeTotal++;
        //    }
        //}

        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] != "")
            {
                cards[i].SetActive(true);
                Sprite image = (Sprite)Resources.Load("Treasure Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i], typeof(Sprite));
                cards[i].GetComponent<Image>().sprite = image;
                activeTotal++;
            }
            else
            {
                cards[i].SetActive(false);
            }
        }

        if (activeTotal == 0)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure = false;
            limbo.SetActive(false);
        }
        else
        {
            limbo.SetActive(true);
            if (activeTotal == 1)
            {
                cards[0].transform.localPosition = new Vector2(0, 0);
            }
            else if (activeTotal == 2)
            {
                cards[0].transform.localPosition = new Vector2(-41.25f, 0);
                cards[1].transform.localPosition = new Vector2(41.25f, 0);
            }
            else if (activeTotal == 3)
            {
                cards[0].transform.localPosition = new Vector2(-82.5f, 0);
                cards[1].transform.localPosition = new Vector2(0, 0);
                cards[2].transform.localPosition = new Vector2(82.5f, 0);
            }
            else if (activeTotal == 4)
            {
                cards[0].transform.localPosition = new Vector2(-123.75f, 0);
                cards[1].transform.localPosition = new Vector2(-41.25f, 0);
                cards[2].transform.localPosition = new Vector2(41.25f, 0);
                cards[3].transform.localPosition = new Vector2(123.75f, 0);
            }
            else if (activeTotal == 5)
            {
                cards[0].transform.localPosition = new Vector2(-165, 0);
                cards[1].transform.localPosition = new Vector2(-82.5f, 0);
                cards[2].transform.localPosition = new Vector2(0, 0);
                cards[3].transform.localPosition = new Vector2(82.5f, 0);
                cards[4].transform.localPosition = new Vector2(165, 0);
            }
            else if (activeTotal == 6)
            {
                cards[0].transform.localPosition = new Vector2(-165, 0);
                cards[1].transform.localPosition = new Vector2(-98.33f, 0);
                cards[2].transform.localPosition = new Vector2(-31.66f, 0);
                cards[3].transform.localPosition = new Vector2(31.66f, 0);
                cards[4].transform.localPosition = new Vector2(98.33f, 0);
                cards[5].transform.localPosition = new Vector2(165, 0);
            }
            else if (activeTotal == 7)
            {
                cards[0].transform.localPosition = new Vector2(-165, 0);
                cards[1].transform.localPosition = new Vector2(-110, 0);
                cards[2].transform.localPosition = new Vector2(-55, 0);
                cards[3].transform.localPosition = new Vector2(0, 0);
                cards[4].transform.localPosition = new Vector2(55, 0);
                cards[5].transform.localPosition = new Vector2(110, 0);
                cards[6].transform.localPosition = new Vector2(165, 0);
            }
        }
    }

    public void confirmationApprove()
    {
        confirmation.SetActive(true);
    }

    public void confirmationCancel()
    {
        confirmation.SetActive(false);
    }

    public void DiscardAll()
    {
        confirmation.SetActive(false);
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards.Length; i++)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[i] = null;
        }
    }

    public void AddCard1()
    {
        int identification = 0;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    public void AddCard2()
    {
        int identification = 1;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    public void AddCard3()
    {
        int identification = 2;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    public void AddCard4()
    {
        int identification = 3;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    public void AddCard5()
    {
        int identification = 4;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    public void AddCard6()
    {
        int identification = 5;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
    public void AddCard7()
    {
        int identification = 6;

        int inventoryLength = 0;
        int inventoryCap = 5;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
            {
                inventoryLength++;
            }
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
            inventoryCap = 7;

        if (inventoryLength < inventoryCap)
        {
            for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LimboTreasureCards[identification] = null;
                    i = 100;
                }
            }
        }
        else
        {
            errorMessage.SetActive(true);
        }
    }
}
