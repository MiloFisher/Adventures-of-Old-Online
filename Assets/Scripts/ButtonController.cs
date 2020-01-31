using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public int playerAmount = 3;
    public GameObject rightArrow;
    public GameObject leftArrow;
    public GameObject next;
    public GameObject join;
    public GameObject playerAmountDisplay;
    public GameObject username;
    // Start is called before the first frame update
    void Start()
    {
        next.SetActive(false);
        join.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAmount == 3)
            leftArrow.SetActive(false);
        else if (leftArrow.activeInHierarchy == false)
            leftArrow.SetActive(true);
        if(playerAmount == 6)
            rightArrow.SetActive(false);
        else if (rightArrow.activeInHierarchy == false)
            rightArrow.SetActive(true);
        if (username.GetComponent<Text>().text != "" && next.activeInHierarchy == false)
        {
            next.SetActive(true);
            join.SetActive(true);
        }
        else if (username.GetComponent<Text>().text == "" && next.activeInHierarchy == true)
        {
            next.SetActive(false);
            join.SetActive(false);
        }
    }

    public void incrementPlayerAmount()
    {
        playerAmount++;
		playerAmountDisplay.GetComponent<Text>().text = playerAmount + "";
    }

    public void decrementPlayerAmount()
    {
        playerAmount--;
        playerAmountDisplay.GetComponent<Text>().text = playerAmount + "";
    }
}
