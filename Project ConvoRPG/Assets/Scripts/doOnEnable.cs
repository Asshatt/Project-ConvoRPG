using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class doOnEnable : MonoBehaviour
{
    public UnityEvent Enable;
    private void OnEnable()
    {
        Enable.Invoke();
    }
}
