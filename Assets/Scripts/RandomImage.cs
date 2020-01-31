using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomImage : MonoBehaviour
{
    public Sprite[] sprite;
    // Start is called before the first frame update
    void Start()
    {
        int random = Random.Range(0, sprite.Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite[random];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
