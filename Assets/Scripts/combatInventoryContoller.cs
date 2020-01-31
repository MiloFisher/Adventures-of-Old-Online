using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class combatInventoryContoller : MonoBehaviour
{
    public GameObject[] inventory;
    public GameObject error;
    public GameObject resurrectionScreen;
    private GameObject[] players;
    private GameObject serverInfo;
    private int activeTotal;
    private int playerID;
    private string characterName;
    private string inv;
    private int inventoryIterator;
    private string[] inventoryIDs = new string[7];
    private int[] cardPositions = new int[7];
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
    }

    void OnEnable()
    {
        //if(players != null)
        //    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ClearScrolls = true;
    }

    // Update is called once per frame
    void Update()
    {
        inventoryIterator = 0;
        for (int j = 0; j < players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; j++)
        {
            inv = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j];
            if (inv == "t0" || inv == "t1" || inv == "t2" || (inv == "t3" && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 21) || inv == "t4" || inv == "t5" || inv == "t6" || inv == "t7" || inv == "t9" || inv == "t10" || inv == "t11")
            {
                cardPositions[inventoryIterator] = j;
                inventoryIDs[inventoryIterator] = inv;
                inventory[inventoryIterator].SetActive(true);
                inventory[inventoryIterator].GetComponent<Image>().sprite = (Sprite)Resources.Load("Treasure Cards/" + inv, typeof(Sprite));
                inventoryIterator++;
            }
        }
        for (int i = inventory.Length - 1; i >= inventoryIterator; i--)
        {
            cardPositions[inventoryIterator] = -1;
            inventoryIDs[i] = null;
            inventory[i].SetActive(false);
        }

        //display
        activeTotal = 0;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].activeInHierarchy)
            {
                activeTotal++;
            }
        }
        if (activeTotal == 1)
        {
            inventory[0].transform.localPosition = new Vector2(0, 0);
        }
        else if (activeTotal == 2)
        {
            inventory[0].transform.localPosition = new Vector2(-55, 0);
            inventory[1].transform.localPosition = new Vector2(55, 0);
        }
        else if (activeTotal == 3)
        {
            inventory[0].transform.localPosition = new Vector2(-110, 0);
            inventory[1].transform.localPosition = new Vector2(0, 0);
            inventory[2].transform.localPosition = new Vector2(110, 0);
        }
        else if (activeTotal == 4)
        {
            inventory[0].transform.localPosition = new Vector2(-165, 0);
            inventory[1].transform.localPosition = new Vector2(-55, 0);
            inventory[2].transform.localPosition = new Vector2(55, 0);
            inventory[3].transform.localPosition = new Vector2(165, 0);
        }
        else if (activeTotal == 5)
        {
            inventory[0].transform.localPosition = new Vector2(-220, 0);
            inventory[1].transform.localPosition = new Vector2(-110, 0);
            inventory[2].transform.localPosition = new Vector2(0, 0);
            inventory[3].transform.localPosition = new Vector2(110, 0);
            inventory[4].transform.localPosition = new Vector2(220, 0);
        }
        else if (activeTotal == 6)
        {
            inventory[0].transform.localPosition = new Vector2(-275, 0);
            inventory[1].transform.localPosition = new Vector2(-165, 0);
            inventory[2].transform.localPosition = new Vector2(-55, 0);
            inventory[3].transform.localPosition = new Vector2(55, 0);
            inventory[4].transform.localPosition = new Vector2(165, 0);
            inventory[5].transform.localPosition = new Vector2(275, 0);
        }
        else if (activeTotal == 7)
        {
            inventory[0].transform.localPosition = new Vector2(-324, 0);
            inventory[1].transform.localPosition = new Vector2(-216, 0);
            inventory[2].transform.localPosition = new Vector2(-108, 0);
            inventory[3].transform.localPosition = new Vector2(0, 0);
            inventory[4].transform.localPosition = new Vector2(108, 0);
            inventory[5].transform.localPosition = new Vector2(216, 0);
            inventory[6].transform.localPosition = new Vector2(324, 0);
        }
    }

    public void Back()
    {
        gameObject.SetActive(false);
    }

    public void Use1()
    {
        ItemUse(inventoryIDs[0], cardPositions[0]);
    }
    public void Use2()
    {
        ItemUse(inventoryIDs[1], cardPositions[1]);
    }
    public void Use3()
    {
        ItemUse(inventoryIDs[2], cardPositions[2]);
    }
    public void Use4()
    {
        ItemUse(inventoryIDs[3], cardPositions[3]);
    }
    public void Use5()
    {
        ItemUse(inventoryIDs[4], cardPositions[4]);
    }
    public void Use6()
    {
        ItemUse(inventoryIDs[5], cardPositions[5]);
    }
    public void Use7()
    {
        ItemUse(inventoryIDs[6], cardPositions[6]);
    }

    public void ItemUse(string id, int position)
    {
        switch (id)
        {
            case "t0": Heal(12); Discard(position); break;
            case "t1": Heal(21); Discard(position); break;
            case "t2": Heal(30); Discard(position); break;
            case "t3":
                int deadPeople = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                        deadPeople++;
                } //King of the Undead Ability - Undead Wrath
                if (deadPeople > 0 && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 21)
                {
                    resurrectionScreen.SetActive(true);
                    Discard(position);
                }
                else
                    error.SetActive(true); break;
            case "t4": GainAbilityCharges(1); Discard(position); break;
            case "t5": GainAbilityCharges(2); Discard(position); break;
            case "t6": GainAbilityCharges(3); Discard(position); break;
            case "t7": DealDamage(); Discard(position); break;//deal damage
            case "t9": RuneOfValor(); Discard(position); break;//+5 str
            case "t10": RuneOfSwiftness(); Discard(position); break;//+5 dex
            case "t11": RuneOfWisdom(); Discard(position); break;//+5 int
        }
        if(!error.activeInHierarchy)
            Back();
    }

    public void Heal(int amount)
    {
        //The Shadow Ability - Lightless Void
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 24)
            amount = 0;

        //Spider Queen Aranne Ability - Venomous Bite
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
    }

    public void GainAbilityCharges(int amount)
    {
        int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
        ac += amount;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
    }

    public void DealDamage()
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming = true;
        //StartCoroutine(Wait());
    }

    public void RuneOfValor()
    {
        //int mod = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);
        //mod += 5;
        //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod = mod + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ValorRunesActive++;
    }

    public void RuneOfSwiftness()
    {
        //int mod = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
        //mod += 5;
        //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod = mod + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SwiftnessRunesActive++;
    }

    public void RuneOfWisdom()
    {
        //int mod = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
        //mod += 5;
        //players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod = mod + "";
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WisdomRunesActive++;
    }

    public void Discard(int position)
    {
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[position] = null;
    }

    //IEnumerator Wait()
    //{
    //    yield return new WaitForSeconds(1);
    //    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming = false;
    //}
}
