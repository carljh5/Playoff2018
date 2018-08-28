using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TauntManager : MonoBehaviour {

    public static TauntManager instance;
    public Text text;

    [Serializable]
    public class Taunt
    {
        [Range(1,5)]
        public int taunterLimbsLeft = 5;
        [Range(1, 5)]
        public int tauntedLimbsLeft = 5;
        public string taunt;
    }

    public List<Taunt> taunts = new List<Taunt>();

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    public static void PlayTaunt(Player taunter, Player taunted)
    {
        List<Taunt> filteredTaunts = new List<Taunt>();
        foreach (Taunt t in instance.taunts)
        {
            if(taunter.LimbManager.limbs.Count <= t.taunterLimbsLeft && taunted.LimbManager.limbs.Count <= t.tauntedLimbsLeft)
                filteredTaunts.Add(t);
        }
        instance.text.text = filteredTaunts[UnityEngine.Random.Range(0, filteredTaunts.Count - 1)].taunt;
        instance.text.transform.parent.gameObject.SetActive(true);
        //return filteredTaunts[UnityEngine.Random.Range(0, filteredTaunts.Count - 1)].taunt;
    }

    //public static string Taunt(TauntCode code, Player playerA, Player playerB) {
    //    List<Taunt> filteredTaunts = new List<Taunt>();
    //    foreach (Taunt t in instance.taunts)
    //    {
    //        if (t.category.Equals(code)) {
    //            if(playerA.LimbManager.limbs.Count )
    //        }
    //            filteredTaunts.Add(t);
    //    }
    //    return filteredTaunts[UnityEngine.Random.Range(0, filteredTaunts.Count - 1)].taunt;
    //}

}
