using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class abilityWindowPilot : MonoBehaviour
{
    public GameObject abilities;
    public GameObject crownOfUndeath;

    private GameObject player;
    private GameObject[] players;
    private GameObject serverInfo;
    private int playerID;
    private string characterName;
    private bool clearedHuntersMark = true;
    private bool dead;
    private bool clearedBerserk = true;

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
        abilities.GetComponent<abilitiesManager>().Setup();
    }

    // Update is called once per frame
    void Update()
    {
        //get player stats
        abilities.GetComponent<abilitiesManager>().AssignStats();

        //set player abilities
        abilities.GetComponent<abilitiesManager>().SetAbilities();

        //format abilities in ability menu
        abilities.GetComponent<abilitiesManager>().FormatAbilities();

        //legendary abilities

        //Ranger's Guise
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Relentless Pursuit")
        {
            clearedHuntersMark = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark = true;
        }
        else if (!clearedHuntersMark)
        {
            clearedHuntersMark = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark = false;
        }

        //Crown of Undeath
        //King of the Undead Ability - Undead Wrath
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Life and Death" && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 21)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            {
                if (!dead)
                {
                    dead = true;
                    crownOfUndeath.SetActive(true);
                }
            }
            else
                dead = false;
        }

        //Warbringer's Visage
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "War Frenzy")
        {
            clearedBerserk = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk = true;
        }
        else if (!clearedBerserk)
        {
            clearedBerserk = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk = false;
        }

        //chi callers
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow >= 7)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow -= 7;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = 20;
        }

        //wavedancers
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "Crashing Waves")
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CrashingWaves = 0;
        }
    }
}
