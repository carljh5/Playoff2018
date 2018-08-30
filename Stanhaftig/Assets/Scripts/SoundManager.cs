using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioSource FxAudio;
    public AudioSource PanLeft;
    public AudioSource PanRight;
    public AudioSource BackgroundAudio;
    private static SoundManager instance;
    public AudioClip[] HitAudios;
    public AudioClip[] BloodAudios;
    public AudioClip[] YellPlayer1;
    public AudioClip[] YellPlayer2;
    public AudioClip[] SpeechYellPlayer1;
    public AudioClip[] SpeechYellPlayer2;
    public AudioClip WinMusic;
    public AudioClip[] FreezeAudio;
    public AudioClip Cry, Laugh, PutOnBucket;
    int lastSpeech;

    bool fading = false;


    private void Start()
    {
        if (instance == null)
            instance = this;
            lastSpeech = 0;
    }

    public static void PlayCry()
    {
        if (instance.Cry && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.Cry);
    }

    public static void PlayBucket()
    {
        if (instance.PutOnBucket && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.PutOnBucket);
    }

    public static void PlayLaugh()
    {
        if (instance.Laugh && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.Laugh);
    }

    public static void PlayHit()
    {
        if (instance.HitAudios.Length > 0 && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.HitAudios[Random.Range(0, instance.HitAudios.Length)]);
    }

    public static void PlayBlood()
    {
        if (instance.BloodAudios.Length > 0 && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.BloodAudios[Random.Range(0, instance.BloodAudios.Length)]);
    }

    public static void PlayYellPlayer1()
    {
        if (instance.YellPlayer1.Length > 0 && instance.PanLeft)
            instance.PanLeft.PlayOneShot(instance.YellPlayer1[Random.Range(0, instance.YellPlayer1.Length)]);
    }

    public static void PlayYellPlayer2()
    {
        if (instance.YellPlayer2.Length > 0 && instance.PanRight)
            instance.PanRight.PlayOneShot(instance.YellPlayer2[Random.Range(0, instance.YellPlayer2.Length)]);
    }

    public static void PlaySpeechPlayer1()
    {
        if (instance.SpeechYellPlayer1.Length > 0 && instance.PanLeft)
            
            instance.PanLeft.PlayOneShot(instance.SpeechYellPlayer1[instance.lastSpeech]);
            instance.lastSpeech = instance.RandomRangeExcept(0, instance.SpeechYellPlayer1.Length, instance.lastSpeech);
    }

    public static void PlaySpeechPlayer2()
    {
        if (instance.SpeechYellPlayer2.Length > 0 && instance.PanRight)

            instance.PanRight.PlayOneShot(instance.SpeechYellPlayer2[instance.lastSpeech]);
        instance.lastSpeech = instance.RandomRangeExcept(0, instance.SpeechYellPlayer2.Length, instance.lastSpeech);
    }

    public static void PlayWin()
    {
        if (instance.WinMusic && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.WinMusic);

        if(!instance.fading)
        {
            instance.fading = true;
            instance.StartCoroutine(instance.fadeRoutine());
        }
    }

    public static void PlayFreeze()
    {
        if (instance.FreezeAudio.Length > 0 && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.FreezeAudio[Random.Range(0, instance.FreezeAudio.Length)]);
    }

    public int RandomRangeExcept(int min, int max, int except)
    {
        int random = Random.Range(min, max);
        if (random >= except) random = (random + 1) % max;
        return random;
    }

    private IEnumerator fadeRoutine()
    {
        var fadeTime = 5f;
        var start = Time.time;

        var startVol = BackgroundAudio.volume;

        while(start + fadeTime >= Time.time)
        {
            BackgroundAudio.volume = startVol * (1-(Time.time - start) / fadeTime);

            yield return new WaitForFixedUpdate();

        }

    }
    
}
