using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class wildwalkers : MonoBehaviour
{
    public GameObject rollNumber;
    public GameObject rollButton;
    public GameObject masterSpellcaster;

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
        if (roll%2 == 0)
        {
            int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
            ac++;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
            masterSpellcaster.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
