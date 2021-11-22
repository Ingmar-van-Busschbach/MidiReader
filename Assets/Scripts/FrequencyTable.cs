using System.Collections;
using System.Collections.Generic;

public class FrequencyTable
{ //Static frequency table to translate note numbers into actual sound frequencies
    private static float[] frequencyTable = {
        16.35f, 17.32f, 18.35f, 19.44f, 20.60f, 21.82f, 23.12f, 24.49f, 25.95f, 27.5f, 29.13f, 30.86f, //C0-B1
        32.70f, 34.64f, 36.70f, 38.89f, 41.20f, 43.65f, 46.24f, 48.99f, 51.91f, 55f, 58.27f, 61.73f, //C1-B2
        65.40f, 69.29f, 73.41f, 77.78f, 82.40f, 87.30f, 92.49f, 97.99f, 103.8f, 110f, 116.5f, 123.4f, //C2-B3
        130.8f, 138.5f, 146.8f, 155.5f, 164.8f, 174.6f, 184.9f, 195.9f, 207.6f, 220f, 233.0f, 246.9f, //C3-B4
        261.6f, 277.1f, 293.6f, 311.1f, 329.6f, 349.2f, 369.9f, 391.9f, 415.3f, 440f, 466.1f, 493.8f, //C4-B5
        523.2f, 554.3f, 587.3f, 622.2f, 659.2f, 698.4f, 739.9f, 783.9f, 830.6f, 880f, 932.3f, 987.7f, //C5-B6
        1046f, 1108f, 1174f, 1244f, 1318f, 1396f, 1479f, 1567f, 1661f, 1760f, 1864f, 1975f, //C6-B7 
        2093f, 2217f, 2349f, 2489f, 2637f, 2793f, 2959f, 3135f, 3322f, 3520f, 3729f, 3951f, //C7-B8 
        4186f, 4434f, 4698f, 4978f, 5274f, 5587f, 5919f, 6271f, 6644f, 7040f, 7458f, 7902f}; //C8-B9
    public static float GetFrequency(int noteNumber)
    {
        return frequencyTable[noteNumber];
    }
}
