using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commons.Music.Midi;
using System.Linq;

namespace Cohortem {
    /// <summary>
    /// Entry point for Cohortem.
    /// </summary>
    public class Cohortem : IDisposable {
        public Cohortem() {
            int index = -1;

            Console.WriteLine("Available devices:");
            ReadLine.HistoryEnabled = true;

            var access = MidiAccessManager.Default;

            var devices = access.Outputs.ToList();

            for (int i = 0; i < devices.Count; i++) {
                string name = devices[i].Name;
                Console.WriteLine("\t{" + i + "} " + name);
            }

            if (devices.Count == 1) {
                index = 0;
            } else {
                Console.WriteLine("\nWhich device do you want?");
                while (index < 0 || index >= devices.Count) {
                    int.TryParse(ReadLine.Read("?> "), out index);
                }
            }

            if (index >= 0 && index < devices.Count) {
                _midiOut = access.OpenOutputAsync(devices[index].Id).Result;
                _midiOut.Send([MidiEvent.Program, GeneralMidi.Instruments.AcousticGrandPiano], 0, 2, 0);
            } else {
                //Impossible to get here I know, but could be useful some day.
                throw new NoMidiOutException("Couldn't find the preferred MidiOut device.");
            }
        }

        private IMidiOutput _midiOut;

        public void Run() {
            //For now, we just want to make sure we can send Midi events.
            List<(byte Pitch, byte Velocity)> notes = [];

            for (byte i = 60; i <= 72; i++) {
                notes.Add((i, 0x70));
            }

            foreach (var note in notes) {
                PlayNote(MidiEvent.NoteOn, note.Pitch, note.Velocity);
                Task.Delay(2000).Wait();
                PlayNote(MidiEvent.NoteOff, note.Pitch, note.Velocity);
                Task.Delay(200).Wait();
            }
        }

        public void PlayNote(byte eventType, byte value, byte velocity) {
            _midiOut.Send([eventType, value, velocity], 0, 3, 0);
        }

        public void Dispose() {
            _midiOut.CloseAsync();
        }
    }
}