using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class reinforcingCharge : MonoBehaviour
{
    public GameObject[] playerDisplays;
    public GameObject[] names;
    public GameObject[] playerPieces;

    private GameObject player;
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private bool isCombatant;
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
        for (int i = 1; i < players.Length; i++)
        {
            isCombatant = false;
            for (int k = 0; k < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; k++)
            {
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[k] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                    isCombatant = true;
            }
            if (isCombatant)
            {
                playerDisplays[i - 1].SetActive(true);
                playerDisplays[i - 1].GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                names[i - 1].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
            else
            {
                playerDisplays[i - 1].SetActive(false);
                playerDisplays[i - 1].GetComponent<Image>().sprite = null;
                names[i - 1].GetComponent<Text>().text = "";
            }
        }
        for (int i = players.Length - 1; i < 5; i++)
        {
            playerDisplays[i].SetActive(false);
            playerDisplays[i].GetComponent<Image>().sprite = null;
            names[i].GetComponent<Text>().text = "";
        }
    }

    public void Teleport1()
    {
        player.transform.position = playerPieces[0].transform.position;
        player.GetComponent<Text>().text = players[1].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = players[1].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        gameObject.SetActive(false);
    }

    public void Teleport2()
    {
        player.transform.position = playerPieces[1].transform.position;
        player.GetComponent<Text>().text = players[2].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = players[2].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        gameObject.SetActive(false);
    }

    public void Teleport3()
    {
        player.transform.position = playerPieces[2].transform.position;
        player.GetComponent<Text>().text = players[3].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = players[3].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        gameObject.SetActive(false);
    }

    public void Teleport4()
    {
        player.transform.position = playerPieces[3].transform.position;
        player.GetComponent<Text>().text = players[4].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = players[4].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        gameObject.SetActive(false);
    }

    public void Teleport5()
    {
        player.transform.position = playerPieces[4].transform.position;
        player.GetComponent<Text>().text = players[5].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = players[5].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
        gameObject.SetActive(false);
    }
}
