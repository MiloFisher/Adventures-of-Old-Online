using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class skeletalCompanionController : MonoBehaviour
{
    public int health = 1;
    public int maxHealth = 1;
    public GameObject healthDisplay;
    public GameObject maxHealthDisplay;

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

    void OnEnable()
    {
        maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower)*4;
        if (maxHealth == 0)
            maxHealth = 1;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthDisplay.GetComponent<Text>().text = health + "";
        maxHealthDisplay.GetComponent<Text>().text = maxHealth + "";

        if (health <= 0)
        {
            gameObject.SetActive(false);
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion = false;
        }

        if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion)
            gameObject.SetActive(false);
    }
}
