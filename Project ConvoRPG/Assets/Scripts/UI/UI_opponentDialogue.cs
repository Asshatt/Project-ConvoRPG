using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_opponentDialogue : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    //function to set opponent dialogue
    /*
     * TODO: Replace string input with the proper "response" class
     * which includes the response string and values for which player responses are correct
    */
    public void setOpponentDialogue(string dialogue)
    {
        text.text = dialogue;
    }
}
