using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour {
    public int opponent { get; internal set; }
    public string opponentTag;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude < 10)
            return;


        Debug.Log(collision.gameObject + ", " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag(opponentTag))
        {

            GameManager.LoseLimb(opponent);
        }

        else if (collision.gameObject.CompareTag("Sword"))
        {
            SoundManager.PlayHit();
        }
    }
}
