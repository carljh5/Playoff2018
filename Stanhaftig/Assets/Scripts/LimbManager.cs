using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LimbManager : MonoBehaviour
{
    public GameObject bloodParticlesPrefab;
    public Canvas giveUpCanvas;

    private bool hasDetachedHead;

    [HideInInspector]
    public Movement movement;

    private bool lLegDetached = false;
    private bool rLegDetached = false;

    [Serializable]
	public class Limb {
        public string name;
        public HingeJoint2D parentJoint;
	}

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && GameManager.SpaceKills())
        {
            LoseLimb(null);
        }
    }

    public GameObject equipment;
    private bool isEquipmentDetached;
    public SpriteRenderer spriteToToggleOnOnEquipmentLost;

    public List<Limb> limbs = new List<Limb>();

    public void DetachLimb(Limb limb) {
        if(limb.name.Equals("Head") &!isEquipmentDetached) {
            equipment.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            equipment.GetComponent<CapsuleCollider2D>().enabled = true;
            if(spriteToToggleOnOnEquipmentLost != null)
                spriteToToggleOnOnEquipmentLost.enabled = true;
            isEquipmentDetached = true;
            return;
        }
        //spawn blood particles and child them to the connected rigidbody of the joint at the joints position;
        Instantiate(bloodParticlesPrefab, limb.parentJoint.transform.position, Quaternion.identity, limb.parentJoint.connectedBody.transform);

        //detach by deactivating hingejoint
        limb.parentJoint.enabled = false;

        int[] ints = new int[4];
        
        if (limb.name.Equals("RLeg")) // No more legs
        {
            rLegDetached = true;

            if (lLegDetached)
            {
                giveUpCanvas.gameObject.SetActive(true);
                movement.StartTorsoMovement();
            }
        }
        else if (limb.name.Equals("LLeg")) // No more legs
        {
            lLegDetached = true;

            if (rLegDetached)
            {
                giveUpCanvas.gameObject.SetActive(true);
                movement.StartTorsoMovement();
            }
        }
        else if (limb.name.Equals("LArm"))
        {
            if(!GameManager.LivingSwordsMode())
                movement.SwordMovement = false;
        }
        else if (limb.name.Equals("Head"))
        {
            if (!hasDetachedHead)
            {
                movement.StartHeadMovement();

                //set the head's rigidbodys freeze y position to false
                limb.parentJoint.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                //set the body's (the limbs connected rigidbody) rigidbodys freeze y position to false
                limb.parentJoint.connectedBody.constraints = RigidbodyConstraints2D.None;
                limb.parentJoint.connectedBody.tag = "Untagged";
                limb.parentJoint.connectedBody.mass = 0.5f;

                //Change movement to affect the head
                gameObject.GetComponent<Movement>().PhysicsBody = limb.parentJoint.gameObject.GetComponent<Rigidbody2D>();
                hasDetachedHead = true;



            }
            return;
        }


        foreach (var l in limb.parentJoint.GetComponentsInChildren<Rigidbody2D>())
        {
            l.tag = "Untagged";
            l.mass = 0.5f;
        }

        //remove the limb from list to cycle
        limbs.Remove(limb);
    }


    public void LoseLimb(Limb preferedLimb)
    {
        if (preferedLimb == null || preferedLimb.name.Equals("LArm") || (preferedLimb.name.Equals("Head")&& isEquipmentDetached))
            DetachLimb(limbs[0]); //should we maybe not do anything if it is the head or sword arm?
        else
            DetachLimb(preferedLimb);

    }
	

    public Limb GetLimb(GameObject limb)
    {
        var currentJoint = limb.GetComponent<HingeJoint2D>();

        if (currentJoint == null)
            currentJoint = limb.GetComponentInParent<HingeJoint2D>();

        while(currentJoint != null)
        {
            var l = GetLimbFromJoint(currentJoint);
            if (l != null)
                return l;
            else if (currentJoint.gameObject.transform.parent)
                currentJoint = currentJoint.gameObject.transform.parent.GetComponentInParent<HingeJoint2D>();
            else break;
        }

        Debug.Log("null limb");
        return null;
    }

    private Limb GetLimbFromJoint(HingeJoint2D joint)
    {
        foreach (var l in limbs)
            if (l.parentJoint == joint) return l;

        return null;
    }
}
