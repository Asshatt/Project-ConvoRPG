using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class response// : MonoBehaviour
{
    [Serializable]
    public class responseDisplayText 
    {
        [TextArea]
        public string text;
        public bool repeatable = true;

        [HideInInspector]
        public bool hasBeenShown;

        [Header("____________________")]

        [Tooltip("This controls what responses the player will show per prompt\nIndexes correspond to specific responses \n0 = Friendly \n1 = Sarcastic \n2 = Aggressive \n3 = Fearful\n4 = Deadpan")]
        [TextArea]
        public List<string> playerResponseTexts;
    }

    [Header("Enemy Properties")]
    public string responseText;
    public List<int> correctResponses;
    public List<int> decentResponses;
    public List<int> badResponses;
    public List<int> veryBadResponses;

    [Space(10)]

    [Tooltip("This controls what is displayed on the textbox. Allows for multiple prompts for the same response")]
    [Header("Response Display Text")]
    public List<responseDisplayText> responseDisplayTexts;
    
    [Space(10)]

    [Header("Player Properties")]
    public List<int> instinctualResponse;

    [Space(10)]

    [Header("Response Variables")]
    public bool repeatable = true;
    public float responseWeight = 1;

    [HideInInspector]
    public bool hasBeenRepeated = false;

    public responseDisplayText chosenDisplayText()
    {
        responseDisplayText chosen = null;
        if (responseDisplayTexts[0].text != "")
        {
            int j = 0;
            bool i = true;
            responseDisplayText chosenText = null;
            while (i)
            {
                j++;
                //debug code quit unity if loop loops too much
                if(j >= 20)
                {
                    Debug.LogError("Loop Never Closed. ResponseText=" + responseText);
                    Debug.Break();
                }

                chosenText = responseDisplayTexts[UnityEngine.Random.Range(0, responseDisplayTexts.Count)];
                Debug.Log(chosenText);
                if (chosenText.repeatable)
                {
                    i = false;
                }
                else if (!chosenText.repeatable && !chosenText.hasBeenShown)
                {
                    chosenText.hasBeenShown = true;
                    i = false;
                }
            }
            chosen = chosenText;
        }
        else
        {
            chosen = null;
        }
        return chosen;
    }
}
