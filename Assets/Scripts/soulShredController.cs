using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class soulShredController : MonoBehaviour
{
    public GameObject inputfield;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private int playerHealth;
    private int amount;
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

    public void Confirm()
    {
        playerHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        amount = int.Parse(inputfield.GetComponent<InputField>().text);
        if (amount < playerHealth && amount > 0)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShredAmount = amount;
            gameObject.GetComponent<InputField>().text = "";
            gameObject.SetActive(false);
        }
    }
}
