using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class classController : MonoBehaviour
{
	public GameObject className;
	public GameObject classDesc;
	private string clss;
    // Start is called before the first frame update
    void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (gameObject.GetComponent<Dropdown>().value == 0 && clss != "warrior")
		{
			clss = "warrior";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 1 && clss != "hunter")
		{
			clss = "hunter";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 2 && clss != "sorcerer")
		{
			clss = "sorcerer";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 3 && clss != "paladin")
		{
			clss = "paladin";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 4 && clss != "cleric")
		{
			clss = "cleric";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 5 && clss != "necromancer")
		{
			clss = "necromancer";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 6 && clss != "rogue")
		{
			clss = "rogue";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 7 && clss != "druid")
		{
			clss = "druid";
			updateInfo();
		}
		else if (gameObject.GetComponent<Dropdown>().value == 8 && clss != "monk")
		{
			clss = "monk";
			updateInfo();
		}
	}

	void updateInfo()
	{
        

        TextAsset textfile = (TextAsset)Resources.Load("Class Info/" + clss + "_info", typeof(TextAsset));
        List<string> data = new List<string>(textfile.text.Split('\n'));

        className.GetComponent<Text>().text = clss.Substring(0, 1).ToUpper() + clss.Substring(1,clss.Length-1);
        classDesc.GetComponent<Text>().text = "Primary Stat: " + data[0] + "\nAbility Charges: " + data[1] + "\nHealth Dice: " + data[2] + "\nArmor Proficiency: " + data[3] + "\nWeapon Proficiency: " + data[4];
	}
}
