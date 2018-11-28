using System;

namespace Cohortem {
    public class NoMidiOutException : Exception {
        public NoMidiOutException() { }
        public NoMidiOutException(string msg) : base(msg) { }
    }
}