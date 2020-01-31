using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMap : MonoBehaviour
{
    public GameObject map;

    void Update()
    {
        gameObject.transform.position = map.transform.position;
    }
}
