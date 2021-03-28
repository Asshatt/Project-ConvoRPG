using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_turnIndicator : MonoBehaviour
{
    TextMeshProUGUI text;
    public float fadeInSpeed = 0.2f;
    public float staySpeed = 1f;
    public float fadeOutSpeed = 0.2f;
    Color32 color;
    Color32 fadeoutcolor;
    //sets color variables to be text colors
    private void Start()
    {
        color = text.color;
        fadeoutcolor = color;
        fadeoutcolor.a = 0;
        gameObject.SetActive(false);
    }
    //fades the color in, waits for determined amount of time, fades the color out
    public IEnumerator fadeTween()
    {
        gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
        LeanTween.rotate(gameObject, Vector3.zero, fadeInSpeed).setEaseInOutQuad();
        LeanTween.value(gameObject, updateValueExampleCallback, fadeoutcolor, color, fadeInSpeed).setEaseInOutQuad();
        yield return new WaitForSeconds(staySpeed);
        LeanTween.value(gameObject, updateValueExampleCallback, color, fadeoutcolor, fadeOutSpeed).setEaseInOutQuad();
        LeanTween.rotate(gameObject, new Vector3(90,0,0), fadeInSpeed).setEaseInOutQuad();
        yield return new WaitForSeconds(fadeOutSpeed);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(fadeTween());
    }
    void updateValueExampleCallback(Color val)
    {
        text.color = val;
    }
}
