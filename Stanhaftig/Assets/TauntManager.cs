using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TauntManager : MonoBehaviour {

    public static TauntManager instance;
    public TextMeshPro player1text;
    public TextMeshPro player2text;


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

        instance.player1PossibleTaunts.Clear();
        instance.player2PossibleTaunts.Clear();

      
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
            } else {
                if (instance.player2LimbsLeft <= t.agressorLimbsLeft && instance.player1LimbsLeft <= t.defenderLimbsLeft)
                {
                    if (t.isAgressor)
                        instance.player2PossibleTaunts.Add(t.taunt);
                    else
                        instance.player1PossibleTaunts.Add(t.taunt);

                }
            }
        }

        instance.StartCoroutine(instance.TauntSequence());

    }

   public IEnumerator TauntSequence() {
        yield return null;
        if (instance.agressor == Agressor.player1)
        {
            player1text.text = player1PossibleTaunts[UnityEngine.Random.Range(0, player1PossibleTaunts.Count - 1)];
            player1text.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            player2text.text = player2PossibleTaunts[UnityEngine.Random.Range(0, player2PossibleTaunts.Count - 1)];
            player2text.gameObject.SetActive(true);
        }
        else
        {
            player2text.text = player2PossibleTaunts[UnityEngine.Random.Range(0, player2PossibleTaunts.Count - 1)];
            player2text.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            player1text.text = player1PossibleTaunts[UnityEngine.Random.Range(0, player1PossibleTaunts.Count - 1)];
            player1text.gameObject.SetActive(true);
        }
    }



}
