using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChannelSettings
{

    public AudioClip sourceSound = null;
    [Tooltip("Source frequency in Hz, should be the same as the frequency of the sound in the source sound")]
    public float sourceFrequency = 100;
    [Range(-36, 36)]
    public int noteOffset = 0;
    [Tooltip("The amount of Audio Sources the midi reader will utulize. More channels means more CPU cost, but having the ability to play more notes at the same time or in short succession.")]
    public int channels = 1;
    [HideInInspector]
    public List<AudioSource> audioSources;
    [HideInInspector]
    public List<GameObject> gameObjects;

    public void GenerateChannels()
    { //Generate audio sources to play the midi audio through
        for (int i = 0; i<channels; i++)
        {
            string name = "Channel " + (i+1);
            
            gameObjects.Add(null);
            gameObjects[i] = GameObject.Find(name);
            if (gameObjects[i] == null)
            {
                gameObjects[i] = new GameObject(name);
            }
            audioSources.Add(null);
            audioSources[i] = gameObjects[i].GetComponent<AudioSource>();
            if (audioSources[i] == null)
            {
                audioSources[i] = gameObjects[i].AddComponent<AudioSource>();
            }
            audioSources[i].clip = sourceSound;
        }
    }
    public void ApplyParenting(GameObject parent)
    { //Parent all sound sources to the gameobject that has the midi reader
        foreach(GameObject gameObject in gameObjects)
        {
            gameObject.transform.parent = parent.transform;
        }
    }
    public void ApplyVolume(float volume)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = volume;
        }
    }
}
