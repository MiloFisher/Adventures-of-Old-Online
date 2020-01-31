using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionManager : MonoBehaviour
{
    public GameObject[] options;
    private int location;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        location = -115;
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].activeInHierarchy)
            {
                options[i].transform.localPosition = new Vector2(0,location);
                location -= 30;
            }
        }
    }
}
