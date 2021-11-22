using System.Collections;
using System.Collections.Generic;

public class NoteTable
{
    private static string[] noteTable = {
        "C0", "C#0", "D0", "D#0", "E0", "F0", "F#0", "G0", "G#0", "A1", "A#1", "B1", //C0-B1
        "C1", "C#1", "D1", "D#1", "E1", "F1", "F#1", "G1", "G#1", "A2", "A#2", "B2", //C1-B2
        "C2", "C#2", "D2", "D#2", "E2", "F2", "F#2", "G2", "G#2", "A3", "A#3", "B3", //C2-B3
        "C3", "C#3", "D3", "D#3", "E3", "F3", "F#3", "G3", "G#3", "A4", "A#4", "B4", //C3-B4
        "C4", "C#4", "D4", "D#4", "E4", "F4", "F#4", "G4", "G#4", "A5", "A#5", "B5", //C4-B5
        "C5", "C#5", "D5", "D#5", "E5", "F5", "F#5", "G5", "G#5", "A6", "A#6", "B6", //C5-B6
        "C6", "C#6", "D6", "D#6", "E6", "F6", "F#6", "G6", "G#6", "A7", "A#7", "B7", //C6-B7 
        "C7", "C#7", "D7", "D#7", "E7", "F7", "F#7", "G7", "G#7", "A8", "A#8", "B8", //C7-B8 
        "C8", "C#8", "D8", "D#8", "E8", "F8", "F#8", "G8", "G#8", "A9", "A#9", "B9"}; //C8-B9
    public static int FindNote(string noteName)
    {
        int returnValue = -1;
        for(int i = 0; i < noteTable.Length; i++)
        {
            if(noteName == noteTable[i])
            {
                returnValue = i;
            }
        }
        return returnValue;
    }
}
