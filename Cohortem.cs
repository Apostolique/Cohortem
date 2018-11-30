using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NAudio.Midi;

namespace Cohortem {
    /// <summary>
    /// Goal: Entry point for Cohortem.
    /// </summary>
    public class Cohortem : IDisposable {
        public Cohortem() {
            int index = -1;

            Console.WriteLine("Available devices:");
            ReadLine.HistoryEnabled = true;

            for (int device = 0; device < MidiOut.NumberOfDevices; device++) {
                string name = MidiOut.DeviceInfo(device).ProductName;
                Console.WriteLine("\t{" + device + "} " + name);
            }
            Console.WriteLine("\nWhich device do you want?");

            while (index < 0 || index >= MidiOut.NumberOfDevices) {
                Int32.TryParse(ReadLine.Read("?> "), out index);
            }

            if (index >= 0 && index < MidiOut.NumberOfDevices) {
                _midiOut = new MidiOut(index);
            } else {
                //Impossible to get here I know, but could be useful some day.
                throw new NoMidiOutException("Couldn't find the preferred MidiOut device.");
            }
        }

        private MidiOut _midiOut;

        public void Run() {
            //For now, we just want to make sure we can send Midi events.
            List<NoteOnEvent> notes = new List<NoteOnEvent>();

            for (int i = 60; i <= 72; i++) {
                notes.Add(new NoteOnEvent(0, 1, i, 70, 0));
            }

            foreach (NoteOnEvent note in notes) {
                PlayNote(note);
                Task.Delay(2000).Wait();
                PlayNote(note.OffEvent);
                Task.Delay(200).Wait();
            }
        }

        public void PlayNote(NoteEvent note) {
            _midiOut.Send(note.GetAsShortMessage());
        }

        public void Dispose() {
            _midiOut.Dispose();
        }
    }
}