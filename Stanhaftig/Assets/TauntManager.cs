using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TauntManager : MonoBehaviour {

    public static TauntManager instance;

    public enum TauntTiming {
        Hurt,
        Hit,
        Idle
    }

    
    

    [Serializable]
    public class Taunt
    {
        public bool playerAAdvantage;
        public bool playerBAdvantage;
        [Range(1,5)]
        public int playerALimbsLeft = 5;
        [Range(1, 5)]
        public int playerBLimbsLeft = 5;
        public string taunt;
    }

    public List<Taunt> taunts = new List<Taunt>();

    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    public static string GetTaunt() {
        List<Taunt> filteredTaunts = new List<Taunt>();
        foreach (Taunt t in instance.taunts)
        {
            if (t.category.Equals(code))
                filteredTaunts.Add(t);
        }
        return filteredTaunts[UnityEngine.Random.Range(0, filteredTaunts.Count - 1)].taunt;
    }

}
