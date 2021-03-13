using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class response// : MonoBehaviour
{
    [Header("Enemy Properties")]
    public string responseText;
    public List<int> correctResponses;
    public List<int> decentResponses;
    public List<int> badResponses;
    public List<int> veryBadResponses;

    [Header("Player Properties")]
    public List<int> instinctualResponse;
}
