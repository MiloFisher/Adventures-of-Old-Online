using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createTiles : MonoBehaviour
{
    public GameObject tile;

    private GameObject player;
    // Start is called before the first frame update
    private float xmod = 73.5f / 27.0f;
    private float ymod = 42.0f / 18.0f;
    private GameObject[] gameTiles;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for(int row = 0; row < 19; row++)
        {
            for(int col = 0; col < 28; col++)
            {
                if (!(row == 0 && col == 0))
                {
                    if (col % 2 == 0)
                    {
                        var newTile = Instantiate(tile);
                        newTile.transform.SetParent(gameObject.transform);
                        newTile.transform.localPosition = new Vector2(tile.transform.localPosition.x + (col * xmod), tile.transform.localPosition.y + (row * ymod));
                        newTile.GetComponent<Text>().text = col + "," + row;
                    }
                    else if (row < 18)
                    {
                        var newTile = Instantiate(tile);
                        newTile.transform.SetParent(gameObject.transform);
                        newTile.transform.localPosition = new Vector2(tile.transform.localPosition.x + (col * xmod), tile.transform.localPosition.y + (row * ymod) + ymod / 2);
                        newTile.GetComponent<Text>().text = col + "," + row;
                    }
                }
            }
        }

        StartCoroutine(neutralize());
    }

    IEnumerator neutralize()
    {
        //data = new List<string>(gameTiles[i].GetComponent<Text>().text.Split(','));

        //string data = "0,16";

        yield return new WaitForSeconds(0.1f);
        gameTiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < gameTiles.Length; i++)
            gameTiles[i].GetComponent<destroyTiles>().neutralized = true;


        player.GetComponent<Text>().text = "0,16";
        //player.GetComponent<movement>().calcMovement();

        //var newNode = Instantiate(node);
        //newNode.GetComponent<Text>().text = data;
        //newNode.transform.position = player.transform.position;
        //newNode.GetComponent<nodeController>().Spread(moveRadius);

        //yield return new WaitForSeconds(0.5f);
        //GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        //for (int i = 0; i < nodes.Length; i++)
        //    nodes[i].layer = 11;

        //yield return new WaitForSeconds(0.1f);
        //GameObject[] tests = GameObject.FindGameObjectsWithTag("Test");
        //for (int i = 0; i < tests.Length; i++)
        //{
        //    if (tests[i].name != "Test Collider")
        //        Destroy(tests[i]);
        //}
    }
}
