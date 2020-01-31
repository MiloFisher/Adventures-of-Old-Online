using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCamera : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y,-10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!(gameObject.transform.position.x == target.transform.position.x && gameObject.transform.position.y == target.transform.position.y))
            gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y,-10);
    }
}
