using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TauntManager : MonoBehaviour {

    public static TauntManager instance;

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


    [Serializable]
    public class Taunt
    {
        //State requirements
        public bool isAgressor;
        [Range(1,5)]
        public int agressorLimbsLeft = 5;
        [Range(1, 5)]
        public int defenderLimbsLeft = 5;


        //state of taunt
       [HideInInspector]
        public bool hasPlayed;
        [HideInInspector]
        public bool isPossible;

        //the taunt string
        public string taunt;

        public Taunt(bool isAgressor, int agressorLimbsLeft, int defenderLimbsLeft, bool hasPlayed, bool isPossible, string taunt) {
            this.isAgressor = isAgressor;
            this.agressorLimbsLeft = agressorLimbsLeft;
            this.defenderLimbsLeft = defenderLimbsLeft;
            this.hasPlayed = hasPlayed;
            this.isPossible = isPossible;
            this.taunt = taunt;
        }

        public Taunt Clone() {
            return new Taunt(isAgressor, agressorLimbsLeft, defenderLimbsLeft, hasPlayed, isPossible, taunt);
        }
    }

    public List<Taunt> taunts = new List<Taunt>();

    //Make copies for each player
   public List<Taunt> player1Taunts = new List<Taunt>();
   public List<Taunt> player2Taunts = new List<Taunt>();




    private void Start()
    {
        if(instance == null)
            instance = this;

        foreach(Taunt t in taunts) {
            player1Taunts.Add(t.Clone());
            player2Taunts.Add(t.Clone());
        }
    }


    public static void PlayTaunt(Player attacker, Player defender)
    {
        //Update state;
        instance.agressor = attacker.CompareTag("Player") ? Agressor.player1 : Agressor.player2;
        instance.player1LimbsLeft = attacker.CompareTag("Player") ? attacker.LimbManager.limbs.Count: defender.LimbManager.limbs.Count;
        if (attacker.CompareTag("Player"))
        {
            instance.agressor = Agressor.player1;
            instance.player1LimbsLeft = attacker.LimbManager.limbs.Count;
            instance.player2LimbsLeft = defender.LimbManager.limbs.Count;
        }
        else
        {
            instance.agressor = Agressor.player2;
            instance.player1LimbsLeft = defender.LimbManager.limbs.Count;
            instance.player2LimbsLeft = attacker.LimbManager.limbs.Count;
        }


        //update state of taunts for each player;
        foreach (Taunt t in instance.taunts)
        {
            if (instance.agressor == Agressor.player1)
            {
                if (instance.player1LimbsLeft <= t.agressorLimbsLeft && instance.player2LimbsLeft <= t.defenderLimbsLeft)
                {
                    if (t.isAgressor)
                    {
                        instance.player1Taunts[instance.taunts.IndexOf(t)].isPossible = true;
                    }
                    else
                    {
                        instance.player2Taunts[instance.taunts.IndexOf(t)].isPossible = true;
                    }

                }
            }
            else
            {
                if (instance.player2LimbsLeft <= t.agressorLimbsLeft && instance.player1LimbsLeft <= t.defenderLimbsLeft)
                {
                    if (t.isAgressor)
                    {
                        instance.player2Taunts[instance.taunts.IndexOf(t)].isPossible = true;
                    }
                    else
                    {
                        instance.player1Taunts[instance.taunts.IndexOf(t)].isPossible = true;
                    }
                }
            }
        }
        if (instance.co == null)
           instance.co = instance.StartCoroutine(instance.TauntSequence());
    }


    private Taunt GetPossibleTauntThatHasNotPlayed(List<Taunt> ts) {
        List<Taunt> possibleTaunts = new List<Taunt>();
        foreach(Taunt t in ts) {
            if(t.isPossible && !t.hasPlayed) {
                possibleTaunts.Add(t);
               
            }
        }
        if(possibleTaunts.Count > 0) {
            Taunt temp = possibleTaunts[UnityEngine.Random.Range(0, possibleTaunts.Count-1)];
            temp.hasPlayed = true;
            return temp;
        }
        //Reset all possible taunts so they can play again;
        foreach(Taunt t in ts) {
            if (t.isPossible)
            {
                t.hasPlayed = false;
                possibleTaunts.Add(t);
            }
        }
        Taunt returnTaunt = possibleTaunts[UnityEngine.Random.Range(0, possibleTaunts.Count - 1)];
        returnTaunt.hasPlayed = true;
        return returnTaunt;
        
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
                    player1text.text = instance.GetPossibleTauntThatHasNotPlayed(player1Taunts).taunt;
                    player1text.gameObject.SetActive(true);
                    taunter = Agressor.player2;
                } else {
                    player2text.text = instance.GetPossibleTauntThatHasNotPlayed(player2Taunts).taunt;
                    player2text.gameObject.SetActive(true);
                    taunter = Agressor.player1;
                }
                time = 0;

            }
            yield return null;
        }

    }



}
