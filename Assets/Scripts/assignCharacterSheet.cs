using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class assignCharacterSheet : MonoBehaviour
{
    public GameObject characterSheet;
    public GameObject displayCards;
    public GameObject x;
    public GameObject magnifiedCard;
    public GameObject mapCamera;
    public GameObject[] names;
    public GameObject[] targets;
    private GameObject[] players;
    private GameObject serverInfo;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine(); 
        string characterName = sr.ReadLine();
        sr.Close();

        characterSheet.GetComponent<characterSheetController>().characterName = characterName;
        displayCards.GetComponent<displayCards>().characterName = characterName;
        x.SetActive(false);
    }

    public void player1()
    {
        if (names[0].GetComponent<Text>().text != null)
        {
            characterSheet.GetComponent<characterSheetController>().characterName = names[0].GetComponent<Text>().text;
            displayCards.GetComponent<displayCards>().characterName = names[0].GetComponent<Text>().text;
            x.SetActive(true);
            magnifiedCard.SetActive(false);
            mapCamera.GetComponent<followCamera>().target = targets[1];
        }
    }
    public void player2()
    {
        if (names[1].GetComponent<Text>().text != null)
        {
            characterSheet.GetComponent<characterSheetController>().characterName = names[1].GetComponent<Text>().text;
            displayCards.GetComponent<displayCards>().characterName = names[1].GetComponent<Text>().text;
            x.SetActive(true);
            magnifiedCard.SetActive(false);
            mapCamera.GetComponent<followCamera>().target = targets[2];
        }
    }
    public void player3()
    {
        if (names[2].GetComponent<Text>().text != null)
        {
            characterSheet.GetComponent<characterSheetController>().characterName = names[2].GetComponent<Text>().text;
            displayCards.GetComponent<displayCards>().characterName = names[2].GetComponent<Text>().text;
            x.SetActive(true);
            magnifiedCard.SetActive(false);
            mapCamera.GetComponent<followCamera>().target = targets[3];
        }
    }
    public void player4()
    {
        if (names[3].GetComponent<Text>().text != null)
        {
            characterSheet.GetComponent<characterSheetController>().characterName = names[3].GetComponent<Text>().text;
            displayCards.GetComponent<displayCards>().characterName = names[3].GetComponent<Text>().text;
            x.SetActive(true);
            magnifiedCard.SetActive(false);
            mapCamera.GetComponent<followCamera>().target = targets[4];
        }
    }
    public void player5()
    {
        if (names[4].GetComponent<Text>().text != null)
        {
            characterSheet.GetComponent<characterSheetController>().characterName = names[4].GetComponent<Text>().text;
            displayCards.GetComponent<displayCards>().characterName = names[4].GetComponent<Text>().text;
            x.SetActive(true);
            magnifiedCard.SetActive(false);
            mapCamera.GetComponent<followCamera>().target = targets[5];
        }
    }
    public void revertPlayer()
    {
        string fileName = "playerInfo";
        var read = File.OpenText(fileName);
        var username = read.ReadLine();
        string n = read.ReadLine();
        characterSheet.GetComponent<characterSheetController>().characterName = n;
        displayCards.GetComponent<displayCards>().characterName = n;
        read.Close();
        x.SetActive(false);
        magnifiedCard.SetActive(false);
        mapCamera.GetComponent<followCamera>().target = targets[0];
    }
}
