using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statBarController : MonoBehaviour
{
    public GameObject statAmount;
    public Sprite image;
    public Sprite empty;
    public Component[] bars;
    // Start is called before the first frame update
    void Start()
    {
        bars = gameObject.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (int.Parse(statAmount.GetComponent<Text>().text) <= 16)
        {
            for (int i = 0; i < int.Parse(statAmount.GetComponent<Text>().text); i++)
            {
                bars[i].GetComponent<Image>().sprite = image;
            }
            for (int i = int.Parse(statAmount.GetComponent<Text>().text); i < 16; i++)
            {
                bars[i].GetComponent<Image>().sprite = empty;
            }
        }
        else
        {
            for (int i = 0; i < 16; i++)
            {
                bars[i].GetComponent<Image>().sprite = image;
            }
        }
    }
}
