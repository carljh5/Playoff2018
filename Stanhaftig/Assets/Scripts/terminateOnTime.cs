using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terminateOnTime : MonoBehaviour
{

    public float time;
    public bool destroy;

    void OnEnable()
    {
        StartCoroutine(waitForSeconds());
    }

    IEnumerator waitForSeconds()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        if (destroy)
        {
            Destroy(gameObject);
        }


    }
}
