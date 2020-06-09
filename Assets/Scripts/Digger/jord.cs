using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jord : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "spade")
            Debug.Log("Hit");
        {
            Destroy(gameObject);
        }
    }
}
