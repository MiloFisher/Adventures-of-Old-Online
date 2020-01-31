using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hoveringOver : MonoBehaviour
{
    public Sprite cardBack;
    public GameObject bossCard;
    public GameObject playerNamesDisplay;
    public GameObject[] frames;
    public GameObject[] names;
    public GameObject gameTileMaster;
    private GameObject player;
    private GameObject[] gameTiles;
    private GameObject TurnManager;
    private GameObject[] players;
    private GameObject serverInfo;
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Text>().enabled = false;
        gameObject.GetComponentInChildren<Button>().enabled = false;
        TurnManager = GameObject.FindGameObjectWithTag("General");
        players = GameObject.FindGameObjectsWithTag("Player Info");
    }

    void OnMouseOver()
    {
        bool hasPlayers = false;
        int nameIterator = 0;
        //gameObject.GetComponent<SpriteRenderer>().enabled = true;
        if(gameObject.GetComponent<SpriteRenderer>().enabled)
            gameObject.GetComponentInChildren<Button>().enabled = true;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == gameObject.GetComponent<Text>().text)
            {
                hasPlayers = true;
                names[nameIterator].GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                nameIterator++;
            }
        }
        for (int i = nameIterator; i < names.Length; i++)
        {
            names[i].GetComponent<Text>().text = null;
        }


        if (hasPlayers)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].GetComponent<Text>().text == null || names[i].GetComponent<Text>().text == "")
                    frames[i].SetActive(false);
                else
                    frames[i].SetActive(true);
            }
            playerNamesDisplay.transform.position = transform.position;
            playerNamesDisplay.SetActive(true);
        }
        else
        {
            playerNamesDisplay.SetActive(false);
        }

        //boss card display
        if (gameObject.GetComponent<Text>().text == "26,16")
        {
            bossCard.transform.position = new Vector2(transform.position.x - 5.8f,transform.position.y);
            if (players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss1)
                bossCard.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[0], typeof(Sprite));
            else
                bossCard.GetComponent<Image>().sprite = cardBack;
            bossCard.SetActive(true);
        }
        else if (gameObject.GetComponent<Text>().text == "1,8")
        {
            bossCard.transform.position = new Vector2(transform.position.x + 5.8f, transform.position.y);
            if (players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss2)
                bossCard.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[1], typeof(Sprite));
            else
                bossCard.GetComponent<Image>().sprite = cardBack;
            bossCard.SetActive(true);
        }
        else if (gameObject.GetComponent<Text>().text == "26,2")
        {
            bossCard.transform.position = new Vector2(transform.position.x - 5.8f, transform.position.y);
            if (players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss3)
                bossCard.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().BossDeck[2], typeof(Sprite));
            else
                bossCard.GetComponent<Image>().sprite = cardBack;
            bossCard.SetActive(true);
        }
        else
            bossCard.SetActive(false);
    }

    void OnMouseExit()
    {
        //gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponentInChildren<Button>().enabled = false;
        playerNamesDisplay.SetActive(false);
        bossCard.SetActive(false);
    }

    public void outputPosition()
    {
        //Debug.Log(gameObject.GetComponent<Text>().text);
        player.transform.position = gameObject.transform.position;
        gameTiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < gameTiles.Length; i++)
            gameTiles[i].GetComponent<SpriteRenderer>().enabled = false;
        //List<string> data = new List<string>(gameObject.GetComponent<Text>().text.Split(','));
        //player.transform.localPosition = new Vector2(int.Parse(data[0]), int.Parse(data[1]));


        GameObject[] players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (i == 0)
            {
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location = gameObject.GetComponent<Text>().text;
            }
        }
        player.GetComponent<Text>().text = gameObject.GetComponent<Text>().text;

        if (TurnManager.GetComponent<turnManager>().phase == 7)
        {
            TurnManager.GetComponent<turnManager>().endTurn();
        }
        else if (TurnManager.GetComponent<turnManager>().phase == 3)
        {
            TurnManager.GetComponent<turnManager>().continueButton.SetActive(true);
        }
        //player.GetComponent<movement>().calcMovement();
        //StartCoroutine(generateMoveArea());
    }

    IEnumerator generateMoveArea()
    {
        //string data = "";

        yield return new WaitForSeconds(0.1f);

        player.GetComponent<Text>().text = gameObject.GetComponent<Text>().text;
        //player.GetComponent<movement>().calcMovement();

        //gameTiles = GameObject.FindGameObjectsWithTag("Tile");
        //for (int i = 0; i < gameTiles.Length; i++)
        //{
        //    gameTiles[i].GetComponent<destroyTiles>().neutralized = true;
        //    if (gameTiles[i].transform.position == player.transform.position)
        //    {
        //        data = gameTiles[i].GetComponent<Text>().text;
        //    }
        //}

        //GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        //for (int i = 0; i < nodes.Length; i++)
        //    nodes[i].layer = 10;

        //var newNode = Instantiate(node);
        //newNode.GetComponent<Text>().text = data;
        //newNode.transform.position = player.transform.position;
        //newNode.GetComponent<nodeController>().Spread(gameTileMaster.GetComponent<createTiles>().moveRadius);

        //yield return new WaitForSeconds(0.5f);
        //nodes = GameObject.FindGameObjectsWithTag("Node");
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
