using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class imageController : MonoBehaviour
{
    public GameObject selector;
    public GameObject mainPortrait;
    public GameObject[] portraits;
    public Sprite[] humans;
    public Sprite[] highElves;
    public Sprite[] dwarves;
    public Sprite[] orcs;
    public Sprite[] centaurs;
    public Sprite[] fairies;
    public Sprite[] nightElves;
    public Sprite[] halflings;
    public Sprite[] djinnis;

    // Start is called before the first frame update
    void Start()
    {
        string fileName = "playerInfo";
        List<string> data = new List<string>();
        var read = File.OpenText(fileName);
        for (int i = 0; i < 7; i++)
        {
            data.Add(read.ReadLine());
        }
        read.Close();

        for (int i = 0; i < 4; i++)
        {
            if (data[2] == "Human")
                portraits[i].GetComponent<Image>().sprite = humans[i];
            else if (data[2] == "High Elf")
                portraits[i].GetComponent<Image>().sprite = highElves[i];
            else if (data[2] == "Dwarf")
                portraits[i].GetComponent<Image>().sprite = dwarves[i];
            else if (data[2] == "Orc")
                portraits[i].GetComponent<Image>().sprite = orcs[i];
            else if (data[2] == "Centaur")
                portraits[i].GetComponent<Image>().sprite = centaurs[i];
            else if (data[2] == "Fairy")
                portraits[i].GetComponent<Image>().sprite = fairies[i];
            else if (data[2] == "Night Elf")
                portraits[i].GetComponent<Image>().sprite = nightElves[i];
            else if (data[2] == "Halfling")
                portraits[i].GetComponent<Image>().sprite = halflings[i];
            else if (data[2] == "Djinni")
                portraits[i].GetComponent<Image>().sprite = djinnis[i];
        }
        mainPortrait.GetComponent<Image>().sprite = portraits[0].GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectPortrait1()
    {
        selector.transform.localPosition = new Vector2(-90, 0);
        mainPortrait.GetComponent<Image>().sprite = portraits[0].GetComponent<Image>().sprite;
    }

    public void selectPortrait2()
    {
        selector.transform.localPosition = new Vector2(-30, 0);
        mainPortrait.GetComponent<Image>().sprite = portraits[1].GetComponent<Image>().sprite;
    }

    public void selectPortrait3()
    {
        selector.transform.localPosition = new Vector2(30, 0);
        mainPortrait.GetComponent<Image>().sprite = portraits[2].GetComponent<Image>().sprite;
    }

    public void selectPortrait4()
    {
        selector.transform.localPosition = new Vector2(90, 0);
        mainPortrait.GetComponent<Image>().sprite = portraits[3].GetComponent<Image>().sprite;
    }
}
