using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resurrectRollController : MonoBehaviour
{
    public GameObject rollNumber;
    public GameObject rollButton;
    public GameObject resurrectionWindow;

    void OnEnable()
    {
        rollButton.SetActive(true);
        rollNumber.GetComponent<Text>().text = "";
    }

    public void Roll()
    {
        rollButton.SetActive(false);
        int rando = Random.Range(1, 7);
        rollNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(ProcessLag(rando));
    }

    IEnumerator ProcessLag(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll >= 4)
            resurrectionWindow.SetActive(true);
        gameObject.SetActive(false);
    }

}
