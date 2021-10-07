﻿using System.Collections.Generic;
using Melanchall.DryWetMidi.Core;

namespace Melanchall.DryWetMidi.Tests.Utilities
{
    public sealed class MidiEventEqualityComparer : IEqualityComparer<MidiEvent>
    {
        #region Fields

        private readonly MidiEventEqualityCheckSettings _settings;

        #endregion

        #region Constructor

        public MidiEventEqualityComparer()
            : this(null)
        {
        }

        public MidiEventEqualityComparer(MidiEventEqualityCheckSettings settings)
        {
            _settings = settings ?? new MidiEventEqualityCheckSettings();
        }

        #endregion

        #region IEqualityComparer<MidiEvent>

        public bool Equals(MidiEvent x, MidiEvent y)
        {
            string message;
            return MidiEvent.Equals(x, y, _settings, out message);
        }

        public int GetHashCode(MidiEvent obj)
        {
            return obj.EventType.GetHashCode();
        }

        #endregion
    }
}
