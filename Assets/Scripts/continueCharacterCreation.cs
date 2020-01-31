using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class continueCharacterCreation : MonoBehaviour
{
    public GameObject name;
    public GameObject setStr;
    public GameObject setDex;
    public GameObject setInt;
    public GameObject nextButton;
    public GameObject warning;

    public GameObject characterName;
    public GameObject raceChoice;
    public GameObject classChoice;
    public GameObject strength;
    public GameObject dexterity;
    public GameObject intellect;
    public GameObject strengthMod;
    public GameObject dexterityMod;
    public GameObject intellectMod;

    // Start is called before the first frame update
    void Start()
    {
        nextButton.SetActive(false);
        warning.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (name.GetComponent<Text>().text != "" && setStr.activeInHierarchy && setDex.activeInHierarchy && setInt.activeInHierarchy)
        {
            if (!nextButton.activeInHierarchy)
                nextButton.SetActive(true);
        }
        else if (nextButton.activeInHierarchy)
        {
            nextButton.SetActive(false);
        }
    }

    public void goOn()
    {
        warning.SetActive(true);
    }

    public void cancelWarning()
    {
        warning.SetActive(false);
    }

    public void loadCharacterCreation2()
    {
        string fileName = "playerInfo";
        List<string> data = new List<string>();
        string sign;
        string mod;
        string value;
        int total;

        var read = File.OpenText(fileName);
        //var line = read.ReadLine();
        //while (line != null)
        //{
        //    data.Add(line);
        //    line = read.ReadLine();
        //}
        data.Add(read.ReadLine());//username only
        read.Close();

        var write = File.CreateText(fileName);
        for (int i = 0; i < data.Count; i++)
        {
            write.WriteLine(data[i]);
        }
        write.WriteLine(characterName.GetComponent<Text>().text);
        write.WriteLine(raceChoice.GetComponent<Dropdown>().options[raceChoice.GetComponent<Dropdown>().value].text);
        write.WriteLine(classChoice.GetComponent<Dropdown>().options[classChoice.GetComponent<Dropdown>().value].text);

        //strength
        sign = strengthMod.GetComponent<Text>().text.Substring(0, 1);
        mod = strengthMod.GetComponent<Text>().text.Substring(1, 1);
        value = strength.GetComponent<Text>().text;
        if (sign == "+")
        {
            total = int.Parse(value) + int.Parse(mod);
            if (total > 16)
                total = 16;   
        }
        else
        {
            total = int.Parse(value) - int.Parse(mod);
            if (total < 1)
                total = 1;
        }
        write.WriteLine(total);

        //dexterity
        sign = dexterityMod.GetComponent<Text>().text.Substring(0, 1);
        mod = dexterityMod.GetComponent<Text>().text.Substring(1, 1);
        value = dexterity.GetComponent<Text>().text;
        if (sign == "+")
        {
            total = int.Parse(value) + int.Parse(mod);
            if (total > 16)
                total = 16;
        }
        else
        {
            total = int.Parse(value) - int.Parse(mod);
            if (total < 1)
                total = 1;
        }
        write.WriteLine(total);

        //intellect
        sign = intellectMod.GetComponent<Text>().text.Substring(0, 1);
        mod = intellectMod.GetComponent<Text>().text.Substring(1, 1);
        value = intellect.GetComponent<Text>().text;
        if (sign == "+")
        {
            total = int.Parse(value) + int.Parse(mod);
            if (total > 16)
                total = 16;
        }
        else
        {
            total = int.Parse(value) - int.Parse(mod);
            if (total < 1)
                total = 1;
        }
        write.WriteLine(total);

        write.Close();
        SceneManager.LoadScene("Character Creation 2");
    }
}
