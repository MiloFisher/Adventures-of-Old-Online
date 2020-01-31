using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drawOtherPlayers : MonoBehaviour
{
    public GameObject tile;
    public GameObject fullMap;
    public GameObject[] playerPieces;

    private GameObject[] players;
    private GameObject[] tiles;
    private List<string> data;
    private float xmod = 73.5f / 27.0f;
    private float ymod = 42.0f / 18.0f;
    private float x;
    private float y;
    private float shifter;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = "0,16";
        }
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 1; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location != null && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location != "")
            {
                playerPieces[i].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                data = new List<string>(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location.Split(','));
                if (data[0] != null)
                {
                    x = float.Parse(data[0]) * xmod + tile.transform.localPosition.x;
                    if (int.Parse(data[0]) % 2 == 0)
                        y = float.Parse(data[1]) * ymod + tile.transform.localPosition.y;
                    else
                        y = float.Parse(data[1]) * ymod + ymod / 2f + tile.transform.localPosition.y;
                    if (fullMap.transform.position.x < 140)
                        shifter = 0;
                    else
                        shifter = 140;

                    playerPieces[i].transform.position = new Vector2(x + shifter, y);
                }
            }
        }
        for (int i = players.Length; i < 6; i++)
        {
            if (i > 0)
                playerPieces[i].SetActive(false);
        }

        for (int i = 0; i < playerPieces.Length; i++)
        {
            for (int j = 0; j < playerPieces.Length; j++)
            {
                if (i != j && playerPieces[i].transform.position == playerPieces[j].transform.position)
                {
                    playerPieces[j].transform.position = new Vector2(playerPieces[j].transform.position.x, playerPieces[j].transform.position.y+.2f);
                }
            }
        }

        //colors
        for (int i = 0; i < players.Length; i++)
        {
            switch (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color)
            {
                case "Red":
                    playerPieces[i].GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case "Blue":
                    playerPieces[i].GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case "Green":
                    playerPieces[i].GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case "Yellow":
                    playerPieces[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case "Cyan":
                    playerPieces[i].GetComponent<SpriteRenderer>().color = Color.cyan;
                    break;
                case "Magenta":
                    playerPieces[i].GetComponent<SpriteRenderer>().color = Color.magenta;
                    break;
                //default:
                    //Debug.Log("No Color selected yet");
                    //switch (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ID)
                    //{
                    //    case 1:
                    //        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color = "Red";
                    //        break;
                    //    case 2:
                    //        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color = "Blue";
                    //        break;
                    //    case 3:
                    //        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color = "Green";
                    //        break;
                    //    case 4:
                    //        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color = "Yellow";
                    //        break;
                    //    case 5:
                    //        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color = "Cyan";
                    //        break;
                    //    case 6:
                    //        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Color = "Magenta";
                    //        break;
                    //    default:
                    //        Debug.Log("My mans is just straight up dumb");
                    //        break;
                    //}
                    //break;
            }
        }
    }
}
