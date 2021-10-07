﻿using System.Collections.Generic;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Tests.Utilities;
using NUnit.Framework;

namespace Melanchall.DryWetMidi.Tests.Interaction
{
    [TestFixture]
    public sealed partial class TimedObjectUtilitiesTests
    {
        #region Test methods

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_AddEmptyCollection([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new ITimedObject[0],
                expectedMidiEvents: new MidiEvent[0]);
        }

        [Test]
        public void AddObjects_OneBaseEvent_AddEmptyCollection([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new[] { new NoteOnEvent() },
                timedObjectsToAdd: new ITimedObject[0],
                expectedMidiEvents: new[] { new NoteOnEvent() });
        }

        [Test]
        public void AddObjects_MultipleBaseEvents_AddEmptyCollection([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[] { new NoteOnEvent(), new NoteOffEvent { DeltaTime = 100 } },
                timedObjectsToAdd: new ITimedObject[0],
                expectedMidiEvents: new MidiEvent[] { new NoteOnEvent(), new NoteOffEvent { DeltaTime = 100 } });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_TimedEvent_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[] { new TimedEvent(new TextEvent("A")) },
                expectedMidiEvents: new[] { new TextEvent("A") });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_TimedEvent_2([Values] bool toTrackChunk, [Values(0, 100)] long time)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[] { new TimedEvent(new TextEvent("A"), time) },
                expectedMidiEvents: new[] { new TextEvent("A") { DeltaTime = time } });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_TimedEvent_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new TimedEvent(new TextEvent("A")),
                    new TimedEvent(new NoteOnEvent())
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new TextEvent("A"),
                    new NoteOnEvent()
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_TimedEvent_2([Values] bool toTrackChunk, [Values(0, 100)] long time1, [Values(350, 250)] long time2)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new TimedEvent(new TextEvent("A"), time1),
                    new TimedEvent(new NoteOnEvent(), time2),
                    new TimedEvent(new NoteOffEvent(), time2)
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new TextEvent("A") { DeltaTime = time1 },
                    new NoteOnEvent { DeltaTime = time2 - time1 },
                    new NoteOffEvent()
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_TimedEvent_3([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new TimedEvent(new TextEvent("A"), 100),
                    new TimedEvent(new NoteOnEvent(), 20)
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent { DeltaTime = 20 },
                    new TextEvent("A") { DeltaTime = 80 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Note_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[] { new Note((SevenBitNumber)70) },
                expectedMidiEvents: new MidiEvent[] { new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity), new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Note_2([Values] bool toTrackChunk, [Values(0, 100)] long time, [Values(0, 35)] long length)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[] { new Note((SevenBitNumber)70, length, time) },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = time },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = length }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)50),
                    new Note((SevenBitNumber)40) { Velocity = (SevenBitNumber)70, OffVelocity = (SevenBitNumber)30 }
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)50, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)50, SevenBitNumber.MinValue),
                    new NoteOnEvent((SevenBitNumber)40, (SevenBitNumber)70),
                    new NoteOffEvent((SevenBitNumber)40, (SevenBitNumber)30),
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_2(
            [Values] bool toTrackChunk,
            [Values(0, 100)] long time1,
            [Values(0, 100)] long length1,
            [Values(350, 200)] long time2,
            [Values(350, 250)] long length2)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, length1, time1),
                    new Note((SevenBitNumber)60, length2, time2) { Channel = (FourBitNumber)7 },
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = time1 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = length1 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { Channel = (FourBitNumber)7, DeltaTime = time2 - (time1 + length1) },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { Channel = (FourBitNumber)7, DeltaTime = length2 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_3([Values] bool toTrackChunk)
        {
            // |====|
            //   |====|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, 100, 0),
                    new Note((SevenBitNumber)60, 200, 50),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 150 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_4([Values] bool toTrackChunk)
        {
            // |====|
            //   |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, 100, 0),
                    new Note((SevenBitNumber)60, 50, 50),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue)
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_5([Values] bool toTrackChunk)
        {
            // |====|
            //  |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, 100, 0),
                    new Note((SevenBitNumber)60, 40, 50),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 40 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 10 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_6([Values] bool toTrackChunk)
        {
            //   |====|
            // |====|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, 200, 50),
                    new Note((SevenBitNumber)60, 100, 0),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 150 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_7([Values] bool toTrackChunk, [Values(0, 50, 99)] long length)
        {
            //     |====|
            // |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, 200, 100),
                    new Note((SevenBitNumber)60, length, 0),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = length },
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 100 - length },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 200 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Note_8([Values] bool toTrackChunk)
        {
            //    |====|
            // |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Note((SevenBitNumber)80, 200, 100),
                    new Note((SevenBitNumber)60, 100, 0),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 100 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 200 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_OneNote_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)70))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue)
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_OneNote_2([Values] bool toTrackChunk, [Values(0, 100)] long time, [Values(0, 35)] long length)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)70, length, time))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = time },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = length }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)50)),
                    new Chord(new Note((SevenBitNumber)40) { Velocity = (SevenBitNumber)70, OffVelocity = (SevenBitNumber)30 })
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)50, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)50, SevenBitNumber.MinValue),
                    new NoteOnEvent((SevenBitNumber)40, (SevenBitNumber)70),
                    new NoteOffEvent((SevenBitNumber)40, (SevenBitNumber)30),
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_2(
            [Values] bool toTrackChunk,
            [Values(0, 100)] long time1,
            [Values(0, 100)] long length1,
            [Values(350, 200)] long time2,
            [Values(350, 250)] long length2)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, length1, time1)),
                    new Chord(new Note((SevenBitNumber)60, length2, time2) { Channel = (FourBitNumber)7 }),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = time1 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = length1 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { Channel = (FourBitNumber)7, DeltaTime = time2 - (time1 + length1) },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { Channel = (FourBitNumber)7, DeltaTime = length2 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_3([Values] bool toTrackChunk)
        {
            // |====|
            //   |====|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, 100, 0)),
                    new Chord(new Note((SevenBitNumber)60, 200, 50)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 150 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_4([Values] bool toTrackChunk)
        {
            // |====|
            //   |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, 100, 0)),
                    new Chord(new Note((SevenBitNumber)60, 50, 50)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue)
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_5([Values] bool toTrackChunk)
        {
            // |====|
            //  |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, 100, 0)),
                    new Chord(new Note((SevenBitNumber)60, 40, 50)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 40 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 10 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_6([Values] bool toTrackChunk)
        {
            //   |====|
            // |====|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, 200, 50)),
                    new Chord(new Note((SevenBitNumber)60, 100, 0)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 150 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_7([Values] bool toTrackChunk, [Values(0, 50, 99)] long length)
        {
            //     |====|
            // |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, 200, 100)),
                    new Chord(new Note((SevenBitNumber)60, length, 0)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = length },
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 100 - length },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 200 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_OneNote_8([Values] bool toTrackChunk)
        {
            //    |====|
            // |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(new Note((SevenBitNumber)80, 200, 100)),
                    new Chord(new Note((SevenBitNumber)60, 100, 0)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 100 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 200 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)50),
                        new Note((SevenBitNumber)40) { Velocity = (SevenBitNumber)70, OffVelocity = (SevenBitNumber)30 })
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)50, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)50, SevenBitNumber.MinValue),
                    new NoteOnEvent((SevenBitNumber)40, (SevenBitNumber)70),
                    new NoteOffEvent((SevenBitNumber)40, (SevenBitNumber)30),
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_2(
            [Values] bool toTrackChunk,
            [Values(0, 100)] long time1,
            [Values(0, 100)] long length1,
            [Values(350, 200)] long time2,
            [Values(350, 250)] long length2)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, length1, time1),
                        new Note((SevenBitNumber)60, length2, time2) { Channel = (FourBitNumber)7 }),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = time1 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = length1 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { Channel = (FourBitNumber)7, DeltaTime = time2 - (time1 + length1) },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { Channel = (FourBitNumber)7, DeltaTime = length2 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_3([Values] bool toTrackChunk)
        {
            // |====|
            //   |====|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 200, 50)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 150 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_4([Values] bool toTrackChunk)
        {
            // |====|
            //   |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 50, 50)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue)
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_5([Values] bool toTrackChunk)
        {
            // |====|
            //  |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 40, 50)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 40 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 10 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_6([Values] bool toTrackChunk)
        {
            //   |====|
            // |====|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 200, 50),
                        new Note((SevenBitNumber)60, 100, 0)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 150 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_OneObject_Chord_MultipleNotes_7([Values] bool toTrackChunk, [Values(0, 50, 100)] long length)
        {
            //    |====|
            // |==|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 200, 100),
                        new Note((SevenBitNumber)60, length, 0)),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity),
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = length },
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 100 - length },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 200 }
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_MultipleNotes_1([Values] bool toTrackChunk, [Values(100, 150)] long time)
        {
            // |====|
            // |  ==|
            //      |===|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 50, 50)),
                    new Chord(
                        new Note((SevenBitNumber)70, 100, time))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = time - 100 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 100 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_MultipleNotes_2([Values] bool toTrackChunk, [Values(40, 50)] long length)
        {
            // |======|
            // |  ====|
            //    |===|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 50, 50)),
                    new Chord(
                        new Note((SevenBitNumber)70, length, 60))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 40 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 60 + length - 100 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_MultipleNotes_3([Values] bool toTrackChunk, [Values(40, 50)] long length)
        {
            // |======|
            // |  ====|
            //  |===  |
            //  |  ===|

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 50, 50)),
                    new Chord(
                        new Note((SevenBitNumber)75, 40, 30),
                        new Note((SevenBitNumber)70, length, 60))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity) { DeltaTime = 30 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 20 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 30 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 60 + length - 100 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Chord_MultipleNotes_4([Values] bool toTrackChunk, [Values(40, 50)] long length)
        {
            //    |======  |
            //    |  ======|
            // |=============|
            // |       =     |

            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 50, 10),
                        new Note((SevenBitNumber)60, 50, 20)),
                    new Chord(
                        new Note((SevenBitNumber)75, 100, 0),
                        new Note((SevenBitNumber)70, 10, 30))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 20 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 30 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Mixed_1([Values] bool toTrackChunk, [Values(100, 150)] long time)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new ITimedObject[]
                {
                    new Note((SevenBitNumber)80, 100, 0),
                    new Chord(
                        new Note((SevenBitNumber)60, 50, 50)),
                    new Chord(
                        new Note((SevenBitNumber)70, 100, time))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 50 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = time - 100 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 100 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Mixed_2([Values] bool toTrackChunk, [Values(40, 50)] long length)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new ITimedObject[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 100, 0),
                        new Note((SevenBitNumber)60, 50, 50)),
                    new TimedEvent(new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity), 60),
                    new TimedEvent(new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue), 60 + length),
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 50 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 40 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 60 + length - 100 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Mixed_3([Values] bool toTrackChunk, [Values(40, 50)] long length)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new ITimedObject[]
                {
                    new TimedEvent(new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity)),
                    new TimedEvent(new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue), 100),
                    new Chord(
                        new Note((SevenBitNumber)60, 50, 50)),
                    new Note((SevenBitNumber)75, 40, 30),
                    new Note((SevenBitNumber)70, length, 60)
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity) { DeltaTime = 30 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 20 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 30 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue),
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 60 + length - 100 },
                });
        }

        [Test]
        public void AddObjects_EmptyBaseEventsCollection_MultipleObjects_Mixed_4([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new MidiEvent[0],
                timedObjectsToAdd: new ITimedObject[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 50, 10)),
                    new Note((SevenBitNumber)60, 50, 20),
                    new TimedEvent(new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity)),
                    new TimedEvent(new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue), 100),
                    new Chord(
                        new Note((SevenBitNumber)70, 10, 30))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 20 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 30 },
                });
        }

        [Test]
        public void AddObjects_OneBaseEvent_MultipleObjects_Mixed_1([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new[] { new NoteOnEvent() },
                timedObjectsToAdd: new ITimedObject[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 50, 10)),
                    new Note((SevenBitNumber)60, 50, 20),
                    new TimedEvent(new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity)),
                    new TimedEvent(new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue), 100),
                    new Chord(
                        new Note((SevenBitNumber)70, 10, 30))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent(),
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 20 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 30 },
                });
        }

        [Test]
        public void AddObjects_OneBaseEvent_MultipleObjects_Mixed_2([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new[] { new NoteOnEvent { DeltaTime = 10 } },
                timedObjectsToAdd: new ITimedObject[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 50, 10)),
                    new Note((SevenBitNumber)60, 50, 20),
                    new TimedEvent(new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity)),
                    new TimedEvent(new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue), 100),
                    new Chord(
                        new Note((SevenBitNumber)70, 10, 30))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity),
                    new NoteOnEvent { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 20 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 30 },
                });
        }

        [Test]
        public void AddObjects_OneBaseEvent_MultipleObjects_Mixed_3([Values] bool toTrackChunk)
        {
            AddObjects(
                toTrackChunk,
                baseMidiEvents: new[] { new NoteOnEvent { DeltaTime = 200 } },
                timedObjectsToAdd: new ITimedObject[]
                {
                    new Chord(
                        new Note((SevenBitNumber)80, 50, 10)),
                    new Note((SevenBitNumber)60, 50, 20),
                    new TimedEvent(new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity)),
                    new TimedEvent(new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue), 100),
                    new Chord(
                        new Note((SevenBitNumber)70, 10, 30))
                },
                expectedMidiEvents: new MidiEvent[]
                {
                    new NoteOnEvent((SevenBitNumber)75, Note.DefaultVelocity),
                    new NoteOnEvent((SevenBitNumber)80, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)60, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOnEvent((SevenBitNumber)70, Note.DefaultVelocity) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)70, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)80, SevenBitNumber.MinValue) { DeltaTime = 20 },
                    new NoteOffEvent((SevenBitNumber)60, SevenBitNumber.MinValue) { DeltaTime = 10 },
                    new NoteOffEvent((SevenBitNumber)75, SevenBitNumber.MinValue) { DeltaTime = 30 },
                    new NoteOnEvent { DeltaTime = 100 },
                });
        }

        #endregion

        #region Private methods

        private void AddObjects(
            bool toTrackChunk,
            IEnumerable<MidiEvent> baseMidiEvents,
            IEnumerable<ITimedObject> timedObjectsToAdd,
            IEnumerable<MidiEvent> expectedMidiEvents)
        {
            if (toTrackChunk)
            {
                var trackChunk = new TrackChunk(baseMidiEvents);
                trackChunk.AddObjects(timedObjectsToAdd);
                MidiAsserts.AreEqual(new TrackChunk(expectedMidiEvents), trackChunk, true, "Events are invalid.");
            }
            else
            {
                var eventsCollection = new EventsCollection();
                eventsCollection.AddRange(baseMidiEvents);
                eventsCollection.AddObjects(timedObjectsToAdd);

                var expectedEventsCollection = new EventsCollection();
                expectedEventsCollection.AddRange(expectedMidiEvents);

                MidiAsserts.AreEqual(expectedEventsCollection, eventsCollection, true, "Events are invalid.");
            }
        }

        #endregion
    }
}
