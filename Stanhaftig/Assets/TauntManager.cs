using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TauntManager : MonoBehaviour {

    public static TauntManager instance;
    //public GameObject player1TextPrefab;
   // public GameObject player2TextPrefab;

    public TextMeshPro player1text;
    public TextMeshPro player2text;

    public Coroutine co;

    public int player1LimbsLeft;
    public int player2LimbsLeft;
    public enum Agressor
    {
        player1,
        player2
    }
    public Agressor agressor;

    public List<string> player1PossibleTaunts = new List<string>();

    public List<string> player2PossibleTaunts = new List<string>();

    [Serializable]
    public class Taunt
    {
        public bool isAgressor;
        [Range(1,5)]
        public int agressorLimbsLeft = 5;
        [Range(1, 5)]
        public int defenderLimbsLeft = 5;
        public string taunt;
    }

    public List<Taunt> taunts = new List<Taunt>();

    private void Start()
    {
        if(instance == null)
            instance = this;
        //GameObject player1Prefab = Instantiate(player1TextPrefab);
        //player1Prefab.GetComponent<terminateOnTime>().
        //Instantiate(player2TextPrefab);

        //instance.player1text = player1TextPrefab.GetComponent<TextMeshPro>();
        //instance.player2text = player2TextPrefab.GetComponent<TextMeshPro>();
    }



    public static void PlayTaunt(Player attacker, Player defender)
    {
        instance.agressor = attacker.CompareTag("Player") ? Agressor.player1 : Agressor.player2;
        instance.player1LimbsLeft = attacker.CompareTag("Player") ? attacker.LimbManager.limbs.Count: defender.LimbManager.limbs.Count;
        if(attacker.CompareTag("Player")) {
            instance.agressor = Agressor.player1;
            instance.player1LimbsLeft = attacker.LimbManager.limbs.Count;
            instance.player2LimbsLeft = defender.LimbManager.limbs.Count;
        }  else {
            instance.agressor = Agressor.player2;
            instance.player1LimbsLeft = defender.LimbManager.limbs.Count;
            instance.player2LimbsLeft = attacker.LimbManager.limbs.Count;
        }

        //instance.player1PossibleTaunts.Clear();
       //instance.player2PossibleTaunts.Clear();

        //if(instance.player1PossibleTaunts.Count == 0 || instance.player2PossibleTaunts.Count == 0) {
        //    instance.player1PossibleTaunts.Clear();
        //    instance.player2PossibleTaunts.Clear();
        //} else {
        //    return;
        //}


        foreach (Taunt t in instance.taunts)
        {
            if (instance.agressor == Agressor.player1)
            {
                if (instance.player1LimbsLeft <= t.agressorLimbsLeft && instance.player2LimbsLeft <= t.defenderLimbsLeft)
                {
                    if (t.isAgressor)
                        instance.player1PossibleTaunts.Add(t.taunt);
                    else
                        instance.player2PossibleTaunts.Add(t.taunt);

                }
            }
            else
            {
                if (instance.player2LimbsLeft <= t.agressorLimbsLeft && instance.player1LimbsLeft <= t.defenderLimbsLeft)
                {
                    if (t.isAgressor)
                        instance.player2PossibleTaunts.Add(t.taunt);
                    else
                        instance.player1PossibleTaunts.Add(t.taunt);
                }
            }
        }
        if (instance.co == null)
           instance.co = instance.StartCoroutine(instance.TauntSequence());
    }




    public IEnumerator TauntSequence() {
        float time = 0;
        float delayDur = 3f;
        Agressor taunter = instance.agressor;
        while(true) {
            if (time < delayDur)
            {
                time += Time.deltaTime;
            }
            else
            {
                if(taunter == Agressor.player1) {
                    string taunt = player1PossibleTaunts[UnityEngine.Random.Range(0, player1PossibleTaunts.Count - 1)];
                    player1text.text = taunt;
                    player1PossibleTaunts.Remove(taunt);
                    player1text.gameObject.SetActive(true);
                    taunter = Agressor.player2;
                } else {
                    string taunt = player2PossibleTaunts[UnityEngine.Random.Range(0, player2PossibleTaunts.Count - 1)];
                    player2text.text = taunt;
                    player2PossibleTaunts.Remove(taunt);
                    player2text.gameObject.SetActive(true);
                    taunter = Agressor.player1;
                }
                time = 0;

            }
            yield return null;
        }

    }



}
