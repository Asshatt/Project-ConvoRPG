using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_tooltipManager : MonoBehaviour
{
    public EventSystem eventSystem;
    public TextMeshProUGUI tooltipText;

    // Update is called once per frame
    void Update()
    {
        tooltipText.text = "";
        if (eventSystem.currentSelectedGameObject.GetComponent<UI_tooltip>() != null)
        {
            string tooltip = eventSystem.currentSelectedGameObject.GetComponent<UI_tooltip>().tooltip;
            if (tooltip != null)
            {
                tooltipText.text = tooltip;
            }
        }
    }
}
