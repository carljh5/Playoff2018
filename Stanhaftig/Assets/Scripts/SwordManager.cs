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
    public float SwingSpeedForSparkles = 0.1f;
    private Rigidbody2D SwordBody;
    private Movement movement;
    [HideInInspector]
    public LimbManager oponentLimbManager;

    private void Start()
    {
        movement = GetComponentInParent<Movement>();
        SwordBody = GetComponent<Rigidbody2D>();

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
            //Debug.Log("Swiing");
            SwordTrailParticles.gameObject.SetActive(true);
        }
        else
            SwordTrailParticles.gameObject.SetActive(false);

        LastPosistion = transform.position;

       
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.relativeVelocity.magnitude < 4) || (!movement.frozen && collision.relativeVelocity.magnitude < 10)  )
            return;


        //Debug.Log(collision.gameObject + ", " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag(opponentTag) && CollisionDelay <= 0)
        {
            CollisionDelay = GameManager.GetCollisionDelay();

            collision.otherRigidbody.AddForce(collision.relativeVelocity*-1);

            GameManager.LoseLimb(opponent,oponentLimbManager.GetLimb(collision.gameObject));
        }

        else if (collision.gameObject.CompareTag("Sword"))
        {
            SoundManager.PlayHit();
            //spawn blood particles and child them to the connected rigidbody of the joint at the joints position;
            var x = Instantiate(sparkleParticlesPrefab, transform);
            x.transform.position = collision.GetContact(0).point;

            SwordBody.AddForce( collision.relativeVelocity * -20f);
        }
    }
}
