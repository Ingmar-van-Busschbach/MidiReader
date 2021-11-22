using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instrument", menuName = "ScriptableObjects/Instrument", order = 1)]
public class Instrument : ScriptableObject
{
    [Tooltip("Set up the sound sources here. Each source should be marked with a note name that you should put into the note name field. Lower numbers go up top in the list.")]
    public List<InstrumentSettings> instrumentSettings;
}
