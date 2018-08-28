using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public AudioSource FxAudio;
    public AudioSource BackgroundAudio;
    private static SoundManager instance;
    public AudioClip[] HitAudios;


    private void Start()
    {
        if (instance == null)
            instance = this;
    }
    
    public static void PlayHit()
    {
        if(instance.HitAudios.Length > 0 && instance.FxAudio)
            instance.FxAudio.PlayOneShot(instance.HitAudios[Random.Range(0, instance.HitAudios.Length)]);
    }

}
