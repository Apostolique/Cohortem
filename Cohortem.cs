using System;
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
            NoteOnEvent noteOnEvent1 = new NoteOnEvent(0, 1, 60, 70, 0);
            NoteOnEvent noteOnEvent2 = new NoteOnEvent(0, 1, 62, 70, 0);

            //TODO: If the MidiOut device went offline, this will cause errors.
            PlayNote(noteOnEvent1);
            Task.Delay(2000).Wait();
            PlayNote(noteOnEvent1.OffEvent);
            Task.Delay(200).Wait();
            PlayNote(noteOnEvent2);
            Task.Delay(2000).Wait();
            PlayNote(noteOnEvent2.OffEvent);
        }

        public void PlayNote(NoteEvent note) {
            _midiOut.Send(note.GetAsShortMessage());
        }

        public void Dispose() {
            _midiOut.Dispose();
        }
    }
}