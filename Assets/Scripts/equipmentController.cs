using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equipmentController : MonoBehaviour
{
    public string characterName;
    public GameObject[] equipment;
    private GameObject[] players;
    private int activeTotal;
    private int iteration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        activeTotal = 0;
        for (int i = 0; i < equipment.Length; i++)
        {
            if (equipment[i].activeInHierarchy)
            {
                activeTotal++;
            }
        }
        iteration = 0;
        if (activeTotal == 1)
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].activeInHierarchy)
                {
                    equipment[i].transform.localPosition = new Vector2(0, 0);
                }
            }
        }
        else if (activeTotal == 2)
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].activeInHierarchy)
                {
                    if(iteration == 0)
                        equipment[i].transform.localPosition = new Vector2(-41.25f, 0);
                    else if (iteration == 1)
                        equipment[i].transform.localPosition = new Vector2(41.25f, 0);
                    iteration++;
                }
            }   
        }
        else if (activeTotal == 3)
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].activeInHierarchy)
                {
                    if (iteration == 0)
                        equipment[i].transform.localPosition = new Vector2(-82.5f, 0);
                    else if (iteration == 1)
                        equipment[i].transform.localPosition = new Vector2(0, 0);
                    else if (iteration == 2)
                        equipment[i].transform.localPosition = new Vector2(82.5f, 0);
                    iteration++;
                }
            }  
        }
        else if (activeTotal == 4)
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].activeInHierarchy)
                {
                    if (iteration == 0)
                        equipment[i].transform.localPosition = new Vector2(-123.75f, 0);
                    else if (iteration == 1)
                        equipment[i].transform.localPosition = new Vector2(-41.25f, 0);
                    else if (iteration == 2)
                        equipment[i].transform.localPosition = new Vector2(41.25f, 0);
                    else if (iteration == 3)
                        equipment[i].transform.localPosition = new Vector2(123.75f, 0);
                    iteration++;
                }
            }  
        }
        else if (activeTotal == 5)
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].activeInHierarchy)
                {
                    if (iteration == 0)
                        equipment[i].transform.localPosition = new Vector2(-165, 0);
                    else if (iteration == 1)
                        equipment[i].transform.localPosition = new Vector2(-82.5f, 0);
                    else if (iteration == 2)
                        equipment[i].transform.localPosition = new Vector2(0, 0);
                    else if (iteration == 3)
                        equipment[i].transform.localPosition = new Vector2(82.5f, 0);
                    else if (iteration == 4)
                        equipment[i].transform.localPosition = new Vector2(165, 0);
                    iteration++;
                }
            }  
        }
        else if (activeTotal == 6)
        {
            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].activeInHierarchy)
                {
                    if (iteration == 0)
                        equipment[i].transform.localPosition = new Vector2(-165, 0);
                    else if (iteration == 1)
                        equipment[i].transform.localPosition = new Vector2(-98.33f, 0);
                    else if (iteration == 2)
                        equipment[i].transform.localPosition = new Vector2(-31.66f, 0);
                    else if (iteration == 3)
                        equipment[i].transform.localPosition = new Vector2(31.66f, 0);
                    else if (iteration == 4)
                        equipment[i].transform.localPosition = new Vector2(98.33f, 0);
                    else if (iteration == 5)
                        equipment[i].transform.localPosition = new Vector2(165, 0);
                    iteration++;
                }
            }   
        }
    }
}
