using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UdpKit;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

public class startServer : Bolt.GlobalEventListener
{
    public GameObject username;
    public GameObject players;
	public GameObject message;
    public GameObject serverType;

    public void bootServer()
	{
        string fileName;
        if (serverType.GetComponent<Text>().text == "Create New Game")
        {
            fileName = "playerInfo";
            var sr = File.CreateText(fileName);
            sr.WriteLine(username.GetComponent<Text>().text);
            sr.Close();
        }

        fileName = "playersInGame";
		var sr1 = File.CreateText(fileName);
		sr1.WriteLine(players.GetComponent<Text>().text);
		sr1.Close();

		message.SetActive(true);
		BoltLauncher.StartServer();
    }

	public override void BoltStartDone()
	{
		if (BoltNetwork.IsServer)
		{
			string matchName = Guid.NewGuid().ToString();
            
			BoltNetwork.SetServerInfo(matchName, null);
            if(serverType.GetComponent<Text>().text == "Create New Game")
                BoltNetwork.LoadScene("Lobby");
            else if(serverType.GetComponent<Text>().text == "Load Game")
                BoltNetwork.LoadScene("Load Lobby");
        }
	}
}