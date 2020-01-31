using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcImageController : MonoBehaviour
{
	public GameObject[] images;
    public GameObject name;
    private string npc;

    void Update()
    {
        npc = name.GetComponent<Text>().text;
        if (npc == "Castle Guard")
            images[0].SetActive(true);
        else
            images[0].SetActive(false);
        if (npc == "Lost Adventurer")
            images[1].SetActive(true);
        else
            images[1].SetActive(false);
        if (npc == "Elven Hunter")
            images[2].SetActive(true);
        else
            images[2].SetActive(false);
        if (npc == "Ominous Cat")
            images[3].SetActive(true);
        else
            images[3].SetActive(false);
        if (npc == "Lucky Fisherman")
            images[4].SetActive(true);
        else
            images[4].SetActive(false);
        if (npc == "Charming Bard")
            images[5].SetActive(true);
        else
            images[5].SetActive(false);
        if (npc == "Sleepy Mermaid")
            images[6].SetActive(true);
        else
            images[6].SetActive(false);
    }
}
