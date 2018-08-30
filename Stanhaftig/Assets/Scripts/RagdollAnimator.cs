using System.Collections;
using System.Collections.Generic;
using System;
using Anima2D;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RagdollAnimator : MonoBehaviour
{
    private Animator animator;

    public GameObject limbContainer;
    public GameObject bloodSpatter;

    [Serializable]
    public class Limb {
        public string name = "limb";
        public Bone2D parentBone;
        //public SpriteRenderer[] sprites;
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

        Vector3 parentBonePosition = limb.parentBone.transform.position;
        GameObject limbContainerClone = Instantiate(limbContainer, parentBonePosition, Quaternion.identity);
        limbContainerClone.SetActive(true);
        //Copy limb and set as child to the limbcontainerClone
        GameObject limbClone = Instantiate(limb.parentBone.gameObject, limbContainerClone.transform);

        //Collect springbones
        UnityChan.SpringBone[] springBones = limbContainerClone.GetComponentsInChildren<UnityChan.SpringBone>();

        //Combine with spring manager
        limbContainerClone.GetComponent<UnityChan.SpringManager>().springBones = springBones;

        //Hide existing sprites
        foreach(SpriteRenderer spriteRenderer in limb.parentBone.gameObject.GetComponentsInChildren<SpriteRenderer>()) {
            spriteRenderer.gameObject.SetActive(false);
        }

        //Make blood
        GameObject bloodSpatterClone = Instantiate(bloodSpatter, parentBonePosition, Quaternion.identity, limb.parentBone.transform);

    }
	
	
	void Update () {
        //if (Input.GetKeyDown(KeyCode.A))
        //    Hurt();
        //if (Input.GetKeyDown(KeyCode.B))
        //Hit();
        if (Input.GetKeyDown(KeyCode.C))
            DetachLimb(detachableLimbs[0]);
	}
}
