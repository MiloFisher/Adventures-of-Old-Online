using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteSelf : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        //target.SetActive(false);
    }

    public void delete()
    {
        target.SetActive(false);
    }
}
