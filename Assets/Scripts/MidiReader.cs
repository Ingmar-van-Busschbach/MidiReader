using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiReader : MonoBehaviour
{
    [Range(50, 500)]
    public int BPM;
    public float sourceFrequency = 100;
    [Range(-3,3)]
    public int octaveOffset = 0;
    public string file = "C:/Users/Gebruiker/Downloads/riven.mid";
    private int lastNote = 0;
    private AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            return;
        }
        MidiFile midiFile = MidiFile.Read(file);
        if (!midiFile.IsEmpty())
        {
            StartCoroutine(playSong(midiFile));
        }
    }
    IEnumerator playSong(MidiFile midiFile)
    {
        List<Note> notes = new List<Note>();
        foreach (Note note in midiFile.GetNotes())
        {
            notes.Add(note);
        }
        for (int i = 0; i < notes.Count; i++ )
        {
            float timeToWait = notes[i].Time - lastNote;
            yield return new WaitForSeconds(timeToWait / (BPM*8));
            if (timeToWait >= 0.05)
            {
                audioSource.pitch = FrequencyTable.GetFrequency((notes[i].NoteNumber + (octaveOffset * 12))) / sourceFrequency;
                audioSource.Play();
            }
            else if(lastNote >= 0.05)
            {
                audioSource2.pitch = FrequencyTable.GetFrequency((notes[i].NoteNumber + (octaveOffset * 12))) / sourceFrequency;
                audioSource2.Play();
            }
            else
            {
                audioSource3.pitch = FrequencyTable.GetFrequency((notes[i].NoteNumber + (octaveOffset * 12))) / sourceFrequency;
                audioSource3.Play();
            }
            Debug.Log(notes[i]);
            Debug.Log(notes[i].NoteNumber);
            lastNote = (int)notes[i].Time;
        }
    }
}
