using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class raceController : MonoBehaviour
{
    public GameObject passiveName;
    public GameObject passiveDesc;
    public GameObject statChanges;
    public GameObject strengthMod;
    public GameObject dexterityMod;
    public GameObject intellectMod;
    private string race;
    // Start is called before the first frame update
    void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Dropdown>().value == 0 && race != "human")
        {
            race = "human";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 1 && race != "high_elf")
        {
            race = "high_elf";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 2 && race != "dwarf")
        {
            race = "dwarf";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 3 && race != "orc")
        {
            race = "orc";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 4 && race != "centaur")
        {
            race = "centaur";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 5 && race != "fairy")
        {
            race = "fairy";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 6 && race != "night_elf")
        {
            race = "night_elf";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 7 && race != "halfling")
        {
            race = "halfling";
            updateInfo();
        }
        else if (gameObject.GetComponent<Dropdown>().value == 8 && race != "djinni")
        {
            race = "djinni";
            updateInfo();
        }
    }

    void updateInfo()
    {
        TextAsset textfile = (TextAsset)Resources.Load("Race Info/" + race + "_info");
        List<string> data = new List<string>(textfile.text.Split('\n'));

		passiveName.GetComponent<Text>().text = data[0];
		passiveDesc.GetComponent<Text>().text = data[1];
		if (race == "high_elf")
		{
			strengthMod.GetComponent<Text>().text = data[2].Substring(0,1) + (int.Parse(data[2]) + 1) + "";
			dexterityMod.GetComponent<Text>().text = data[3].Substring(0, 1) + (int.Parse(data[3]) + 1) + "";
			intellectMod.GetComponent<Text>().text = data[4].Substring(0, 1) + (int.Parse(data[4]) + 1) + "";
		}
		else
		{
			strengthMod.GetComponent<Text>().text = data[2];
			dexterityMod.GetComponent<Text>().text = data[3];
			intellectMod.GetComponent<Text>().text = data[4];
		}

		string statChangeLog = "";
		string strSign = data[2].Substring(0, 1);
		string dexSign = data[3].Substring(0, 1);
		string intSign = data[4].Substring(0, 1);
        string healthSign = data[5].Substring(0, 1);
		int components = 0;

		if (data[2] == "+0" && data[3] == "+0" && data[4] == "+0")
			statChangeLog = "(none)";
		else
		{
			if (strSign == "+" && data[2] != "+0")
			{
				statChangeLog += data[2] + " Strength, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (dexSign == "+" && data[3] != "+0")
			{
				statChangeLog += data[3] + " Dexterity, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (intSign == "+" && data[4] != "+0")
			{ 
			    statChangeLog += data[4] + " Intellect, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (healthSign == "+" && data[5] != "+0")
			{
				statChangeLog += data[5] + " Health, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (strSign == "-")
			{
				statChangeLog += data[2] + " Strength, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (dexSign == "-")
			{
				statChangeLog += data[3] + " Dexterity, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (intSign == "-")
			{
				statChangeLog += data[4] + " Intellect, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}
			if (healthSign == "-")
			{
				statChangeLog += data[5] + " Health, ";
				components++;
				if (components == 2)
				{
					components = 0;
					statChangeLog += "\n";
				}
			}

			if (components == 0)
			    statChangeLog = statChangeLog.Substring(0, statChangeLog.Length - 3);
            else
				statChangeLog = statChangeLog.Substring(0, statChangeLog.Length - 2);
		}
		statChanges.GetComponent<Text>().text = statChangeLog;
    }
}
