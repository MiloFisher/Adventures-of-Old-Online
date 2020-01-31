using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class inventoryController : MonoBehaviour
{
    public string characterName;
    public GameObject[] inventory;
    private GameObject[] players;
    private int activeTotal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            inventory[0].transform.localPosition = new Vector2(0,0);
        }
        else if (activeTotal == 2)
        {
            inventory[0].transform.localPosition = new Vector2(-41.25f, 0);
            inventory[1].transform.localPosition = new Vector2(41.25f, 0);
        }
        else if (activeTotal == 3)
        {
            inventory[0].transform.localPosition = new Vector2(-82.5f, 0);
            inventory[1].transform.localPosition = new Vector2(0, 0);
            inventory[2].transform.localPosition = new Vector2(82.5f, 0);
        }
        else if (activeTotal == 4)
        {
            inventory[0].transform.localPosition = new Vector2(-123.75f, 0);
            inventory[1].transform.localPosition = new Vector2(-41.25f, 0);
            inventory[2].transform.localPosition = new Vector2(41.25f, 0);
            inventory[3].transform.localPosition = new Vector2(123.75f, 0);
        }
        else if (activeTotal == 5)
        {
            inventory[0].transform.localPosition = new Vector2(-165, 0);
            inventory[1].transform.localPosition = new Vector2(-82.5f, 0);
            inventory[2].transform.localPosition = new Vector2(0, 0);
            inventory[3].transform.localPosition = new Vector2(82.5f, 0);
            inventory[4].transform.localPosition = new Vector2(165, 0);
        }
        else if (activeTotal == 6)
        {
            inventory[0].transform.localPosition = new Vector2(-165, 0);
            inventory[1].transform.localPosition = new Vector2(-98.33f, 0);
            inventory[2].transform.localPosition = new Vector2(-31.66f, 0);
            inventory[3].transform.localPosition = new Vector2(31.66f, 0);
            inventory[4].transform.localPosition = new Vector2(98.33f, 0);
            inventory[5].transform.localPosition = new Vector2(165, 0);
        }
        else if (activeTotal == 7)
        {
            inventory[0].transform.localPosition = new Vector2(-165, 0);
            inventory[1].transform.localPosition = new Vector2(-110, 0);
            inventory[2].transform.localPosition = new Vector2(-55, 0);
            inventory[3].transform.localPosition = new Vector2(0, 0);
            inventory[4].transform.localPosition = new Vector2(55, 0);
            inventory[5].transform.localPosition = new Vector2(110, 0);
            inventory[6].transform.localPosition = new Vector2(165, 0);
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
