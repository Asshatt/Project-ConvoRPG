using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [System.Serializable]
    public class soundCategories
    {
        public string categoryName;
        public Sound[] sounds;
    }

    private static audioManager _audio;

    [HideInInspector]
    //variable that holds the song thats currently playing
    public AudioSource currentlyPlayingMusic;

    public static audioManager audio 
    {
        get 
        {
            if (_audio == null)
            {
                _audio = GameObject.FindObjectOfType<audioManager>();
            }
            return _audio;
        }
    }
    public AudioMixerSnapshot[] mixerSnapshots;

    public soundCategories[] soundEffects;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(soundCategories i in soundEffects) 
        {
            foreach(Sound s in i.sounds) 
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;

                s.source.loop = s.loop;

                s.source.outputAudioMixerGroup = s.audioGroup;
            }
        }
    }

    public void Play(string name) 
    {
        Sound sound = null;
        for (int i = 0; i < soundEffects.Length; i++)
        {
            sound = Array.Find(soundEffects[i].sounds, j => j.name == name);
            if(sound != null) 
            {
                break;
            }
        }
        if (sound != null)
        {
            sound.source.Play();
            if (sound.audioType == Sound.soundType.Music) 
            {
                currentlyPlayingMusic = sound.source;
            }
        }
        else
        {
            Debug.LogError("Sound " + name + " not found.");
        }
    }

    public void transitionToAudioSnapshot(int index) 
    {
        mixerSnapshots[index].TransitionTo(0.5f);
    }
}
