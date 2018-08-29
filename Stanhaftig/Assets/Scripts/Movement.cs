using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Key mapping")]
    public KeyCode Left, Right,Hit;

    private Rigidbody2D[] RigidBodies;

    private bool frozen = false;

    public bool TorsoMovement;

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

    [Header("Sword")]
    public Rigidbody2D Sword;
    [HideInInspector]
    public float SwordForce;
    public int direction;

    private void Start()
    {
        RigidBodies = GetComponentsInChildren<Rigidbody2D>();

        direction = ((int)transform.localScale.x) * -1;
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

        if (!frozen && (Input.GetKey(Left) || Input.GetKey(Right)))
        {
            currentSpeed = AccelarationCurve.Evaluate(Mathf.Clamp((Time.time - StartMoveTime) / SecsUntillTopSpeed, 0, 1)) * TopSpeed;

            currentSpeed *= moveDirectionLeft ? -1 : 1;

            if (TorsoMovement)
                currentSpeed *= 0.05f;

            var t = PhysicsBody.transform;
            
            PhysicsBody.MovePosition( new Vector2(t.position.x+currentSpeed,t.position.y));
        }

        if(Input.GetKeyDown(Hit) && Sword)
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

    internal void StartHeadMovement()
    {
        TorsoMovement = false;
    }

    internal void StartTorsoMovement()
    {
        TorsoMovement = true;
        foreach (var rb in RigidBodies)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        //PhysicsBody.constraints = RigidbodyConstraints2D.None;
    }
}
