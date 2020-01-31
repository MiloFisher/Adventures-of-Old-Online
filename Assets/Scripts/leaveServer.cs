using UnityEngine;
using System.Collections;
using System;
using UdpKit;
using UnityEngine.SceneManagement;

public class leaveServer : Bolt.GlobalEventListener
{
    public void disconnectServer()
    {
        BoltLauncher.Shutdown();
        SceneManager.LoadScene("Main Menu");
    }
}