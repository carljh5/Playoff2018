using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public SwordManager swordManager;
    public Movement Movement;
    public LimbManager LimbManager;

    private void Start()
    {

        if (CompareTag("Player"))
        {
            swordManager.opponent = 2;
            swordManager.opponentTag = "Player2";
        }
        else if (CompareTag("Player2"))
        {
            swordManager.opponent = 1;
            swordManager.opponentTag = "Player";
        }
        else
            Debug.LogWarning("Wrong Player tag set: " + tag);
    }
}
