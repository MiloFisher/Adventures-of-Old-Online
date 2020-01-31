using UnityEngine;
using System.Collections;
using System;
using UdpKit;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class joinServer : Bolt.GlobalEventListener
{
    public GameObject username;
    public GameObject error;
    public GameObject message;
    public GameObject serverType;

    public void bootServer()
    {
        if (!error.activeInHierarchy)
        {
            string fileName;
            if (serverType.GetComponent<Text>().text == "Join Game")
            {
                fileName = "playerInfo";
                var sr = File.CreateText(fileName);
                sr.WriteLine(username.GetComponent<Text>().text);
                sr.Close();
            }

			fileName = "playersInGame";
		    var sr1 = File.CreateText(fileName);
			sr1.WriteLine(0 + "");
			sr1.Close();

            message.SetActive(true);
            BoltLauncher.StartClient();
        }
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            string matchName = Guid.NewGuid().ToString();

            BoltNetwork.SetServerInfo(matchName, null);
            if (serverType.GetComponent<Text>().text == "Join Game")
                BoltNetwork.LoadScene("Lobby");
            else if (serverType.GetComponent<Text>().text == "Load Game")
                BoltNetwork.LoadScene("Load Lobby");
        }
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);

        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                BoltNetwork.Connect(photonSession);
            }
        }

        StartCoroutine(Timeout());
    }

    IEnumerator Timeout()
    {
        yield return new WaitForSeconds(30);
        message.SetActive(false);
        Shutdown();
        error.SetActive(true);
    }

    public void Shutdown()
    {
        BoltLauncher.Shutdown();
    }
}