using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class made to handle responses from the opponent.
[Serializable]
public class response// : MonoBehaviour
{
    public string responseText;
    public List<int> correctResponses;
    public List<int> decentResponses;
    public List<int> badResponses;
    public List<int> veryBadResponses;
}
