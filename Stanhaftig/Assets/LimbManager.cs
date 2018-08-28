﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LimbManager : MonoBehaviour
{
    public GameObject bloodParticlesPrefab;

    [Serializable]
	public class Limb {
        public string name;
        public HingeJoint2D parentJoint;
	}

    public List<Limb> limbs = new List<Limb>();

    public void DetachLimb(Limb limb) {
        //spawn blood particles and child them to the connected rigidbody of the joint at the joints position;
        Instantiate(bloodParticlesPrefab, limb.parentJoint.transform.position, Quaternion.identity, limb.parentJoint.connectedBody.transform);

        if(limb.name.Equals("Head")) {
            //set the head's rigidbodys freeze y position to false
            limb.parentJoint.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            //set the body's (the limbs connected rigidbody) rigidbodys freeze y position to false
            limb.parentJoint.connectedBody.constraints = RigidbodyConstraints2D.None;
            //Change movement to affect the head
            gameObject.GetComponent<Movement>().PhysicsBody = limb.parentJoint.gameObject.GetComponent<Rigidbody2D>();
        }
        //detach by deactivating hingejoint
        limb.parentJoint.enabled = false;
        //remove the limb from list to cycle
        limbs.Remove(limb);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            DetachLimb(limbs[0]);
	}
}
