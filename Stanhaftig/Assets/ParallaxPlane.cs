using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxPlane : MonoBehaviour {
    private Parallax parallax;
    private Vector3 OrigPos;
   // private Vector3 Offset;
    private int layerModifier = 0;
	// Use this for initialization
	void Start () {
        OrigPos = transform.position;
        GetRelations(transform.parent);
        //Offset = parallax.targetToFollow - OrigPos;
	}

    private void GetRelations(Transform parent) {
        if (parent == null) {
            Debug.LogError("Child has no parent with a Parallax component");
            return;
        }

        layerModifier++;
        parallax = parent.GetComponent<Parallax>();
        if (parallax == null)
            GetRelations(parent.parent);
    }
	
	// Update is called once per frame
	void Update () {
        if (!parallax.isReady)
            return;
        Vector3 target = OrigPos + (parallax.DeltaDistance() * (parallax.speed / layerModifier));
        Vector3 curVelocity = Vector3.zero;
        OrigPos = Vector3.SmoothDamp(OrigPos, target, ref curVelocity, 0.1f);
        transform.position = OrigPos;

	}
}
