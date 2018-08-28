using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public KeyCode Left, Right;
    public float TopSpeed;
    public float SecsUntillTopSpeed;
    public AnimationCurve AccelarationCurve;
    private float currentSpeed;
    private bool moveDirectionLeft;
    private float StartMoveTime;


    private void Update()
    {
        if(Input.GetKeyDown(Left))
        {
            moveDirectionLeft = true;
            StartMoveTime = Time.time;
            Debug.Log("Going left;");
        }
        else if (Input.GetKeyDown(Right))
        {
            moveDirectionLeft = false;
            StartMoveTime = Time.time;

            Debug.Log("Going Right!!");
        }

        if (Input.GetKey(Left) || Input.GetKey(Right))
        {
            currentSpeed = AccelarationCurve.Evaluate(Mathf.Clamp((Time.time - StartMoveTime) / SecsUntillTopSpeed, 0, 1)) * TopSpeed;

            currentSpeed *= moveDirectionLeft ? -1 : 1;

            Debug.Log(currentSpeed);

            gameObject.transform.Translate(new Vector3(currentSpeed, 0));
        }
    }
    
}
