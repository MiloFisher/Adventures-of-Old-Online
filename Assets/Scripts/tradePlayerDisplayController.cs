using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tradePlayerDisplayController : MonoBehaviour
{
    public Sprite empty;
    public GameObject[] displays;
    public GameObject[] names;
    public GameObject[] outOfRanges;
    private GameObject[] players;
    private List<string> location;
    private int playerX;
    private int playerY;
    private int targetX;
    private int targetY;
    private int shifter;
    private bool tradingPost;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < outOfRanges.Length; i++)
            outOfRanges[i].SetActive(false);
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        location = new List<string>(players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location.Split(','));
        playerX = int.Parse(location[0]);
        playerY = int.Parse(location[1]);
        if (playerX % 2 == 0)
            shifter = -1;
        else
            shifter = 0;

        tradingPost = false;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradingPostActive)
                tradingPost = true;
        }

        for (int i = 1; i < players.Length; i++)
        {
            location = new List<string>(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location.Split(','));
            targetX = int.Parse(location[0]);
            targetY = int.Parse(location[1]);

            Sprite image = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
            displays[i - 1].GetComponent<Image>().sprite = image;
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            {
                outOfRanges[i - 1].SetActive(true);
                names[i - 1].GetComponent<Text>().text = "Dead";
            }
            else if ((targetX == playerX && targetY == playerY) || (targetX == playerX && targetY == playerY + 1) || (targetX == playerX && targetY == playerY - 1) || (targetX == playerX + 1 && targetY == playerY + 1 + shifter) || (targetX == playerX + 1 && targetY == playerY + shifter) || (targetX == playerX - 1 && targetY == playerY + 1 + shifter) || (targetX == playerX - 1 && targetY == playerY + shifter))
            {
                names[i - 1].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                outOfRanges[i - 1].SetActive(false);
            }
            else if (tradingPost)
            {
                names[i - 1].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                outOfRanges[i - 1].SetActive(false);
            }
            else
            {
                outOfRanges[i - 1].SetActive(true);
                names[i - 1].GetComponent<Text>().text = "Out of Range";
            }
        }
        for (int i = 0; i < displays.Length; i++)
        {
            if (displays[i].GetComponent<Image>().sprite == null)
            {
                //displays[i].SetActive(false);
                displays[i].GetComponent<Image>().sprite = empty;
            }
        }
    }
}
