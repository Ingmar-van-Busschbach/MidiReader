using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ParsedMidi
{
     public int? ProgramNumber { get; set; }
     public long Time { get; set; }
     public long Length { get; set; }
     public int NoteNumber { get; set; }
}

