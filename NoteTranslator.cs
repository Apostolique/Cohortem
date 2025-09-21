using System;

namespace Cohortem {
    /// <summary>
    /// Given a midi note, returns the note's name in Lilypond notation.
    /// </summary>
    public static class NoteTranslator {
        private static int MiddleC = 60;
        private static int OctaveRange = 12;
        private static string[] SharpNames = {
            "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b"
        };
        private static string[] FlatNames = {
            "c", "des", "d", "ees", "e", "f", "ges", "g", "aes", "a", "bes", "b"
        };

        public static string GetSharpNoteName((int Pitch, int Duration) note) {
            return GetNoteName(note, SharpNames);
        }
        public static string GetFlatNoteName((int Pitch, int Duration) note) {
            return GetNoteName(note, FlatNames);
        }

        private static string GetNoteName((int Pitch, int Duration) note, string[] nameSet) {
            int absoluteNote = note.Pitch - MiddleC + OctaveRange;
            int relativeNote = Math.Abs((int)Math.Floor(absoluteNote / (double)OctaveRange));
            string name = nameSet[note.Pitch % OctaveRange];

            if (note.Pitch < MiddleC - OctaveRange) {
                for (int i = 0; i < relativeNote; i++) {
                    name += ',';
                }
            } else if (note.Pitch >= MiddleC) {
                for (int i = 0; i < relativeNote; i++) {
                    name += '\'';
                }
            }

            return $"{name}{note.Duration}";
        }
    }
}
