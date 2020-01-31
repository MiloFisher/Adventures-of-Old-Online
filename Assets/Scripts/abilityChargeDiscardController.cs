using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class abilityChargeDiscardController : MonoBehaviour
{
    public GameObject generalScripts;
    public GameObject rightArrow;
    public GameObject leftArrow;
    public GameObject numberDisplay;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private int ac;
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
        ac = generalScripts.GetComponent<turnManager>().abilityChargesToDiscard;
        if (ac == 0)
            leftArrow.SetActive(false);
        else
            leftArrow.SetActive(true);
        if (ac == 7 || ac == int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges))
            rightArrow.SetActive(false);
        else
            rightArrow.SetActive(true);

        numberDisplay.GetComponent<Text>().text = ac + "";
    }
}
