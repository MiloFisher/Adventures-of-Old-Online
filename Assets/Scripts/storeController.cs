using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storeController : MonoBehaviour
{
    public Text gold;
    public GameObject[] cards;
    private GameObject serverInfo;
    private GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
    }

    // Update is called once per frame
    void Update()
    {
        if (serverInfo != null)
        {
            for(int i = 0; i < cards.Length; i++)
                cards[i].GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Store[i], typeof(Sprite));
        }
        gold.text = "Gold: " + players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold;
    }
}
