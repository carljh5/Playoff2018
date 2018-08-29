using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour {
    public int opponent { get; internal set; }
    public string opponentTag;
    [HideInInspector] public float CollisionDelay;
    public GameObject sparkleParticlesPrefab;
    public TrailRenderer SwordTrailParticles;
    private Vector2 LastPosistion;
    public float SwingSpeedForSparkles = 5;

    private void Start()
    {
        if(!SwordTrailParticles)
        {
            SwordTrailParticles = GetComponentInChildren<TrailRenderer>();
        }

    }

    public void FixedUpdate()
    {
        CollisionDelay -= Time.fixedDeltaTime;

        if (Vector2.Distance(transform.position, LastPosistion) > SwingSpeedForSparkles)
        {
            Debug.Log("Swiing");
            SwordTrailParticles.gameObject.SetActive(true);
        }
        else
            SwordTrailParticles.gameObject.SetActive(false);

        LastPosistion = transform.position;

       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude < 10)
            return;


        Debug.Log(collision.gameObject + ", " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag(opponentTag) && CollisionDelay <= 0)
        {
            CollisionDelay = GameManager.GetCollisionDelay();

            GameManager.LoseLimb(opponent);
        }

        else if (collision.gameObject.CompareTag("Sword"))
        {
            SoundManager.PlayHit();
            //spawn blood particles and child them to the connected rigidbody of the joint at the joints position;
            var x = Instantiate(sparkleParticlesPrefab, transform);
            x.transform.position = collision.GetContact(0).point;
        }
    }
}
