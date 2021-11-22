using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaybackSettings
{
    [Range(0.01f, 1)]
    public float volume = 0.3f;
    [Range(0.3f, 3)]
    public float playbackSpeed = 1f;
    [Range(0.3f, 3)]
    public float pitch = 1f;
}
