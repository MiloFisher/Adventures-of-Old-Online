using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyTiles : MonoBehaviour
{
    public bool neutralized = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!neutralized)
            Destroy(gameObject);
        else
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
