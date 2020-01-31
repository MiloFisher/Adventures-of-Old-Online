using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class receiveHealingController : MonoBehaviour
{
    public GameObject[] playerPieces;

    private GameObject player;
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private bool healNullified;
    private string lastName8;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        //receive healing
        //healNullified = true;
        //for (int i = 0; i < players.Length; i++)
        //{
        //    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount == 0)
        //        healNullified = false;
        //}

        //if (healNullified)
        //    lastName8 = null;

        //for (int i = 0; i < players.Length; i++)
        //{
        //    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount > 0 && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName8)
        //    {
        //        if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
        //        {
        //            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead && playerID != i)
        //            {
        //                int level = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level);
        //                int friendLevel = int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level);
        //                if (friendLevel > level)
        //                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level;

        //                player.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        //                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;

        //                player.transform.position = playerPieces[i-1].transform.position;
        //            }
        //            int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        //            int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
        //            health += players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount;
        //            if (health > maxHealth)
        //                health = maxHealth;
        //            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
        //            lastName8 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;

        //            //Spider Queen Aranne Ability - Venomous Bite
        //            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
        //                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;
        //        }
        //    }
        //}
    }
}
