using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleCameraController : MonoBehaviour
{
    public Animator animator;
    public GameObject battleManagerObj;

    // Update is called once per frame
    void Update()
    {
        switch (battleManager.instance.state) 
        {
            case battleState.playerTurn:
                animator.SetBool("isPlayerTurn", true);
                break;
            case battleState.enemyTurn:
                animator.SetBool("isPlayerTurn", false);
                break;
            default:
                animator.SetBool("isPlayerTurn", false);
                break;
        }
    }
}
