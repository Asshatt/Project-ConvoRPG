using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_damageIndicator : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public float distance = 1;
    public float displayTime = 0.65f;
    public float fadeSpeed = 0.2f;
    Color32 color;
    Color32 fadeOutColor;
    TextMeshPro text;

    float randomFrameOffset;

    //parse color of text and make a transparent version
    private void Start()
    {
        text = gameObject.GetComponent<TextMeshPro>();
        color = text.color;
        fadeOutColor = color;
        fadeOutColor.a = 0;
        randomFrameOffset = Random.Range(0.01f, 0.03f);
        StartCoroutine(animate());
    }

    IEnumerator animate() 
    {
        yield return new WaitForSeconds(randomFrameOffset);
        transform.LeanMoveLocalY(distance, moveSpeed).setEaseOutSine();
        yield return new WaitForSeconds(displayTime);
        LeanTween.value(gameObject, updateColor, color, fadeOutColor, displayTime).setDestroyOnComplete(true);
    }

    void updateColor(Color val) 
    {
        text.color = val;
    }
}
