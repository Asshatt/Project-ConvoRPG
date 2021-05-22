using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeInOrFadeOut : MonoBehaviour
{
    public enum fadeInOrFadeOut { Fade_In, Fade_Out };
    public fadeInOrFadeOut transitionType;
    public float transitionSpeed = 0.5f;
    Color32 initColor;
    Color32 transColor;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
        initColor = img.color;
        transColor = initColor;
        transColor.a = 0;

        if(transitionType == fadeInOrFadeOut.Fade_Out) 
        {
            LeanTween.value(gameObject, changeColor, initColor, transColor, transitionSpeed).setDestroyOnComplete(true);
        }
        else
        {
            LeanTween.value(gameObject, changeColor, transColor, initColor, transitionSpeed).setDestroyOnComplete(true);
        }
    }

    void changeColor(Color val)
    {
        img.color = val;
    }
}
