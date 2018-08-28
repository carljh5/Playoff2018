using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject + ", " + collision.gameObject.tag);
        
    }
}
