using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uniquePhaseMusic : MonoBehaviour
{
    [Tooltip("name of audio clip found in AudioManager")]
    public string audioClipName;
    [Tooltip("Phase in which to play the desired clip")]
    public int phaseToPlay;
    
    int phase;
    EnemyUnit unit;
    GameObject phaseIndicator;
    // Start is called before the first frame update
    void Start()
    {
        phaseIndicator = GameObject.Find("phaseIndicator");
        unit = gameObject.GetComponent<EnemyUnit>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (unit.currentPhase == phaseToPlay - 1 && phaseIndicator.active) 
        {
            StartCoroutine(playMusic());
        }
    }

    IEnumerator playMusic() 
    {
        audioManager.audio.currentlyPlayingMusic.Stop();
        yield return new WaitForSeconds(1.5f);
        audioManager.audio.Play(audioClipName);
        Destroy(this);
    }
}
