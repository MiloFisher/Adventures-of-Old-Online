using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerDisplayController : MonoBehaviour
{
    public GameObject turnMarker;
    public Sprite empty;
    public GameObject[] displays;
    public GameObject[] names;
    public GameObject[] grayOverlays;
    private GameObject[] players;
    private GameObject serverInfo;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        turnMarker.SetActive(false);
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 1; i < players.Length; i++)
        {
            Sprite image = (Sprite)Resources.Load("Images/"+players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
            displays[i-1].GetComponent<Image>().sprite = image;
            names[i-1].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                grayOverlays[i - 1].SetActive(true);
            else
                grayOverlays[i - 1].SetActive(false);
        }
        for (int i = 0; i < displays.Length; i++)
        {
            if (displays[i].GetComponent<Image>().sprite == null)
            {
                //displays[i].SetActive(false);
                displays[i].GetComponent<Image>().sprite = empty;
            }
        }

        //turn marker
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] != null && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] != players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
            turnMarker.SetActive(true);
        else
            turnMarker.SetActive(false);

        if (turnMarker.activeInHierarchy)
        {
            for (int i = 1; i < players.Length; i++)
            {
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                {
                    turnMarker.transform.position = displays[i - 1].transform.position;
                }
            }
        }
    }

}
