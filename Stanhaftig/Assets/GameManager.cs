using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Player Player1, Player2;
    private static GameManager instance;

    private void Start()
    {
        if (!instance)
            instance = this;

        if(!Player1 || !Player2)
        {
            Debug.LogError("Player Game Objects not set on Game Manager");
        }

        if(!Player2.CompareTag("Player2"))
        {
            Debug.LogError("Player 2 not tagged as player 2");
        }
    }

    public static void LoseLimb(int Player)
    {
        if (Player == 1)
        {
            instance.Player1.LimbManager.LoseLimb();
        }
        else if (Player == 1)
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
