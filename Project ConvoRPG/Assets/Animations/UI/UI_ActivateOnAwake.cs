using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UI_ActivateOnAwake : MonoBehaviour
{
    public UnityEvent onCanvasAwake;
    void Awake()
    {
        onCanvasAwake.Invoke();
    }
}
