using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class displayStats : MonoBehaviour
{
    public GameObject username;
    public GameObject name;
    public GameObject race;
    public GameObject clss;
    public GameObject damage;
    public GameObject level;
    public GameObject strength;
    public GameObject dexterity;
    public GameObject intellect;
	public GameObject healthMod;

	// Start is called before the first frame update
	void Start()
    {
        string fileName = "playerInfo";
        List<string> data = new List<string>();

        var read = File.OpenText(fileName);
        for(int i = 0; i < 7; i++)
        {
            data.Add(read.ReadLine());
        }
        read.Close();

        username.GetComponent<Text>().text = data[0] + "'s Character";
        name.GetComponent<Text>().text = data[1];
        race.GetComponent<Text>().text = data[2];
        clss.GetComponent<Text>().text = data[3];
        damage.GetComponent<Text>().text = "1";
        level.GetComponent<Text>().text = "1";
        strength.GetComponent<Text>().text = data[4];
        dexterity.GetComponent<Text>().text = data[5];
        intellect.GetComponent<Text>().text = data[6];

        string tempRace = race.GetComponent<Text>().text;
        for (int i = 0; i < tempRace.Length; i++)
        {
            if (tempRace.Substring(i, 1) == " ")
            {
                tempRace = tempRace.Substring(0, i) + "_" + tempRace.Substring(i + 1, tempRace.Length - (i + 1));
                i = tempRace.Length;
            }
        }

        TextAsset textfile = (TextAsset)Resources.Load("Race Info/" + tempRace + "_info");
        List<string> lines = new List<string>(textfile.text.Split('\n'));
        data.Clear();
        for (int i = 0; i < 6; i++)
        {
            data.Add(lines[i]);
        }

        healthMod.GetComponent<Text>().text = data[5];
	}
}
