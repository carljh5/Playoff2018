using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Key mapping")]
    public KeyCode Left, Right,Hit;

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

        if (Input.GetKey(Left) || Input.GetKey(Right))
        {
            currentSpeed = AccelarationCurve.Evaluate(Mathf.Clamp((Time.time - StartMoveTime) / SecsUntillTopSpeed, 0, 1)) * TopSpeed;

            currentSpeed *= moveDirectionLeft ? -1 : 1;

            var t = PhysicsBody.transform;
            
            PhysicsBody.MovePosition( new Vector2(t.position.x+currentSpeed,t.position.y));
        }

        if(Input.GetKeyDown(Hit) && Sword)
        {
            var t = Sword.transform;

            //if sword is up hit down and vice versa
            var x = t.rotation.z > 0 ? SwordForce : -SwordForce;

            Sword.MovePosition(new Vector2(t.position.x+SwordForce*direction , t.position.y + SwordForce));
        }
    }
    
}
