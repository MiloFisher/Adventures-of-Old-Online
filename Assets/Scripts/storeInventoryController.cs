using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class storeInventoryController : MonoBehaviour
{
    public string characterName;
    public GameObject[] inventory;
    private GameObject[] players;
    private int activeTotal;
    private int playerID;
    // Start is called before the first frame update
    void Start()
    {
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
        for (int j = 0; j < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; j++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j] != "")
            {
                inventory[j].SetActive(true);
                inventory[j].GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j], typeof(Sprite));
            }
            else
            {
                inventory[j].SetActive(false);
            }
        }

        //display
        activeTotal = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].activeInHierarchy)
            {
                activeTotal++;
            }
        }
        if (activeTotal == 1)
        {
            inventory[0].transform.localPosition = new Vector2(0, 0);
        }
        else if (activeTotal == 2)
        {
            inventory[0].transform.localPosition = new Vector2(-55, 0);
            inventory[1].transform.localPosition = new Vector2(55, 0);
        }
        else if (activeTotal == 3)
        {
            inventory[0].transform.localPosition = new Vector2(-110, 0);
            inventory[1].transform.localPosition = new Vector2(0, 0);
            inventory[2].transform.localPosition = new Vector2(110, 0);
        }
        else if (activeTotal == 4)
        {
            inventory[0].transform.localPosition = new Vector2(-165, 0);
            inventory[1].transform.localPosition = new Vector2(-55, 0);
            inventory[2].transform.localPosition = new Vector2(55, 0);
            inventory[3].transform.localPosition = new Vector2(165, 0);
        }
        else if (activeTotal == 5)
        {
            inventory[0].transform.localPosition = new Vector2(-220, 0);
            inventory[1].transform.localPosition = new Vector2(-110, 0);
            inventory[2].transform.localPosition = new Vector2(0, 0);
            inventory[3].transform.localPosition = new Vector2(110, 0);
            inventory[4].transform.localPosition = new Vector2(220, 0);
        }
        else if (activeTotal == 6)
        {
            inventory[0].transform.localPosition = new Vector2(-275, 0);
            inventory[1].transform.localPosition = new Vector2(-165, 0);
            inventory[2].transform.localPosition = new Vector2(-55, 0);
            inventory[3].transform.localPosition = new Vector2(55, 0);
            inventory[4].transform.localPosition = new Vector2(165, 0);
            inventory[5].transform.localPosition = new Vector2(275, 0);
        }
        else if (activeTotal == 7)
        {
            inventory[0].transform.localPosition = new Vector2(-324, 0);
            inventory[1].transform.localPosition = new Vector2(-216, 0);
            inventory[2].transform.localPosition = new Vector2(-108, 0);
            inventory[3].transform.localPosition = new Vector2(0, 0);
            inventory[4].transform.localPosition = new Vector2(108, 0);
            inventory[5].transform.localPosition = new Vector2(216, 0);
            inventory[6].transform.localPosition = new Vector2(324, 0);
        }

        //content
        //players = GameObject.FindGameObjectsWithTag("Player Info");
        //for (int i = 0; i < players.Length; i++)
        //{
        //    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
        //    {
        //        for (int j = 0; j < inventory.Length; j++)
        //        {
        //            //inventory[j].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges;
        //        }
        //    }
        //}
    }
}
