using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChannelSettings
{
    public Instrument instrument = null;
    [Range(-12, 12)]
    [Tooltip("Non-runtime-editable and special variable. Should generally be 1-6 depending on the source instrument, but should only be adjusted if you know what you are doing")]
    public int instrumentOffset = 0;
    [Range(-36, 36)]
    [Tooltip("Similar to the pitch setting, but using notes instead of percentages")]
    public int noteOffset = 0;
    [Tooltip("Non-runtime-editable. The amount of Audio Sources the midi reader will utulize. More channels means more CPU cost, but having the ability to play more notes at the same time or in short succession.")]
    public int channels = 25;
    [HideInInspector]
    public float sourceFrequency = 100;
    [HideInInspector]
    public List<AudioSource> audioSources;
    [HideInInspector]
    public List<GameObject> gameObjects;

    public void GenerateChannels()
    { //Generate audio sources to play the midi audio through
        audioSources = new List<AudioSource>();
        gameObjects = new List<GameObject>();
        if (instrument.instrumentSettings[0].sourceSound == null)
        {
            Debug.Log("Please assign an instrument!");
            return;
        }
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
            audioSources[i].clip = instrument.instrumentSettings[0].sourceSound;
        }
        for (int i = 0; i < instrument.instrumentSettings.Count; i++)
        {
            instrument.instrumentSettings[i].sourceFrequency = FrequencyTable.GetFrequency(NoteTable.FindNote(instrument.instrumentSettings[i].noteName));
            instrument.instrumentSettings[i].sourceCutoff = NoteTable.FindNote(instrument.instrumentSettings[i].noteName)+instrumentOffset;
            if(i == instrument.instrumentSettings.Count - 1) { instrument.instrumentSettings[i].sourceCutoff = 999; }
        }
    }
    public void ApplySourceSeparation(int noteNumber, AudioSource audioSource)
    {
        for (int i = 0; i < instrument.instrumentSettings.Count; i++)
        {
            int index = instrument.instrumentSettings.Count - i - 1;
            if (noteNumber < instrument.instrumentSettings[index].sourceCutoff)
            {
                audioSource.clip = instrument.instrumentSettings[index].sourceSound;
                sourceFrequency = instrument.instrumentSettings[index].sourceFrequency;
            }
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
