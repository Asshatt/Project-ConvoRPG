using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAnim : MonoBehaviour
{
    public float initialSpeed = 0.2f;
    public float displayTime = 1f;
    public float fadeoutTime = 0.4f;

    Color32 color;
    Color32 fadeoutColor;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        color = sr.color;
        fadeoutColor = color;
        fadeoutColor.a = 0;
        gameObject.transform.localScale = new Vector3(transform.localScale.x * 2, 0, transform.localScale.z);
        StartCoroutine(Animate());
    }

    IEnumerator Animate() 
    {
        LeanTween.value(gameObject, updateColor, fadeoutColor, color, initialSpeed).setEaseOutSine();
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), initialSpeed).setEaseOutSine();
        yield return new WaitForSeconds(displayTime);
        LeanTween.value(gameObject, updateColor, color, fadeoutColor, fadeoutTime).setEaseInSine();
        LeanTween.scale(gameObject, new Vector3(0, 2, 1), fadeoutTime).setEaseInSine().setDestroyOnComplete(true);
    }

    void updateColor(Color val)
    {
        sr.color = val;
    }
}
