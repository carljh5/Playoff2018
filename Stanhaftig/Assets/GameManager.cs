
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private bool FreezeMode;
    public ParticleSystem FreezeParticle;
    [SerializeField]
    private bool LivingSwordMode;
    public Player Player1, Player2;
    private static GameManager instance;
    public float SwordForce;
    public float TopSpeed;
    public float SecsUntillTopSpeed;
    public AnimationCurve AccelarationCurve;
    //to prevent cutting of all the limbs at once
    public float CollisionDelay;
    public KeyCode RestartKey;

    public static Transform GetPlayerHeadTransform(bool isPlayer1) {
        if (isPlayer1)
            return instance.Player1.Movement.HeadBody.transform;
        return instance.Player2.Movement.HeadBody.transform;
    }

    private void Start()
    {
        if (!instance)
            instance = this;

        if (!Player1 || !Player2)
        {
            Debug.LogError("Player Game Objects not set on Game Manager");
            return;
        }
        if (!Player2.CompareTag("Player2"))
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

        Player1.gameObject.SetActive(false);
        Player2.gameObject.SetActive(false);

    }

    public static float GetCollisionDelay() {
        return instance.CollisionDelay; }

    public static void LoseLimb(int Player)
    {
        SoundManager.PlayBlood();

        if (Player == 1)
        {
            instance.Player1.LimbManager.LoseLimb();
            TauntManager.PlayTaunt(instance.Player2, instance.Player1);
            SoundManager.PlayYellPlayer1();
        }
        else if (Player == 2)
        {

            instance.Player2.LimbManager.LoseLimb();
            TauntManager.PlayTaunt(instance.Player1, instance.Player2);
            SoundManager.PlayYellPlayer2();
        }
        else
            Debug.LogWarning("Player " + Player + " does not exist");
    }

    void Update()
    {
        StartGame();

        if (Input.GetKeyDown(RestartKey))
            SceneManager.LoadScene(Application.loadedLevel);
    }
    

    public void StartGame()
    {
        if (Input.GetKeyDown(Player1.Movement.Jump))
        {
            Player1.gameObject.SetActive(true);
            Player1.KeyHintCanvas.SetActive(false);
        }

        if (Input.GetKeyDown(Player2.Movement.Jump))
        {
            Player2.gameObject.SetActive(true);
            Player2.KeyHintCanvas.SetActive(false);
        }
    }

    public static bool Freeze() { return instance.FreezeMode; }

    public static bool LivingSwordsMode() { return instance.LivingSwordMode; }
    public static ParticleSystem GetFreezeParticle() { return instance.FreezeParticle; }


    public static bool GameStarted()
    {
        return instance.Player1.isActiveAndEnabled && 
            instance.Player2.isActiveAndEnabled;
    }
}
