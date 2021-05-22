using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakdownPostProController : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        animator.SetBool("isMentalShutdown", battleManager.instance.isMentalShutdown);
    }
}
