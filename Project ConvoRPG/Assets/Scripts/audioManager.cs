using UnityEngine.Audio;
using System;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [System.Serializable]
    public class soundCategories
    {
        public string categoryName;
        public Sound[] sounds;
    }

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
        }
        else
        {
            Debug.LogError("Sound " + name + " not found.");
        }
    }
}
