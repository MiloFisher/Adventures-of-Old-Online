using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuShortcutsController : MonoBehaviour
{
    public GameObject menuShortcuts;
    public GameObject arrowDisplay;
    public Sprite rightArrow;
    public Sprite leftArrow;
    // Start is called before the first frame update
    void Start()
    {
        menuShortcuts.SetActive(false);
    }

    public void toggleMenuShortcuts()
    {
        if (menuShortcuts.activeInHierarchy == true)
        {
            menuShortcuts.SetActive(false);
            arrowDisplay.GetComponent<Image>().sprite = rightArrow;
        }
        else
        {
            menuShortcuts.SetActive(true);
            arrowDisplay.GetComponent<Image>().sprite = leftArrow;
        }
    }
}
