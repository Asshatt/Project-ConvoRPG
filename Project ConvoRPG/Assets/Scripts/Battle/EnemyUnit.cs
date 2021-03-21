using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    //array which contains all possible responses that the opponent can have
    public string name;
    [SerializeField] public response[] responses;

    //chooses a response from the string
    public response chooseResponse(response[] responses)
    {
        response chosenResponse;
        chosenResponse = responses[UnityEngine.Random.Range(0, responses.Length)];
        return chosenResponse;
    }

    public int turnLimit;
}
