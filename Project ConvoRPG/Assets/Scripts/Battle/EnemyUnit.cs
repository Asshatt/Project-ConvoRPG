using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    [System.Serializable]
    public class responseCollection 
    {
        public string categoryName;
        public response[] responses;
    }
    //array which contains all possible responses that the opponent can have
    public string name;
    [Space(10)]
    [SerializeField] public responseCollection[] responseCategories;
    [Space(10)]
    [Header("Enemy Specific Properties")]
    //phase which indicates which index responseCollection will get to parse a response
    public bool hasMultiplePhases = false;
    
    [HideInInspector]
    public int currentPhase = 0;
    [HideInInspector]
    public bool isFinalPhase = false;

    //chooses a response from the string
    public response chooseResponse(responseCollection[] response)
    {
        bool i = true;
        response chosenResponse = null;
        while (i)
        {
            chosenResponse = response[currentPhase].responses[UnityEngine.Random.Range(0, response[currentPhase].responses.Length)];
            //checks if the response has been repeated and rerolls if it has
            if (!chosenResponse.repeatable) 
            {
                if (!chosenResponse.hasBeenRepeated)
                {
                    i = false;
                    chosenResponse.hasBeenRepeated = true;
                }
            }
            else
            {
                i = false;
            }
        }
        return chosenResponse;
    }

    public int turnLimit;
}
