﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    public KeyCode Left, Right,Hit, Jump, Surrender;

    private Rigidbody2D[] RigidBodies;

    public bool frozen = false;
    private bool freezeAvailable = true;

    private SpriteRenderer[] spriteList;
    private Color[] originalColorList;
    public Color frozenColor;

    public bool HeadMovement;
    [HideInInspector]
    public float freezeTime = 1f;
    public float freezeCoolDown = 1f;

    public bool TorsoMovement;

    public float Jumping ;
    public float JumpTime;
    public AnimationCurve JumpAcceleration;

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


        spriteList = GetComponentsInChildren<SpriteRenderer>();
        originalColorList = new Color[spriteList.Length];

        for (int i = 0; i < spriteList.Length; i++)
        {
            originalColorList[i] = spriteList[i].color;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Surrender))
        {
            if (gameObject.CompareTag("Player"))
            {
                var player1text = TauntManager.GetPlayer1Text();

                player1text.text = "I yield, I yield!";
                player1text.gameObject.SetActive(true);
                SoundManager.PlayCry();
            }
            else
            {
                var player2text = TauntManager.GetPlayer2Text();

                player2text.text = "I yield, I yield!";
                player2text.gameObject.SetActive(true);
                SoundManager.PlayCry();
            }
            TauntManager.instance.StopCoroutine(TauntManager.instance.co);
            SoundManager.PlayWin();
            StartCoroutine(EndScreenIn3());
            return;

        }
    }

    private void FixedUpdate()
    {

        if (InTheAir) //&& PhysicsBody.velocity.y < 0)
        {
            PhysicsBody.velocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.fixedDeltaTime;
        }




        if(Input.GetKey(Left))
        {
            moveDirectionLeft = true;
            StartMoveTime = Time.time;
        }
        else if (Input.GetKey(Right))
        {
            moveDirectionLeft = false;
            StartMoveTime = Time.time;
        }

        if (!frozen &! InTheAir && (Input.GetKey(Left) || Input.GetKey(Right)))
        {

            currentSpeed = TopSpeed;// AccelarationCurve.Evaluate(Mathf.Clamp((Time.time - StartMoveTime) / SecsUntillTopSpeed, 0, 1)) * TopSpeed;

            currentSpeed *= moveDirectionLeft ? -1 : 1;

            if (TorsoMovement)
                currentSpeed *= 0.05f;

            var t = PhysicsBody.transform;
            
            PhysicsBody.velocity += ( new Vector2(currentSpeed,0));
        }

        if(Input.GetKey(Jump) &! frozen &! InTheAir)
        {
            //Debug.Log("jump");


            InTheAir = true;

            foreach (var rb in RigidBodies)
            {
                rb.constraints = RigidbodyConstraints2D.None;
            }

            var jumbBody = HeadMovement ? HeadBody : PhysicsBody;

            StartCoroutine(MoveBodyOverTime(jumbBody, JumpTime));

        }

        if(Input.GetKey(Hit) && Sword && SwordMovement && freezeAvailable)
        {
            if (GameManager.Freeze())
            {

                //if (!frozen)
                //{
                frozen = true;
                freezeAvailable = false;

                foreach (var rb in RigidBodies)
                {
                    if (rb.gameObject.CompareTag("Untagged"))
                        continue;

                    rb.velocity *= 1.7f;

                    rb.freezeRotation = frozen;
                }


                foreach (SpriteRenderer sprite in spriteList)
                {
                    //if (sprite.gameObject.CompareTag("Untagged"))
                    //    continue;

                    sprite.color = frozenColor;
                }


                GameObject particle = Instantiate(GameManager.GetFreezeParticle().gameObject, 
                                                            new Vector3(PhysicsBody.transform.position.x, PhysicsBody.transform.position.y, PhysicsBody.transform.position.z -5), 
                                                            new Quaternion());
                particle.transform.parent = PhysicsBody.gameObject.transform;
                SoundManager.PlayFreeze();

                StartCoroutine(UnfreezeAfterDelay());
                //}
            }
            else
            {
                var t = Sword.transform;

                //if sword is up hit down and vice versa
                var x = t.rotation.z > 0 ? SwordForce : -SwordForce;

                Sword.MovePosition(new Vector2(t.position.x + SwordForce * direction, t.position.y + SwordForce));
            }
        }
    }

    private IEnumerator EndScreenIn3()
    {
        Debug.Log("game Ending");

        yield return new WaitForSeconds(2f);

        SoundManager.PlayLaugh();

        yield return new WaitForSeconds(2f);

        Debug.Log("game done");
        GameManager.Endscreen().SetActive(true);
    }

    private IEnumerator MoveBodyOverTime(Rigidbody2D body, float secs)
    {
        var start = Time.time;

        while(start + secs > Time.time)
        {

            yield return new WaitForFixedUpdate();

            var t = body.transform;

            var xFactor = JumpAcceleration.Evaluate((Time.time-start)/ (secs));

            //Debug.Log(xFactor);

            body.velocity += new Vector2(0.3f * Jumping * (moveDirectionLeft ? -xFactor : xFactor) ,  Jumping * xFactor);
            
        }


        // reset freeze position when jump is over. Check if jumping
        if (!HeadMovement)
            StartCoroutine(ReFreezeBodyAfterJump());
        else
            StartCoroutine(ReFreezeHeadAfterJump());
    }

    private IEnumerator UnfreezeAfterDelay()
    {
        yield return new WaitForSeconds(freezeTime);


        frozen = false;

        foreach (var rb in RigidBodies)
        {

            rb.freezeRotation = frozen;
        }

        for (int i = 0; i < spriteList.Length; i++)
        {
            spriteList[i].color = originalColorList[i];
        }

        yield return new WaitForSeconds(freezeCoolDown);

        freezeAvailable = true;

    }

    IEnumerator ReFreezeHeadAfterJump()
    {
        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(() => HeadBody.position.y <= HeadPositionY && (HeadMovement || !InTheAir));
        
        if(!HeadMovement)
            HeadBody.constraints = RigidbodyConstraints2D.FreezePositionY;

        InTheAir = false;

    }
    IEnumerator ReFreezeBodyAfterJump()
    {
        //Probably gives errors with the changing of the physicsBody to head.

        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(() => PhysicsBody.position.y <= TorsoPositionY);

        if (!HeadMovement)
            PhysicsBody.constraints = RigidbodyConstraints2D.FreezePositionY;

        //Debug.Log("No longer in the air");
        InTheAir = false;
    }



    internal void StartHeadMovement()
    {
        HeadMovement = true;
        TorsoMovement = false;
        TopSpeed *= 0.4f;
        Jumping = 2f;
    }

    internal void StartTorsoMovement()
    {
        Jumping = 5f;
        TorsoMovement = true;
        foreach (var rb in RigidBodies)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        //PhysicsBody.constraints = RigidbodyConstraints2D.None;
    }
}
