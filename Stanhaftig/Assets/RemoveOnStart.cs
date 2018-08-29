using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnStart : MonoBehaviour {
    public float Delay;
    private bool Deleting = false;
    public CanvasGroup GroupToFade;
 

    private IEnumerator DeleteAfterSeconds()
    {
        Debug.Log("Fading logo");

        var start = Time.time;
        var t = Time.time + Delay;
        
        while (Time.time <= t)
        {
            yield return null;
            GroupToFade.alpha = Mathf.Lerp(0, 1, (t- Time.time)/Delay);
        }

        Destroy(gameObject);

    }

	// Update is called once per frame
	void FixedUpdate () {
		if(!Deleting && GameManager.GameStarted() )
        {
            Debug.Log("Game Started;");

            Deleting = true;

            StartCoroutine(DeleteAfterSeconds());
        }

	}
}
