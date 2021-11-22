using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class MidiReader : MonoBehaviour
{
    public string file = "C:/Users/Gebruiker/Downloads/riven.mid";

    [Range(50, 500)]
    [SerializeField] private int BPM = 1;
    [Tooltip("Playback speed, pitch and volume settings")]
    [SerializeField] private PlaybackSettings playbackSettings;
    [Tooltip("Sound source sampling settings")]
    [SerializeField] private ChannelSettings channelSettings;
    [Tooltip("Whether to read BPM data from the midi file. Some midi files may not contain such data, in which case this should be disabled")]
    [SerializeField] private bool automaticBPMUpdate = true;
    [SerializeField] private bool debug = true;
    [Tooltip("Is required to be on if you swap from an instrument with multiple sources to an instrument with a single source. Otherwise, can be disabled")]
    [SerializeField] private bool alwaysUseSourceSeparation = true;
    private int lastNote = 0;
    private float volume = 1f;
    private static int defaultOffset = -12;
    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (debug) { Debug.Log("Initializing Variables..."); }
        //Catch null variables
        if (playbackSettings.volume == 0) { playbackSettings.volume = 1; }
        if (playbackSettings.playbackSpeed == 0) { playbackSettings.playbackSpeed = 1; }
        if (playbackSettings.pitch == 0) { playbackSettings.pitch = 1; }
        if (BPM == 0) { BPM = 1; }
        if (debug) { Debug.Log("Loading Instruments..."); }
        if (channelSettings.instrument.instrumentSettings.Count < 1 || channelSettings.instrument == null)
        {
            Debug.Log("Please assign an instrument!");
            return;
        }
        //Generate audio sources
        channelSettings.GenerateChannels();
        channelSettings.ApplyParenting(this.gameObject);
        channelSettings.ApplyVolume(playbackSettings.volume);
        if (debug) { Debug.Log("Loading Midi File..."); }
        //Load midi file
        MidiFile midiFile = MidiFile.Read(file);
        if (!midiFile.IsEmpty())
        { //Start playing song
            if (debug) { Debug.Log("Starting Song Playback"); }
            StartCoroutine(playSong(midiFile));
        }
    }
    IEnumerator playSong(MidiFile midiFile)
    { //Load BPM info from tempoMap and add the notes to play into a list
        TempoMap tempoMap = midiFile.GetTempoMap();
        List<Note> notes = new List<Note>();

        foreach (Note note in midiFile.GetNotes())
        {
            notes.Add(note);
        }
        // Note playback is done in a for loop that gets delayed between notes, which is much less resource intensive than a per frame based solution
        for (int i = 0; i < notes.Count; i++ )
        { 
            //Cycle through all sound sources so no sound overlapping/cutoff occurs.
            AudioSource audioSource = channelSettings.audioSources[i % channelSettings.audioSources.Count];
            
            //Wait for the next note
            float timeToWait = notes[i].Time - lastNote;
            yield return new WaitForSeconds(timeToWait / (BPM * 8 * playbackSettings.playbackSpeed));

            //Every time the BPM changes in the midi file, it automatically gets updated in the player
            var time = new MidiTimeSpan(notes[i].Time);
            Tempo tempo = tempoMap.GetTempoAtTime(time);
            if (BPM != (int)tempo.BeatsPerMinute && automaticBPMUpdate)
            {
                BPM = (int)tempo.BeatsPerMinute;
                if (debug) { Debug.Log("BPM Change: " + BPM + "BPM"); }
            }
            if (playbackSettings.volume != volume)
            {
                channelSettings.ApplyVolume(playbackSettings.volume);
                volume = playbackSettings.volume;
            }
            if(channelSettings.instrument.instrumentSettings.Count > 1 || alwaysUseSourceSeparation)
            { //Source seperation is the use of multiple source audio files to reduce audio stretching when a source audio is pitched to very high or low pitches, improving audio quality.
                channelSettings.ApplySourceSeparation(notes[i].NoteNumber + channelSettings.noteOffset + defaultOffset, audioSource);
            }
            //Get the pitch for the next note, and play it.
            audioSource.pitch = FrequencyTable.GetFrequency((notes[i].NoteNumber + channelSettings.noteOffset + defaultOffset)) / channelSettings.sourceFrequency * playbackSettings.pitch;
            audioSource.Play();
            if (debug) { Debug.Log(notes[i]); }
            lastNote = (int)notes[i].Time;
        }
        if (debug) { Debug.Log("Song Finished."); }
    }
}
