﻿using Melanchall.DryWetMidi.Interaction;

namespace Melanchall.DryWetMidi.Devices
{
    /// <summary>
    /// Settings according to which an instance of the <see cref="Playback"/> should be created.
    /// </summary>
    public sealed class PlaybackSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets settings of the internal playback's clock.
        /// </summary>
        public MidiClockSettings ClockSettings { get; set; }

        /// <summary>
        /// Gets or sets settings which define how notes should be detected and built internally
        /// by the <see cref="Playback"/>.
        /// </summary>
        public NoteDetectionSettings NoteDetectionSettings { get; set; }

        #endregion
    }
}
