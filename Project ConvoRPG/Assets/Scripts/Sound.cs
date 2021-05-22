using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop = false;

    public enum soundType 
    {
        Sound_Effect,
        Music,
        Ambience
    }

    public soundType audioType;

    [HideInInspector]
    public AudioSource source;

    public AudioMixerGroup audioGroup;
}
