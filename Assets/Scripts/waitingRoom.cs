using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class waitingRoom : MonoBehaviour
{
    private GameObject[] players;
    private string username;
    private int unfinshed = 0;
    private string usernames = "";
	private int ready = 0;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            usernames += players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Username + "\n";
        }
        
        //get this player info
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        username = sr.ReadLine();
        sr.Close();

        gameObject.GetComponent<Text>().text = "Waiting on " + players.Length + " players...\n" + usernames;
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Username == username && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == null)
            {
                players[i].GetComponent<playerInfoBehaviour>().updateInfo();
            }
        }

        usernames = "";
        unfinshed = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == null)
            {
                usernames += players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Username + "\n";
                unfinshed++;
            }
        }
        gameObject.GetComponent<Text>().text = "Waiting on " + unfinshed + " players...\n" + usernames;

		//check to start game
		ready = 0;
		for (int i = 0; i < players.Length; i++)
		{
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges != null)
                ready++;
		}
        if (ready == players.Length)
            StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game Running");
    }
}
