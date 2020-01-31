using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class joinWaitingRoom : MonoBehaviour
{
    public GameObject confirmation;
    public GameObject health;
    public GameObject image;

    // Start is called before the first frame update
    void Start()
    {
        confirmation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void back()
    {
        confirmation.SetActive(false);
    }

    public void openConfirmation()
    {
        confirmation.SetActive(true);
    }

    public void goOn()
    {
        string fileName = "playerInfo";
        List<string> data = new List<string>();

        var read = File.OpenText(fileName);
        for (int i = 0; i < 7; i++)
        {
            data.Add(read.ReadLine());
        }
        read.Close();

        TextAsset textfile = (TextAsset)Resources.Load("Class Info/" + data[3].ToLower() + "_info", typeof(TextAsset));
        List<string> classInfo = new List<string>(textfile.text.Split('\n'));

        var write = File.CreateText(fileName);
        for (int i = 0; i < data.Count; i++)
        {
            write.WriteLine(data[i]);
        }
        write.WriteLine(health.GetComponent<Text>().text);//health - 7
        write.WriteLine(health.GetComponent<Text>().text);//max health - 8
        write.WriteLine("1");//level
        write.WriteLine("1");//damage
        write.WriteLine("0");//physical power
        write.WriteLine("0");//spell power
        write.WriteLine("0");//damage reduction
        write.WriteLine("0");//gold
        write.WriteLine(image.GetComponent<Image>().sprite.name);//image
        write.WriteLine(classInfo[1]);
        write.WriteLine("0");//strength mod
        write.WriteLine("0");//dexterity mod
        write.WriteLine("0");//intellect mod
        write.WriteLine("0");//damage mod
        write.Close();



        SceneManager.LoadScene("Waiting Room");
    }
}
