using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terminateOnTime : MonoBehaviour {
    
    

	void OnEnable()
    {
        StartCoroutine(waitForSeconds());
    }

    IEnumerator waitForSeconds()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
