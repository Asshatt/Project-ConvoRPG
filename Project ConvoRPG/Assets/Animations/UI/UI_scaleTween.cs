using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_scaleTween : MonoBehaviour
{
    public float transitionSpeed = 0.2f;
    //scales UI to scale 1
    public void scaleTween()
    {
        transform.LeanScale(Vector3.one, transitionSpeed).setEaseInOutQuad();
    }
    private void OnEnable()
    {
        //set scale of object to zero, so that scaletween can set it to 1
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        scaleTween();
    }
}
