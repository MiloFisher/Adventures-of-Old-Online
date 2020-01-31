using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class abilityVisuals : MonoBehaviour
{
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private string image;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        string Race = sr.ReadLine();
        string Class = sr.ReadLine();
        string Strength = sr.ReadLine();
        string Dexterity = sr.ReadLine();
        string Intellect = sr.ReadLine();
        string Health = sr.ReadLine();
        string MaxHealth = sr.ReadLine();
        string Level = sr.ReadLine();
        string Damage = sr.ReadLine();
        string PhysicalPower = sr.ReadLine();
        string SpellPower = sr.ReadLine();
        string DamageReduction = sr.ReadLine();
        string Gold = sr.ReadLine();
        image = sr.ReadLine();
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
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image = "worgForm";
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image = "bearForm";
        else
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image = image;
    }
}
