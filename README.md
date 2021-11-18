# MidiReader
 
This project was an experiment to implement a non-Unity oriented C# library in Unity and use that to create an interesting editor tool.

#### DryWetMidi
The library I chose was DryWetMidi. This library is a Midi reading and writing library. I used this to make a tool that can open Midi files within Unity and play audio based on its contents.

#### Instruments
The Midi files only contain the data of what notes to play at what time. It does not contain any audio files. As such, I created my own library that can modulate sound files to the right frequency, with a given source frequency and note number input.

[![Demo Video](https://img.youtube.com/vi/PlKLKvDN9Yk/0.jpg)](https://www.youtube.com/watch?v=PlKLKvDN9Yk)
