using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class movement : MonoBehaviour
{
	public int moveRadius;
    public int area = 1;// 1,2,3
    private GameObject TurnManager;
    private GameObject[] players;
    private GameObject serverInfo;
    private int playerID;

    void Start()
    {
        TurnManager = GameObject.FindGameObjectWithTag("General");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        string characterName = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
    }

    void Update()
    {
        area = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area;
    }

    class Node
    {
        public int x;
        public int y;
        public int length;

        public Node(int x, int y, int length)
        {
            this.x = x;
            this.y = y;
            this.length = length;
        }
    }

	public void calcMovement()
	{
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Sprint)
        {
            moveRadius += 2;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Nature's Call" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation)
                moveRadius++;
        }

        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area != 0)
            area = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Area;

        int attemptedX;
        int attemptedY;
        int shifter;
        bool success;
        List<string> playerData = new List<string>(gameObject.GetComponent<Text>().text.Split(','));
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        List<int> xs = new List<int>();
        List<int> ys = new List<int>();
        List<string> data;
        for (int i = 0; i < tiles.Length; i++)
        {
            data = new List<string>(tiles[i].GetComponent<Text>().text.Split(','));
            xs.Add(int.Parse(data[0]));
            ys.Add(int.Parse(data[1]));
        }

        List<Node> nodes = new List<Node>();

        Node startNode = new Node(int.Parse(playerData[0]),int.Parse(playerData[1]),moveRadius);
        nodes.Add(startNode);

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].length > 0)
            {

                if(nodes[i].x % 2 == 0)//even collumns
                    shifter = -1;
                else//odd collumns
                    shifter = 0;

                //above
                attemptedX = nodes[i].x;
                attemptedY = nodes[i].y + 1;
                success = false;
                for (int j = 0; j < xs.Count; j++)//first check if coords are of a real tile
                {
                    if (attemptedX == xs[j] && attemptedY == ys[j])
                    {
                        success = true;
                        tiles[j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                for (int j = 0; j < nodes.Count; j++)//second check if a node is already here
                {
                    if (attemptedX == nodes[j].x && attemptedY == nodes[j].y)
                        success = false;
                    if (area == 1 && ((attemptedX == 24 && attemptedY == 13) || (attemptedX == 25 && attemptedY == 12) || (attemptedX == 26 && attemptedY == 12) || (attemptedX == 27 && attemptedY == 11)))
                        success = false;
                    else if (area == 2 && ((attemptedX == 0 && attemptedY == 6) || (attemptedX == 1 && attemptedY == 5) || (attemptedX == 2 && attemptedY == 5) || (attemptedX == 3 && attemptedY == 4)))
                        success = false;
                }
                if (success)
                    nodes.Add(new Node(attemptedX,attemptedY,(nodes[i].length-1)));

                //below
                attemptedX = nodes[i].x;
                attemptedY = nodes[i].y - 1;
                success = false;
                for (int j = 0; j < xs.Count; j++)//first check if coords are of a real tile
                {
                    if (attemptedX == xs[j] && attemptedY == ys[j])
                    {
                        success = true;
                        tiles[j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                for (int j = 0; j < nodes.Count; j++)//second check if a node is already here
                {
                    if (attemptedX == nodes[j].x && attemptedY == nodes[j].y)
                        success = false;
                    if (area == 1 && ((attemptedX == 24 && attemptedY == 13) || (attemptedX == 25 && attemptedY == 12) || (attemptedX == 26 && attemptedY == 12) || (attemptedX == 27 && attemptedY == 11)))
                        success = false;
                    else if (area == 2 && ((attemptedX == 0 && attemptedY == 6) || (attemptedX == 1 && attemptedY == 5) || (attemptedX == 2 && attemptedY == 5) || (attemptedX == 3 && attemptedY == 4)))
                        success = false;
                }
                if (success)
                    nodes.Add(new Node(attemptedX, attemptedY, (nodes[i].length - 1)));

                //top right
                attemptedX = nodes[i].x + 1;
                attemptedY = nodes[i].y + 1 + shifter;
                success = false;
                for (int j = 0; j < xs.Count; j++)//first check if coords are of a real tile
                {
                    if (attemptedX == xs[j] && attemptedY == ys[j])
                    {
                        success = true;
                        tiles[j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                for (int j = 0; j < nodes.Count; j++)//second check if a node is already here
                {
                    if (attemptedX == nodes[j].x && attemptedY == nodes[j].y)
                        success = false;
                    if (area == 1 && ((attemptedX == 24 && attemptedY == 13) || (attemptedX == 25 && attemptedY == 12) || (attemptedX == 26 && attemptedY == 12) || (attemptedX == 27 && attemptedY == 11)))
                        success = false;
                    else if (area == 2 && ((attemptedX == 0 && attemptedY == 6) || (attemptedX == 1 && attemptedY == 5) || (attemptedX == 2 && attemptedY == 5) || (attemptedX == 3 && attemptedY == 4)))
                        success = false;
                }
                if (success)
                    nodes.Add(new Node(attemptedX, attemptedY, (nodes[i].length - 1)));

                //bottom right
                attemptedX = nodes[i].x + 1;
                attemptedY = nodes[i].y + shifter;
                success = false;
                for (int j = 0; j < xs.Count; j++)//first check if coords are of a real tile
                {
                    if (attemptedX == xs[j] && attemptedY == ys[j])
                    {
                        success = true;
                        tiles[j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                for (int j = 0; j < nodes.Count; j++)//second check if a node is already here
                {
                    if (attemptedX == nodes[j].x && attemptedY == nodes[j].y)
                        success = false;
                    if (area == 1 && ((attemptedX == 24 && attemptedY == 13) || (attemptedX == 25 && attemptedY == 12) || (attemptedX == 26 && attemptedY == 12) || (attemptedX == 27 && attemptedY == 11)))
                        success = false;
                    else if (area == 2 && ((attemptedX == 0 && attemptedY == 6) || (attemptedX == 1 && attemptedY == 5) || (attemptedX == 2 && attemptedY == 5) || (attemptedX == 3 && attemptedY == 4)))
                        success = false;
                }
                if (success)
                    nodes.Add(new Node(attemptedX, attemptedY, (nodes[i].length - 1)));

                //top left
                attemptedX = nodes[i].x - 1;
                attemptedY = nodes[i].y + 1 + shifter;
                success = false;
                for (int j = 0; j < xs.Count; j++)//first check if coords are of a real tile
                {
                    if (attemptedX == xs[j] && attemptedY == ys[j])
                    {
                        success = true;
                        tiles[j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                for (int j = 0; j < nodes.Count; j++)//second check if a node is already here
                {
                    if (attemptedX == nodes[j].x && attemptedY == nodes[j].y)
                        success = false;
                    if (area == 1 && ((attemptedX == 24 && attemptedY == 13) || (attemptedX == 25 && attemptedY == 12) || (attemptedX == 26 && attemptedY == 12) || (attemptedX == 27 && attemptedY == 11)))
                        success = false;
                    else if (area == 2 && ((attemptedX == 0 && attemptedY == 6) || (attemptedX == 1 && attemptedY == 5) || (attemptedX == 2 && attemptedY == 5) || (attemptedX == 3 && attemptedY == 4)))
                        success = false;
                }
                if (success)
                    nodes.Add(new Node(attemptedX, attemptedY, (nodes[i].length - 1)));

                //bottom left
                attemptedX = nodes[i].x - 1;
                attemptedY = nodes[i].y + shifter;
                success = false;
                for (int j = 0; j < xs.Count; j++)//first check if coords are of a real tile
                {
                    if (attemptedX == xs[j] && attemptedY == ys[j])
                    {
                        success = true;
                        tiles[j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                for (int j = 0; j < nodes.Count; j++)//second check if a node is already here
                {
                    if (attemptedX == nodes[j].x && attemptedY == nodes[j].y)
                        success = false;
                    if (area == 1 && ((attemptedX == 24 && attemptedY == 13) || (attemptedX == 25 && attemptedY == 12) || (attemptedX == 26 && attemptedY == 12) || (attemptedX == 27 && attemptedY == 11)))
                        success = false;
                    else if (area == 2 && ((attemptedX == 0 && attemptedY == 6) || (attemptedX == 1 && attemptedY == 5) || (attemptedX == 2 && attemptedY == 5) || (attemptedX == 3 && attemptedY == 4)))
                        success = false;
                }
                if (success)
                    nodes.Add(new Node(attemptedX, attemptedY, (nodes[i].length - 1)));

            }
        }
        //manually turn off temp barriers
        int barrierX;
        int barrierY;
        for (int i = 0; i < tiles.Length; i++)
        {
            data = new List<string>(tiles[i].GetComponent<Text>().text.Split(','));
            barrierX = int.Parse(data[0]);
            barrierY = int.Parse(data[1]);
            if (area == 1 && ((barrierX == 24 && barrierY == 13) || (barrierX == 25 && barrierY == 12) || (barrierX == 26 && barrierY == 12) || (barrierX == 27 && barrierY == 11)))
                tiles[i].GetComponent<SpriteRenderer>().enabled = false;
            else if (area == 2 && ((barrierX == 0 && barrierY == 6) || (barrierX == 1 && barrierY == 5) || (barrierX == 2 && barrierY == 5) || (barrierX == 3 && barrierY == 4)))
                tiles[i].GetComponent<SpriteRenderer>().enabled = false;
        }

        //Debugs

        for (int i = 0; i < nodes.Count; i++)
        {
            //Debug.Log("Node [" + i + "]: X=" + nodes[i].x + "  Y=" + nodes[i].y);
        }

        for (int i = 0; i < xs.Count; i++)
        {
            //Debug.Log("Tile[" + i + "]: X=" + xs[i] + "  Y=" + ys[i]);
        }

        //update phase
        //TurnManager.GetComponent<turnManager>().phase++;
    }
}
