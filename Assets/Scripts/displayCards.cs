using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class displayCards : MonoBehaviour
{
    public string characterName;
    public GameObject failMessage;
    public GameObject failMessage2;
    public GameObject failMessage3;
    public GameObject magnifiedCard;
    public GameObject equipOption;
    public GameObject equipMainHandOption;
    public GameObject equipOffHandOption;
    public GameObject unequipOption;
    public GameObject useOption;
    public GameObject discardOption;
    public GameObject[] inventory;
    public GameObject[] equipment;
    public GameObject error;
    public GameObject error2;
    public GameObject resurrectionScreen;
    public GameObject teleportScreen;
    //public GameObject quest;
    private GameObject[] players;
    private Sprite image;
    private string itemID = "";
    private int area = 0;
    private int slotID = 0;
    private int playerID = 0;
    // Start is called before the first frame update
    void Start()
    {
        magnifiedCard.SetActive(false);
        failMessage.SetActive(false);
        failMessage2.SetActive(false);
        error.SetActive(false);
        error2.SetActive(false);
        resurrectionScreen.SetActive(false);
        teleportScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                //inventory
                for (int j = 0; j < players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory.Length; j++)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j] != null && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j] != "")
                    {
                        inventory[j].SetActive(true);
                        image = (Sprite)Resources.Load("Treasure Cards/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[j], typeof(Sprite));
                        inventory[j].GetComponent<Image>().sprite = image;
                    }
                    else
                    {
                        inventory[j].SetActive(false);
                    }
                }
                //equipment
                for (int j = 0; j < players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment.Length; j++)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[j] != null && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[j] != "")
                    {
                        equipment[j].SetActive(true);
                        image = (Sprite)Resources.Load("Treasure Cards/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[j], typeof(Sprite));
                        equipment[j].GetComponent<Image>().sprite = image;
                    }
                    else
                    {
                        equipment[j].SetActive(false);
                    }
                }
            }
        }
    }

    public void Deselect()
    {
        magnifiedCard.SetActive(false);
    }

    public void SelectInventory1()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[0].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[0].transform.position.x,-3);
        ShowOptions(0,0);
    }

    public void SelectInventory2()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[1].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[1].transform.position.x, -3);
        ShowOptions(0,1);
    }

    public void SelectInventory3()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[2].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[2].transform.position.x, -3);
        ShowOptions(0,2);
    }

    public void SelectInventory4()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[3].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[3].transform.position.x, -3);
        ShowOptions(0,3);
    }

    public void SelectInventory5()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[4].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[4].transform.position.x, -3);
        ShowOptions(0,4);
    }

    public void SelectInventory6()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[5].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[5].transform.position.x, -3);
        ShowOptions(0,5);
    }

    public void SelectInventory7()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = inventory[6].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(inventory[6].transform.position.x, -3);
        ShowOptions(0,6);
    }

    public void SelectHead()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = equipment[0].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(equipment[0].transform.position.x, 9.5f);
        ShowOptions(1, 0);
    }

    public void SelectChest()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = equipment[1].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(equipment[1].transform.position.x, 9.5f);
        ShowOptions(1, 1);
    }

    public void SelectLegs()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = equipment[2].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(equipment[2].transform.position.x, 9.5f);
        ShowOptions(1, 2);
    }

    public void SelectGloves()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = equipment[3].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(equipment[3].transform.position.x, 9.5f);
        ShowOptions(1, 3);
    }

    public void SelectMainhand()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = equipment[4].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(equipment[4].transform.position.x, 9.5f);
        ShowOptions(1, 4);
    }

    public void SelectOffhand()
    {
        magnifiedCard.SetActive(true);
        magnifiedCard.GetComponent<Image>().sprite = equipment[5].GetComponent<Image>().sprite;
        magnifiedCard.transform.position = new Vector2(equipment[5].transform.position.x, 9.5f);
        ShowOptions(1, 5);
    }

    public void ShowOptions(int area, int slotID)
    {
        equipOption.SetActive(false);
        equipMainHandOption.SetActive(false);
        equipOffHandOption.SetActive(false);
        unequipOption.SetActive(false);
        useOption.SetActive(false);
        discardOption.SetActive(false);

        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        string playingAs = sr.ReadLine();
        sr.Close();

        if (characterName == playingAs)//only gives options on player's own inventory
        {
            players = GameObject.FindGameObjectsWithTag("Player Info");
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
                {
                    //sets itemID to selected item
                    if(area == 0)
                        itemID = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID];
                    else if (area == 1)
                        itemID = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[slotID];
                    this.area = area;
                    this.slotID = slotID;
                    playerID = i;
                }
            }

            if (area == 0)//card is in inventory
            {
                if (itemID.Substring(0, 1) == "t")//treasure items are useable
                {
                    useOption.SetActive(true);
                }
                else if (itemID.Substring(0, 1) == "a")//armor items are equippable based on class
                {
                    TextAsset textfile = (TextAsset)Resources.Load("Class Info/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class.ToLower() + "_info", typeof(TextAsset));
                    List<string> data = new List<string>(textfile.text.Split('\n'));

                    string id = itemID.Substring(1, itemID.Length - 1);

                    if (data[3] == "Light")
                    {
                        //light armor check
                        if (id == "8" || id == "9" || id == "10" || id == "11" || id == "20" || id == "21" || id == "22" || id == "23" || id == "24" || id == "29" || id == "31")
                        {
                            //legendary check
                            if (id == "24" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Sorcerer")
                                equipOption.SetActive(true);
                            else if (id == "29" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Necromancer")
                                equipOption.SetActive(true);
                            else if (id == "31" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Druid")
                                equipOption.SetActive(true);
                            else
                                equipOption.SetActive(true);
                        }
                    }
                    else if (data[3] == "Medium")
                    {
                        //medium armor check
                        if (id == "4" || id == "5" || id == "6" || id == "7" || id == "16" || id == "17" || id == "18" || id == "19" || id == "25" || id == "27" || id == "32" || id == "33")
                        {
                            //legendary check
                            if (id == "25" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Hunter")
                                equipOption.SetActive(true);
                            else if (id == "27" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Rogue")
                                equipOption.SetActive(true);
                            else if ((id == "32" || id == "33") && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Monk")
                                equipOption.SetActive(true);
                            else
                                equipOption.SetActive(true);
                        }
                    }
                    else if (data[3] == "Heavy")
                    {
                        //heavy armor check
                        if (id == "0" || id == "1" || id == "2" || id == "3" || id == "12" || id == "13" || id == "14" || id == "15" || id == "26" || id == "28" || id == "30")
                        {
                            //legendary check
                            if (id == "26" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Cleric")
                                equipOption.SetActive(true);
                            else if (id == "28" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Paladin")
                                equipOption.SetActive(true);
                            else if (id == "30" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Warrior")
                                equipOption.SetActive(true);
                            else
                                equipOption.SetActive(true);
                        }
                    }
                }
                else if (itemID.Substring(0, 1) == "w")//weapons can be equipped to main hand, off hand, or both
                {
                    TextAsset textfile = (TextAsset)Resources.Load("Class Info/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class.ToLower() + "_info", typeof(TextAsset));
                    List<string> data = new List<string>(textfile.text.Split('\n'));
                    List<string> weapons = new List<string>(data[4].Split(','));
                    for (int i = 0; i < weapons.Count; i++)
                    {
                        //Debug.Log(weapons[i]);
                        if (weapons[i].Substring(0, 1) == " ")
                            weapons[i] = weapons[i].Substring(1, weapons[i].Length - 1);
                    }

                    string id = itemID.Substring(1, itemID.Length - 1);

                    for (int i = 0; i < weapons.Count; i++)
                    {
                        if (weapons[i] == "Swords")
                        {
                            //swords check
                            if (id == "0" || id == "2" || id == "7" || id == "9" || id == "18" || id == "20")
                            {
                                //legendary check
                                if (id == "18" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Paladin")
                                    equipMainHandOption.SetActive(true);
                                else if (id == "20" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Warrior")
                                {
                                    equipMainHandOption.SetActive(true);
                                    equipOffHandOption.SetActive(true);
                                }
                                else
                                {
                                    if (id == "0" || id == "7")
                                        equipMainHandOption.SetActive(true);
                                    else
                                    {
                                        equipMainHandOption.SetActive(true);
                                        equipOffHandOption.SetActive(true);
                                    }
                                }
                            }
                        }
                        else if (weapons[i] == "Shields")
                        {
                            //shields check
                            if (id == "1" || id == "8")
                            {
                                equipOffHandOption.SetActive(true);
                            }
                        }
                        else if (weapons[i] == "Bows")
                        {
                            //bows check
                            if (id == "3" || id == "10" || id == "15")
                            {
                                //legendary check
                                if (id == "15" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Hunter")
                                    equipMainHandOption.SetActive(true);
                                else
                                    equipMainHandOption.SetActive(true);
                            }
                        }
                        else if (weapons[i] == "Staves")
                        {
                            //staves check
                            if (id == "4" || id == "11" || id == "14" || id == "21")
                            {
                                //legendary check
                                if (id == "14" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Sorcerer")
                                    equipMainHandOption.SetActive(true);
                                else if (id == "21" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Druid")
                                    equipMainHandOption.SetActive(true);
                                else
                                    equipMainHandOption.SetActive(true);
                            }
                        }
                        else if (weapons[i] == "Scythes")
                        {
                            //legendary check
                            if (id == "19" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Necromancer")
                                equipMainHandOption.SetActive(true);
                        }
                        else if (weapons[i] == "1-Handed Swords")
                        {
                            //1-handed swords check
                            if (id == "2" || id == "9" ||id == "20")
                            {
                                //legendary check
                                if (id == "20" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Warrior")
                                {
                                    equipMainHandOption.SetActive(true);
                                    equipOffHandOption.SetActive(true);
                                }
                                else
                                {
                                    equipMainHandOption.SetActive(true);
                                    equipOffHandOption.SetActive(true);
                                }
                            }
                        }
                        else if (weapons[i] == "Daggers")
                        {
                            //legendary check
                            if (id == "17" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Rogue")
                            {
                                equipMainHandOption.SetActive(true);
                                equipOffHandOption.SetActive(true);
                            }
                        }
                        else if (weapons[i] == "Hammers")
                        {
                            //hammers check
                            if (id == "5" || id == "6" || id == "12" || id == "13" || id == "16")
                            {
                                //legendary check
                                if (id == "16" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Cleric")
                                {
                                    equipMainHandOption.SetActive(true);
                                    equipOffHandOption.SetActive(true);
                                }
                                else
                                {
                                    if (id == "6" || id == "13")
                                        equipMainHandOption.SetActive(true);
                                    else
                                    {
                                        equipMainHandOption.SetActive(true);
                                        equipOffHandOption.SetActive(true);
                                    }
                                }
                            }
                        }
                    }
                }

                discardOption.SetActive(true);//can discard inventory cards no matter what
            }
            else if (area == 1)//card is equipped
            {
                unequipOption.SetActive(true);//can only unequip equipped items
            }
        }
    }

    public void equip()
    {
        magnifiedCard.SetActive(false);
        //itemID ex: t8
        //slotID 0 - 6
        //playerID 0 - 5

        string id = itemID.Substring(1, itemID.Length - 1);
        string temp;

        //check if item is a helmet
        if (id == "0" || id == "4" || id == "8" || id == "12" || id == "16" || id == "20" || id == "25" || id == "29" || id == "30")
        {
            //check if head slot is empty
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] == "")
            {
                if (id == "0" || id == "12")
                {
                    if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 1)
                    {
                        //if so, assign item to head slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                        failMessage2.SetActive(true);
                }
                else
                {
                    if (id == "25" || id == "29" || id == "30")
                    {
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "")
                            failMessage3.SetActive(true);
                        else
                        {
                            //if so, assign item to head slot, and remove from inventory
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                        }
                    }
                    else
                    {
                        //if so, assign item to head slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                }
            }
            else
            {
				if (id == "0" || id == "12")
				{
					if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 1)
					{
						//otherwise, assign item to head slot, and head slot item to inventory
						temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0];
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] = itemID;
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
					}
					else
						failMessage2.SetActive(true);
				}
				else
				{
					//otherwise, assign item to head slot, and head slot item to inventory
					temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0];
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0] = itemID;
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
				}
            }
        }//check if item is a chestplate
        else if (id == "1" || id == "5" || id == "9" || id == "13" || id == "17" || id == "21" || id == "24" || id == "27" || id == "28")
        {
            //check if chest slot is empty
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] == "")
            {
                if (id == "1" || id == "13")
                {
                    if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 2)
                    {
                        //if so, assign item to chest slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                        failMessage2.SetActive(true);
                }
                else
                {
                    if (id == "24" || id == "27" || id == "28")
                    {
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "")
                            failMessage3.SetActive(true);
                        else
                        {
                            //if so, assign item to head slot, and remove from inventory
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                        }
                    }
                    else
                    {
                        //if so, assign item to head slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                }
            }
            else
            {
				if (id == "1" || id == "13")
				{
					if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 1)
					{
						//otherwise, assign item to chest slot, and chest slot item to inventory
						temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1];
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] = itemID;
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
					}
					else
						failMessage2.SetActive(true);
				}
				else
				{
					//otherwise, assign item to chest slot, and chest slot item to inventory
					temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1];
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1] = itemID;
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
				}
            }
        }//check if item is boots
        else if (id == "2" || id == "6" || id == "10" || id == "14" || id == "18" || id == "22" || id == "31" || id == "33")
        {
            //check if boots slot is empty
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] == "")
            {
                if (id == "2" || id == "14")
                {
                    if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 2)
                    {
                        //if so, assign item to boots slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                        failMessage2.SetActive(true);
                }
                else
                {
                    if (id == "31" || id == "33")
                    {
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "")
                            failMessage3.SetActive(true);
                        else
                        {
                            //if so, assign item to head slot, and remove from inventory
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                        }
                    }
                    else
                    {
                        //if so, assign item to head slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                }
            }
            else
            {
				if (id == "2" || id == "14")
				{
					if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 1)
					{
						//otherwise, assign item to boots slot, and chest slot item to inventory
						temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2];
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] = itemID;
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
					}
					else
						failMessage2.SetActive(true);
				}
				else
				{
					//otherwise, assign item to boots slot, and boots slot item to inventory
					temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2];
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2] = itemID;
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
				}
            }
        }//check if item is gloves
        else if (id == "3" || id == "7" || id == "11" || id == "15" || id == "19" || id == "23" || id == "26" || id == "32")
        {
            //check if gloves slot is empty
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] == "")
            {
                if (id == "3" || id == "15")
                {
                    if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 1)
                    {
                        //if so, assign item to gloves slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                        failMessage2.SetActive(true);
                }
                else
                {
                    if (id == "26" || id == "32")
                    {
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "")
                            failMessage3.SetActive(true);
                        else
                        {
                            //if so, assign item to head slot, and remove from inventory
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                        }
                    }
                    else
                    {
                        //if so, assign item to head slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                }
            }
            else
            {
				if (id == "3" || id == "15")
				{
					if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) > 1)
					{
						//otherwise, assign item to gloves slot, and gloves slot item to inventory
						temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3];
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] = itemID;
						players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
					}
					else
						failMessage2.SetActive(true);
				}
				else
				{
					//otherwise, assign item to gloves slot, and gloves slot item to inventory
					temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3];
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3] = itemID;
					players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
				}
            }
        }

        //finally shift inventory items down
        for (int i = 0; i < 6; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1] = null;
            }
        }
    }

    public void equipMainHand()
    {
        magnifiedCard.SetActive(false);
        //itemID ex: t8
        //slotID 0 - 6
        //playerID 0 - 5

        string id = itemID.Substring(1, itemID.Length - 1);
        string temp;

        if (id == "14" || id == "15" || id == "16" || id == "17" || id == "18" || id == "19" || id == "20" || id == "21")
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "")
                failMessage3.SetActive(true);
            else
            {
                //check if item is 1 handed
                if (id == "2" || id == "5" || id == "9" || id == "12" || id == "16" || id == "17" || id == "20")
                {
                    //check if main hand slot is empty
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == "")
                    {
                        //if so, assign item to main hand slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                    {
                        //otherwise, assign item to main hand slot, and main hand slot item to inventory
                        temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                    }
                }//check if item is 2 handed
                else if (id == "0" || id == "3" || id == "4" || id == "6" || id == "7" || id == "10" || id == "11" || id == "13" || id == "14" || id == "15" || id == "18" || id == "19" || id == "21")
                {
                    //check if main hand slot is empty
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == "")
                    {
                        //check if off hand slot is empty
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                        {
                            //if so, assign item to main hand slot, and remove from inventory
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                        }
                        else
                        {
                            //otherwise, assign item to main hand slot, and off hand slot item to inventory
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = null;
                        }
                    }
                    else
                    {
                        //check if off hand slot is empty
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                        {
                            //if so, assign item to main hand slot, and main hand slot item to inventory
                            temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                        }
                        else
                        {
                            //otherwise, check if inventory has empty slot, then assign item to main hand slot, and main hand slot item plus off hand slot item to inventory
                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
                            {
                                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == "")
                                {
                                    temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = null;
                                }
                                else
                                {
                                    failMessage.SetActive(true);
                                    //.Log("Recognized as a dwarf");
                                }
                            }
                            else
                            {
                                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == "")
                                {
                                    temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = null;
                                }
                                else
                                {
                                    failMessage.SetActive(true);
                                    //Debug.Log("Definitely not recognized as a dwarf");
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            //check if item is 1 handed
            if (id == "2" || id == "5" || id == "9" || id == "12" || id == "16" || id == "17" || id == "20")
            {
                //check if main hand slot is empty
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == "")
                {
                    //if so, assign item to main hand slot, and remove from inventory
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                }
                else
                {
                    //otherwise, assign item to main hand slot, and main hand slot item to inventory
                    temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                }
            }//check if item is 2 handed
            else if (id == "0" || id == "3" || id == "4" || id == "6" || id == "7" || id == "10" || id == "11" || id == "13" || id == "14" || id == "15" || id == "18" || id == "19" || id == "21")
            {
                //check if main hand slot is empty
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] == "")
                {
                    //check if off hand slot is empty
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                    {
                        //if so, assign item to main hand slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                    {
                        //otherwise, assign item to main hand slot, and off hand slot item to inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = null;
                    }
                }
                else
                {
                    //check if off hand slot is empty
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                    {
                        //if so, assign item to main hand slot, and main hand slot item to inventory
                        temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                    }
                    else
                    {
                        //otherwise, check if inventory has empty slot, then assign item to main hand slot, and main hand slot item plus off hand slot item to inventory
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
                        {
                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == "")
                            {
                                temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = null;
                            }
                            else
                            {
                                failMessage.SetActive(true);
                                //.Log("Recognized as a dwarf");
                            }
                        }
                        else
                        {
                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == "")
                            {
                                temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = itemID;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = null;
                            }
                            else
                            {
                                failMessage.SetActive(true);
                                //Debug.Log("Definitely not recognized as a dwarf");
                            }
                        }
                    }
                }
            }
        }
        

        //finally shift inventory items down *special shift*
        for (int k = 0; k < 7; k++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1] = null;
                }
            }
        }
    }

    public void equipOffHand()
    {
        magnifiedCard.SetActive(false);
        //itemID ex: t8
        //slotID 0 - 6
        //playerID 0 - 5
        string id1 = itemID.Substring(1, itemID.Length - 1);
		string id = "";
        if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] != "")
            id = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4].Substring(1, players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4].Length - 1);
        string temp;

        if (id1 == "16" || id1 == "17" || id1 == "20")
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != null && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "")
                failMessage3.SetActive(true);
            else
            {
                //check if main hand is two handed
                if (id == "0" || id == "3" || id == "4" || id == "6" || id == "7" || id == "10" || id == "11" || id == "13" || id == "14" || id == "15" || id == "18" || id == "19" || id == "21")
                {
                    //check if off hand slot is empty
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                    {
                        //if so, assign item to off hand slot, assign main hand item to inventory, and clear main hand
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = null;
                    }
                    else
                    {
                        //check if there's open room in inventory
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
                        {
                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == "")
                            {
                                //otherwise, assign item to off hand, off hand to inventory, main hand to inventory and clear main hand
                                temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = null;
                            }
                            else
                            {
                                failMessage.SetActive(true);
                                //Debug.Log("Recognized as a dwarf");
                            }
                        }
                        else
                        {
                            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == "")
                            {
                                //otherwise, assign item to off hand, off hand to inventory, main hand to inventory and clear main hand
                                temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = null;
                            }
                            else
                            {
                                failMessage.SetActive(true);
                                //Debug.Log("Definitely not recognized as a dwarf");
                            }
                        }
                    }
                }
                else
                {
                    //check if off hand slot is empty
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                    {
                        //if so, assign item to off hand slot, and remove from inventory
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                    }
                    else
                    {
                        //otherwise, assign item to off hand slot, and off hand slot item to inventory
                        temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                    }
                }
            }
        }
        else
        {
            //check if main hand is two handed
            if (id == "0" || id == "3" || id == "4" || id == "6" || id == "7" || id == "10" || id == "11" || id == "13" || id == "14" || id == "15" || id == "18" || id == "19" || id == "21")
            {
                //check if off hand slot is empty
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                {
                    //if so, assign item to off hand slot, assign main hand item to inventory, and clear main hand
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = null;
                }
                else
                {
                    //check if there's open room in inventory
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
                    {
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == "")
                        {
                            //otherwise, assign item to off hand, off hand to inventory, main hand to inventory and clear main hand
                            temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = null;
                        }
                        else
                        {
                            failMessage.SetActive(true);
                            //Debug.Log("Recognized as a dwarf");
                        }
                    }
                    else
                    {
                        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == "")
                        {
                            //otherwise, assign item to off hand, off hand to inventory, main hand to inventory and clear main hand
                            temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4] = null;
                        }
                        else
                        {
                            failMessage.SetActive(true);
                            //Debug.Log("Definitely not recognized as a dwarf");
                        }
                    }
                }
            }
            else
            {
                //check if off hand slot is empty
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "")
                {
                    //if so, assign item to off hand slot, and remove from inventory
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                }
                else
                {
                    //otherwise, assign item to off hand slot, and off hand slot item to inventory
                    temp = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] = itemID;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = temp;
                }
            }
        }

        

        //finally shift inventory items down
        for (int i = 0; i < 6; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1] = null;
            }
        }
    }

    public void unequip()
    {
        magnifiedCard.SetActive(false);
        //itemID ex: t8
        //slotID 0 - 6
        //playerID 0 - 5

        //check if there's open room in inventory
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Dwarf")
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] == "")
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[6] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[slotID];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[slotID] = null;
            }
            else
            {
                failMessage.SetActive(true);
                //Debug.Log("Recognized as a dwarf");
            }
        }
        else
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] == "")
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[4] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[slotID];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[slotID] = null;
            }
            else
            {
                failMessage.SetActive(true);
                //Debug.Log("Definitely not recognized as a dwarf");
            }
        }

        //finally shift inventory items down *special shift*
        for (int k = 0; k < 7; k++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
                {
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1];
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1] = null;
                }
            }
        }
    }

    public void use()
    {
        magnifiedCard.SetActive(false);
        //implement later

        
        switch (int.Parse(itemID.Substring(1, itemID.Length - 1)))
        {
            case 0: Heal(12); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;
            case 1: Heal(21); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;
            case 2: Heal(30); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;
            case 3:
                int deadPeople = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                        deadPeople++;
                }
                if (deadPeople > 0)
                {
                    resurrectionScreen.SetActive(true);
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;
                }
                else
                    error2.SetActive(true);
                break;//resurrect screen
            case 4: GainAbilityCharges(1); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;
            case 5: GainAbilityCharges(2); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;
            case 6: GainAbilityCharges(3); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;
            case 7: error.SetActive(true); break;//deal damage
            case 8: teleportScreen.SetActive(true); players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null; break;//teleport
            case 9: error.SetActive(true); break;//gain +5 strength
            case 10: error.SetActive(true); break;//gain +5 dexterity
            case 11: error.SetActive(true); break;//gain +5 intellect
        }

        //finally shift inventory items down
        for (int i = 0; i < 6; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1] = null;
            }
        }
    }

    public void Heal(int amount)
    {
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

    public void discard()
    {
        magnifiedCard.SetActive(false);
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[slotID] = null;

        //finally shift inventory items down
        for (int i = 0; i < 6; i++)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == null || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] == "")
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i] = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1];
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Inventory[i + 1] = null;
            }
        }
    }
}
