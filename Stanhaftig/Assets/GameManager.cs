using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Player Player1, Player2;
    private static GameManager instance;
    public float SwordForce;
    public float TopSpeed;
    public float SecsUntillTopSpeed;
    public AnimationCurve AccelarationCurve;

    private void Start()
    {
        if (!instance)
            instance = this;

        if(!Player1 || !Player2)
        {
            Debug.LogError("Player Game Objects not set on Game Manager");
            return;
        }
        if(!Player2.CompareTag("Player2"))
        {
            Debug.LogError("Player 2 not tagged as player 2");
        }

        Player1.Movement.TopSpeed = TopSpeed;
        Player2.Movement.TopSpeed = TopSpeed;

        Player1.Movement.AccelarationCurve = AccelarationCurve;
        Player2.Movement.AccelarationCurve = AccelarationCurve;

        Player1.Movement.SecsUntillTopSpeed = SecsUntillTopSpeed;
        Player2.Movement.SecsUntillTopSpeed = SecsUntillTopSpeed;
        
        Player1.Movement.SwordForce = SwordForce;
        Player2.Movement.SwordForce = SwordForce;



    }

    public static void LoseLimb(int Player)
    {
        SoundManager.PlayBlood();

        if (Player == 1)
        {
            instance.Player1.LimbManager.LoseLimb();
        }
        else if (Player == 2)
        {

            instance.Player2.LimbManager.LoseLimb();
        }
        else
            Debug.LogWarning("Player " + Player + " does not exist");
    }

    public void StartGame()
    {
        if (Input.GetKeyDown("W"))
        {
            Instantiate(Player1);
        }
    }
}
