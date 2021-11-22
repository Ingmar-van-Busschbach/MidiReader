using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstrumentSettings
{
    [Tooltip("What sound effect to use for this instrument")]
    public AudioClip sourceSound = null;
    [Tooltip("Name of the note. Should always be a capital letter and a number, black notes are always notes as sharp, not flat. Examples: C4, F#2, G7")]
    public string noteName = "C0";
    //[Tooltip("Source frequency in Hz, should be the same as the frequency of the sound in the source sound")]
    [HideInInspector]
    public float sourceFrequency = 100;
    //[Tooltip("If the note number is under this number, it will be played by this instrument. Different Hertz variants of the same instruments can also be used to reduce audio warp. C0 is 0, C1 is 12, and so on.")]
    [HideInInspector]
    public int sourceCutoff = 999;
}
