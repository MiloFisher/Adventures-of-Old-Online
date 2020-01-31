using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statsController : MonoBehaviour
{
    public GameObject stat1;
    public GameObject stat2;
    public GameObject stat3;
    public GameObject generatorButton;
    public GameObject Str;
    public GameObject Dex;
    public GameObject Int;
    public GameObject setStr;
    public GameObject setDex;
    public GameObject setInt;

    private int num1 = 0;
    private int num2 = 0;
    private int num3 = 0;

    private string placeholder1;
    private string placeholder2;

    private List<string> options = new List<string>();
    private List<string> strOptions = new List<string>();
    private List<string> dexOptions = new List<string>();
    private List<string> intOptions = new List<string>();
    private int allowOptions = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allowOptions == 1)
        {
            Str.GetComponent<Dropdown>().ClearOptions();
            Dex.GetComponent<Dropdown>().ClearOptions();
            Int.GetComponent<Dropdown>().ClearOptions();
            options.Add("-");
            options.Add(num1 + "");
            options.Add(num2 + " ");
            options.Add(num3 + "  ");
            for (int i = 0; i < options.Count; i++)
            {
                strOptions.Add(options[i]);
                dexOptions.Add(options[i]);
                intOptions.Add(options[i]);
            }
            Str.GetComponent<Dropdown>().AddOptions(strOptions);
            Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
            Int.GetComponent<Dropdown>().AddOptions(intOptions);
            allowOptions = 2;
        }
        
        //num1 chunk
        if (Str.GetComponent<Dropdown>().options[Str.GetComponent<Dropdown>().value].text == num1 + "" || Dex.GetComponent<Dropdown>().options[Dex.GetComponent<Dropdown>().value].text == num1 + "" || Int.GetComponent<Dropdown>().options[Int.GetComponent<Dropdown>().value].text == num1 + "")
        {
                if (Str.activeInHierarchy && Str.GetComponent<Dropdown>().options[Str.GetComponent<Dropdown>().value].text == num1 + "")
                {
                    Dex.GetComponent<Dropdown>().ClearOptions();
                    Int.GetComponent<Dropdown>().ClearOptions();
                    dexOptions.Remove(num1 + "");
                    intOptions.Remove(num1 + "");
                    Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
                    Int.GetComponent<Dropdown>().AddOptions(intOptions);

                    setStr.SetActive(true);
                    setStr.GetComponentInChildren<Text>().text = num1 + "";
                    Str.SetActive(false);
                }
                else if (Dex.activeInHierarchy && Dex.GetComponent<Dropdown>().options[Dex.GetComponent<Dropdown>().value].text == num1 + "")
                {
                    Str.GetComponent<Dropdown>().ClearOptions();
                    Int.GetComponent<Dropdown>().ClearOptions();
                    strOptions.Remove(num1 + "");
                    intOptions.Remove(num1 + "");
                    Str.GetComponent<Dropdown>().AddOptions(strOptions);
                    Int.GetComponent<Dropdown>().AddOptions(intOptions);

                    setDex.SetActive(true);
                    setDex.GetComponentInChildren<Text>().text = num1 + "";
                    Dex.SetActive(false);
                }
                else if (Int.activeInHierarchy && Int.GetComponent<Dropdown>().options[Int.GetComponent<Dropdown>().value].text == num1 + "")
                {
                    Dex.GetComponent<Dropdown>().ClearOptions();
                    Str.GetComponent<Dropdown>().ClearOptions();
                    dexOptions.Remove(num1 + "");
                    strOptions.Remove(num1 + "");
                    Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
                    Str.GetComponent<Dropdown>().AddOptions(strOptions);

                    setInt.SetActive(true);
                    setInt.GetComponentInChildren<Text>().text = num1 + "";
                    Int.SetActive(false);
                }
        }

        //num2 chunk
        if (Str.GetComponent<Dropdown>().options[Str.GetComponent<Dropdown>().value].text == num2 + " " || Dex.GetComponent<Dropdown>().options[Dex.GetComponent<Dropdown>().value].text == num2 + " " || Int.GetComponent<Dropdown>().options[Int.GetComponent<Dropdown>().value].text == num2 + " ")
        {
                if (Str.activeInHierarchy && Str.GetComponent<Dropdown>().options[Str.GetComponent<Dropdown>().value].text == num2 + " ")
                {
                    Dex.GetComponent<Dropdown>().ClearOptions();
                    Int.GetComponent<Dropdown>().ClearOptions();
                    dexOptions.Remove(num2 + " ");
                    intOptions.Remove(num2 + " ");
                    Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
                    Int.GetComponent<Dropdown>().AddOptions(intOptions);

                    setStr.SetActive(true);
                    setStr.GetComponentInChildren<Text>().text = num2 + "";
                    Str.SetActive(false);
                }
                else if (Dex.activeInHierarchy && Dex.GetComponent<Dropdown>().options[Dex.GetComponent<Dropdown>().value].text == num2 + " ")
                {
                    Str.GetComponent<Dropdown>().ClearOptions();
                    Int.GetComponent<Dropdown>().ClearOptions();
                    strOptions.Remove(num2 + " ");
                    intOptions.Remove(num2 + " ");
                    Str.GetComponent<Dropdown>().AddOptions(strOptions);
                    Int.GetComponent<Dropdown>().AddOptions(intOptions);

                    setDex.SetActive(true);
                    setDex.GetComponentInChildren<Text>().text = num2 + "";
                    Dex.SetActive(false);
                }
                else if (Int.activeInHierarchy && Int.GetComponent<Dropdown>().options[Int.GetComponent<Dropdown>().value].text == num2 + " ")
                {
                    Dex.GetComponent<Dropdown>().ClearOptions();
                    Str.GetComponent<Dropdown>().ClearOptions();
                    dexOptions.Remove(num2 + " ");
                    strOptions.Remove(num2 + " ");
                    Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
                    Str.GetComponent<Dropdown>().AddOptions(strOptions);

                    setInt.SetActive(true);
                    setInt.GetComponentInChildren<Text>().text = num2 + "";
                    Int.SetActive(false);
                }
        }

        //num3 chunk
        if (Str.GetComponent<Dropdown>().options[Str.GetComponent<Dropdown>().value].text == num3 + "  " || Dex.GetComponent<Dropdown>().options[Dex.GetComponent<Dropdown>().value].text == num3 + "  " || Int.GetComponent<Dropdown>().options[Int.GetComponent<Dropdown>().value].text == num3 + "  ")
        {

                if (Str.activeInHierarchy && Str.GetComponent<Dropdown>().options[Str.GetComponent<Dropdown>().value].text == num3 + "  ")
                {
                    Dex.GetComponent<Dropdown>().ClearOptions();
                    Int.GetComponent<Dropdown>().ClearOptions();
                    dexOptions.Remove(num3 + "  ");
                    intOptions.Remove(num3 + "  ");
                    Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
                    Int.GetComponent<Dropdown>().AddOptions(intOptions);

                    setStr.SetActive(true);
                    setStr.GetComponentInChildren<Text>().text = num3 + "";
                    Str.SetActive(false);
                }
                else if (Dex.activeInHierarchy && Dex.GetComponent<Dropdown>().options[Dex.GetComponent<Dropdown>().value].text == num3 + "  ")
                {
                    Str.GetComponent<Dropdown>().ClearOptions();
                    Int.GetComponent<Dropdown>().ClearOptions();
                    strOptions.Remove(num3 + "  ");
                    intOptions.Remove(num3 + "  ");
                    Str.GetComponent<Dropdown>().AddOptions(strOptions);
                    Int.GetComponent<Dropdown>().AddOptions(intOptions);

                    setDex.SetActive(true);
                    setDex.GetComponentInChildren<Text>().text = num3 + "";
                    Dex.SetActive(false);
                }
                else if (Int.activeInHierarchy && Int.GetComponent<Dropdown>().options[Int.GetComponent<Dropdown>().value].text == num3 + "  ")
                {
                    Dex.GetComponent<Dropdown>().ClearOptions();
                    Str.GetComponent<Dropdown>().ClearOptions();
                    dexOptions.Remove(num3 + "  ");
                    strOptions.Remove(num3 + "  ");
                    Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
                    Str.GetComponent<Dropdown>().AddOptions(strOptions);

                    setInt.SetActive(true);
                    setInt.GetComponentInChildren<Text>().text = num3 + "";
                    Int.SetActive(false);
                }
        }

    }//end update()

    public void rollForStats()
    {

        do
        {
            num1 = Random.Range(2, 13);
            num2 = Random.Range(2, 13);
            num3 = Random.Range(2, 13);
        } while (num1 + num2 + num3 < 18);
        stat1.GetComponent<Text>().text = num1 + "";
        stat2.GetComponent<Text>().text = num2 + "";
        stat3.GetComponent<Text>().text = num3 + "";
        generatorButton.SetActive(false);
        allowOptions = 1;
    }

    public void undo()
    {
        if (allowOptions > 0)
        {
            setStr.SetActive(false);
            setDex.SetActive(false);
            setInt.SetActive(false);
            Str.SetActive(true);
            Dex.SetActive(true);
            Int.SetActive(true);
            Str.GetComponent<Dropdown>().ClearOptions();
            Dex.GetComponent<Dropdown>().ClearOptions();
            Int.GetComponent<Dropdown>().ClearOptions();
            for (int i = 0; i < strOptions.Count; i++)
            {
                strOptions.Remove(strOptions[i]);
                i--;
            }
            for (int i = 0; i < dexOptions.Count; i++)
            {
                dexOptions.Remove(dexOptions[i]);
                i--;
            }
            for (int i = 0; i < intOptions.Count; i++)
            {
                intOptions.Remove(intOptions[i]);
                i--;
            }
            for (int i = 0; i < options.Count; i++)
            {
                strOptions.Add(options[i]);
                dexOptions.Add(options[i]);
                intOptions.Add(options[i]);
            }
            Str.GetComponent<Dropdown>().AddOptions(strOptions);
            Dex.GetComponent<Dropdown>().AddOptions(dexOptions);
            Int.GetComponent<Dropdown>().AddOptions(intOptions);
        }
    }
}
