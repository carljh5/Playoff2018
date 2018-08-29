using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    public KeyCode Left, Right,Hit, Jump;

    private Rigidbody2D[] RigidBodies;

    private bool frozen = false;

    public bool HeadMovement { get; private set; }

    public bool TorsoMovement;

    public float Jumping ;
    private float TorsoPositionY, HeadPositionY;
    private bool InTheAir;

    [HideInInspector]
    public bool SwordMovement = true;

    [Header("Movement")]
    [HideInInspector]
    public float TopSpeed;
    [HideInInspector]
    public float SecsUntillTopSpeed;
    [HideInInspector] public AnimationCurve AccelarationCurve;
    private float currentSpeed;
    private bool moveDirectionLeft;
    private float StartMoveTime;
    public Rigidbody2D PhysicsBody;
    public Rigidbody2D HeadBody;

    [Header("Sword")]
    public Rigidbody2D Sword;
    [HideInInspector]
    public float SwordForce;
    public int direction;

    private void Start()
    {
        RigidBodies = GetComponentsInChildren<Rigidbody2D>();

        direction = ((int)transform.localScale.x) * -1;

        TorsoPositionY = PhysicsBody.position.y;
        HeadPositionY = HeadBody.position.y;
    }

    private void Update()
    {
        if(Input.GetKeyDown(Left))
        {
            moveDirectionLeft = true;
            StartMoveTime = Time.time;
        }
        else if (Input.GetKeyDown(Right))
        {
            moveDirectionLeft = false;
            StartMoveTime = Time.time;
        }

        if (!frozen &! InTheAir && (Input.GetKey(Left) || Input.GetKey(Right)))
        {

            currentSpeed = AccelarationCurve.Evaluate(Mathf.Clamp((Time.time - StartMoveTime) / SecsUntillTopSpeed, 0, 1)) * TopSpeed;

            currentSpeed *= moveDirectionLeft ? -1 : 1;

            if (TorsoMovement)
                currentSpeed *= 0.05f;

            var t = PhysicsBody.transform;
            
            PhysicsBody.MovePosition( new Vector2(t.position.x+currentSpeed,t.position.y));
        }

        if(Input.GetKeyDown(Jump) &! frozen &! InTheAir)
        {
            Debug.Log("jump");


            InTheAir = true;

            foreach (var rb in RigidBodies)
            {
                rb.constraints = RigidbodyConstraints2D.None;
            }

            var t = PhysicsBody.transform;
            PhysicsBody.MovePosition(new Vector2(t.position.x + Jumping * (moveDirectionLeft ? -1 : 1), t.position.y + Jumping));

            // reset freeze position when jump is over. Check if jumping
            StartCoroutine(ReFreezeBodyAfterJump());
            StartCoroutine(ReFreezeHeadAfterJump());
        }

        if(Input.GetKeyDown(Hit) && Sword && SwordMovement)
        {
            if(GameManager.Freeze())
            {
                frozen = !frozen;

                foreach(var rb in RigidBodies)
                {

                    rb.freezeRotation = frozen;
                }
            }

            var t = Sword.transform;

            //if sword is up hit down and vice versa
            var x = t.rotation.z > 0 ? SwordForce : -SwordForce;

            Sword.MovePosition(new Vector2(t.position.x+SwordForce*direction , t.position.y + SwordForce));
        }
    }

    IEnumerator ReFreezeHeadAfterJump()
    {
        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(() => HeadBody.position.y <= HeadPositionY && !InTheAir);
        
        HeadBody.constraints = RigidbodyConstraints2D.FreezePositionY;

        Debug.Log("ReFreeze Head");

    }
    IEnumerator ReFreezeBodyAfterJump()
    {
        //Probably gives errors with the changing of the physicsBody to head.

        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(() => PhysicsBody.position.y <= TorsoPositionY);

        if (!HeadMovement)
            PhysicsBody.constraints = RigidbodyConstraints2D.FreezePositionY;

        Debug.Log("No longer in the air");
        InTheAir = false;
    }



    internal void StartHeadMovement()
    {
        HeadMovement = true;
        TorsoMovement = false;
        Jumping = 0.01f;
    }

    internal void StartTorsoMovement()
    {
        Jumping = 0;
        TorsoMovement = true;
        foreach (var rb in RigidBodies)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        //PhysicsBody.constraints = RigidbodyConstraints2D.None;
    }
}
