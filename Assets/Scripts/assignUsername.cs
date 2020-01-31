using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class assignUsername : MonoBehaviour
{
    public GameObject error;
    // Start is called before the first frame update
    void Start()
    {
        error.SetActive(false);

        string fileName = "playerInfo";
        if (File.Exists(fileName))
        {
            var sr = File.OpenText(fileName);
            var line = sr.ReadLine();
            List<string> data = new List<string>();

            while (line != null)
            {
                data.Add(line);
                line = sr.ReadLine();
            }
            sr.Close();

            if (data.Count > 1)
            {
                gameObject.GetComponent<Text>().text = data[0];
            }
            else
            {
                error.SetActive(true);
            }
        }
        else
        {
            error.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
