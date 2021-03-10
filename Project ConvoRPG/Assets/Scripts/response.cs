using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Class made to handle responses from the opponent.
[Serializable]
public class response// : MonoBehaviour
{
    public string responseText;
    public int[] correctResponses;
    public int[] decentResponses;
    public int[] badResponses;
    public int[] veryBadResponses;
}
