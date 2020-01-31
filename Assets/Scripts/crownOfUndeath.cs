using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class crownOfUndeath : MonoBehaviour
{
    public GameObject rollNumber;
    public GameObject rollButton;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    void Start()
    {
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        sr.Close();
        players = GameObject.FindGameObjectsWithTag("Player Info");
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
        rollButton.SetActive(true);
        rollNumber.GetComponent<Text>().text = "";
    }

    public void Roll()
    {
        rollButton.SetActive(false);
        int rando = Random.Range(1, 7);
        rollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(ProcessLag(rando));
    }

    IEnumerator ProcessLag(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll >= 5)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth;
        }
        gameObject.SetActive(false);
    }
}
