using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechScript : MonoBehaviour
{
    public GameObject SpeechBubble;

    // Use this for initialization
    void Start()
    {
        SpeechBubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SpeechBubble.SetActive(true);
        }
    }
}
