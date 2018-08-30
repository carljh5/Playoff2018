using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechScript : MonoBehaviour
{
    public GameObject SpeechBubble;
    public Text m_Text;
    public string[] RandomTaunts;
    private float timeleft;

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
            Speak();
        }

    }

    public void Speak()
    {
        SpeechBubble.SetActive(true);
        m_Text.text = RandomTaunts[Random.Range(0, RandomTaunts.Length)];
    }
}
