using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class arrowVolley : MonoBehaviour
{
    public GameObject rollNumber;
    public GameObject rollButton;

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
    private void OnEnable()
    {
        rollButton.SetActive(true);
    }

    public void Roll()
    {
        int rando = Random.Range(1, 7);
        rollButton.SetActive(false);
        rollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(Calculate(rando));
    }

    IEnumerator Calculate(int roll)
    {
        yield return new WaitForSeconds(.2f);
        if (roll % 2 == 0)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);
        }
        yield return new WaitForSeconds(.8f);
        rollNumber.GetComponent<Text>().text = "";
        if (roll % 2 == 0)
            rollButton.SetActive(true);
        else
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArrowVolley = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = 0;
    }
}
