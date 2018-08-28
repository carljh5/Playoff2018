using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RagdollAnimator : MonoBehaviour
{
    private Animator animator;

    [Serializable]
    public class Limb {
        public string name = "limb";
        public SpriteRenderer[] sprites;
    }

    public List<Limb> detachableLimbs = new List<Limb>(); 

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    public void Hurt() {
        animator.SetTrigger("Hurt");
    }

    public void Hit() {
        animator.SetTrigger("Hit");
    }

    public void DetachLimb(Limb limb) {


    }
	
	
	void Update () {
        //if (Input.GetKeyDown(KeyCode.A))
        //    Hurt();
        //if (Input.GetKeyDown(KeyCode.B))
            //Hit();
	}
}
