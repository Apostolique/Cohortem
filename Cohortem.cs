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

            for (int device = 0; device < MidiOut.NumberOfDevices; device++) {
                string name = MidiOut.DeviceInfo(device).ProductName;
                Console.WriteLine("\t{" + device + "} " + name);
            }
            Console.WriteLine("\nWhich device do you want?");

            while (index < 0 || index >= MidiOut.NumberOfDevices) {
                Int32.TryParse(Console.ReadLine(), out index);
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
            _midiOut.Send(noteOnEvent1.GetAsShortMessage());
            Task.Delay(2000).Wait();
            _midiOut.Send(noteOnEvent1.OffEvent.GetAsShortMessage());
            Task.Delay(200).Wait();
            _midiOut.Send(noteOnEvent2.GetAsShortMessage());
            Task.Delay(2000).Wait();
            _midiOut.Send(noteOnEvent2.OffEvent.GetAsShortMessage());
        }

        public void Dispose() {
            _midiOut.Dispose();
        }
    }
}