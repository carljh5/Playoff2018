using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

    //private Transform[] children

    private Transform p1;
    private Transform p2;

    public bool isReady;
    public float speed = 1f;

    private Vector3 targetToFollow;

    private Vector3 lastPos;

	// Use this for initialization
    void Start () {
        StartCoroutine(WaitForGameStartedRoutine());
	}

    IEnumerator WaitForGameStartedRoutine() {
        yield return null;
        while (!GameManager.GameStarted())
            yield return null;

        p1 = GameManager.GetPlayerHeadTransform(true);
        p2 = GameManager.GetPlayerHeadTransform(false);
        isReady = true;
    }
	
    public Vector3 DeltaDistance() {
        return lastPos - targetToFollow;
    }

	// Update is called once per frame
	void Update () {
        if (isReady)
        {
            lastPos = targetToFollow;
            targetToFollow = (p1.position + p2.position) / 2;
        }
		
	}
}
