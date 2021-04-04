using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class doOnSceneAwake : MonoBehaviour
{
    public UnityEvent onAwake;
    private void Start()
    {
        onAwake.Invoke();
    }
}
