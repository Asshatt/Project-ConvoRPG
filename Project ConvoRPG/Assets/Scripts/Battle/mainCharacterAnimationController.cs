using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class mainCharacterAnimationController : MonoBehaviour
{
    [System.Serializable]
    public class AnimationContainer
    {
        public string name;
        public string animationClipName = "idle";
        public float length = 1f;
    }

    public Animator animator;
    public string idleAnimation;
    public AnimationContainer[] responseAnimations;
    public AnimationContainer[] stimAnimations;
    public AnimationContainer damageAnimation;
    [HideInInspector]
    public float currentAnimLength;
    // Start is called before the first frame update
    void Start()
    {
        animator.Play(idleAnimation);
    }
    //plays damage animation
    public void playDamageAnimation()
    {
        currentAnimLength = damageAnimation.length / 1.5f;
        StartCoroutine(playAnimation(damageAnimation.animationClipName));
    }

    //function that gets the stim index and uses it to play an animation
    public void animateStim(int index)
    {
        currentAnimLength = stimAnimations[index].length / 1.5f;
        StartCoroutine(playAnimation(stimAnimations[index].animationClipName));
    }

    //same banana as the above function but using a different array 
    public void animateResponse(int index)
    {
        currentAnimLength = responseAnimations[index].length / 1.5f;
        StartCoroutine(playAnimation(responseAnimations[index].animationClipName));
    }

    //play the animation and after its done shift back to the idle animation
    IEnumerator playAnimation(string clip)
    {
        animator.Play(clip);
        yield return new WaitForSeconds(currentAnimLength);
        animator.Play(idleAnimation);
        yield break;
    }
}
