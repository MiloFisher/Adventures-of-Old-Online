using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class healthController : MonoBehaviour
{
    public GameObject health;
    public GameObject healthTotal;
    public GameObject healthMod;
    public GameObject clss;
    public GameObject generatorButton;
    public GameObject nextButton;
    private int hp;
    // Start is called before the first frame update
    void Start()
    {
        nextButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void rollForHealth()
    {
        TextAsset textfile = (TextAsset)Resources.Load("Class Info/" + clss.GetComponent<Text>().text.ToLower() + "_info");
        List<string> data = new List<string>(textfile.text.Split('\n'));

        for (int i = 0; i < int.Parse(data[2]); i++)
        {
            hp += Random.Range(1, 7);
        }

        healthTotal.GetComponent<Text>().text = hp + "";
        health.GetComponent<Text>().text = (hp + int.Parse(healthMod.GetComponent<Text>().text)) + "";//int.Parse(healthMod.GetComponent<Text>().text.Substring(1, healthMod.GetComponent<Text>().text.Length-1))) + "";
        generatorButton.SetActive(false);
        nextButton.SetActive(true);
    }
}
