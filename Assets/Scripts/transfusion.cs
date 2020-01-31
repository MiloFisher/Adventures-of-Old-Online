using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class transfusion : MonoBehaviour
{
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private int items;
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
        items = 0;
        for (int i = 0; i < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] != "")
                items++;
        }
        if (items == 0)
            gameObject.SetActive(false);
    }

    public void Transfuse1()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[0] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Transfuse2()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[1] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Transfuse3()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[2] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Transfuse4()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[3] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Transfuse5()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Transfuse6()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[5] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Transfuse7()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = null;
        GainAbilityCharge();
        gameObject.SetActive(false);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    void GainAbilityCharge()
    {
        int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
        ac++;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
    }
}
