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
    public AudioClip[] YellAudios;
    public AudioClip[] SpeechYellPlayer1;
    public AudioClip[] SpeechYellPlayer2;
    public AudioClip WinMusic;
    public AudioClip[] FreezeAudio;
    int lastSpeech;


    private void Start()
    {
        if (instance == null)
            instance = this;
            lastSpeech = 0;
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

    public static void PlayYell()
    {
        if (instance.YellAudios.Length > 0 && instance.PanRight)
            instance.PanRight.PlayOneShot(instance.YellAudios[Random.Range(0, instance.YellAudios.Length)]);
    }

    public static void PlaySpeechPlayer1()
    {
        if (instance.SpeechYellPlayer1.Length > 0 && instance.PanLeft)
            
            instance.PanLeft.PlayOneShot(instance.SpeechYellPlayer1[instance.lastSpeech]);
            instance.lastSpeech = instance.RandomRangeExcept(0, instance.SpeechYellPlayer1.Length, instance.lastSpeech);
    }

    public static void PlaySpeechPlayer2()
    {
        if (instance.SpeechYellPlayer2.Length > 0 && instance.FxAudio)

            instance.FxAudio.PlayOneShot(instance.SpeechYellPlayer2[instance.lastSpeech]);
        instance.lastSpeech = instance.RandomRangeExcept(0, instance.SpeechYellPlayer2.Length, instance.lastSpeech);
    }

    public static void PlayWin()
    {
        if (instance.WinMusic && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.WinMusic);
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

}
