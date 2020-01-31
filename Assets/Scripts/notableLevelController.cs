using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class notableLevelController : MonoBehaviour
{
    public GameObject strengthDisplay;
    public GameObject dexterityDisplay;
    public GameObject intellectDisplay;
    public GameObject healthDisplay;
    public GameObject maxHealthDisplay;
    public GameObject strengthButton;
    public GameObject dexterityButton;
    public GameObject intellectButton;

    private GameObject[] players;
    private int playerID;
    private int strength;
    private int dexterity;
    private int intellect;
    private int health;
    private int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
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

    // Update is called once per frame
    void Update()
    {
        strength = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength);
        dexterity = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity);
        intellect = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect);
        health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);


        strengthDisplay.GetComponent<Text>().text = strength + "";
        dexterityDisplay.GetComponent<Text>().text = dexterity + "";
        intellectDisplay.GetComponent<Text>().text = intellect + "";
        healthDisplay.GetComponent<Text>().text = health + "";
        maxHealthDisplay.GetComponent<Text>().text = maxHealth + "";

        if (strength >= 16)
            strengthButton.SetActive(false);
        else
            strengthButton.SetActive(true);

        if (dexterity >= 16)
            dexterityButton.SetActive(false);
        else
            dexterityButton.SetActive(true);

        if (intellect >= 16)
            intellectButton.SetActive(false);
        else
            intellectButton.SetActive(true);
    }

    public void incrementStrength()
    {
        int temp = strength + 1;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength = temp + "";
        buffDamage();
        gameObject.SetActive(false);
    }

    public void incrementDexterity()
    {
        int temp = dexterity + 1;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity = temp + "";
        buffDamage();
        gameObject.SetActive(false);
    }

    public void incrementIntellect()
    {
        int temp = intellect + 1;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect = temp + "";
        buffDamage();
        gameObject.SetActive(false);
    }

    public void incrementHealth()
    {
        int temp = health + 3;
        int temp2 = maxHealth + 3;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = temp + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth = temp2 + "";
        buffDamage();
        gameObject.SetActive(false);
    }

    public void buffDamage()
    {
        int damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage);
        damage++;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage = damage + "";
    }
}
