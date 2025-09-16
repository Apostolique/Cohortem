using System;

namespace Cohortem {
    /// <summary>
    /// Given a midi note, returns the note's name in Lilypond notation.
    /// </summary>
    public static class NoteTranslator {
        private static int MiddleC = 60;
        private static int HigherC = 72;
        private static int OctaveRange = 12;
        private static string[] SharpNames = {
            "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b"
        };
        private static string[] FlatNames = {
            "c", "des", "d", "ees", "e", "f", "ges", "g", "aes", "a", "bes", "b"
        };

        public static string GetSharpNoteName(int note) {
            return GetNoteName(note, SharpNames);
        }
        public static string GetFlatNoteName(int note) {
            return GetNoteName(note, FlatNames);
        }

        private static string GetNoteName(int note, string[] nameSet) {
            int absoluteNote = note - MiddleC;
            int relativeNote = Math.Abs(absoluteNote) / OctaveRange;
            string name = nameSet[note % OctaveRange];

            if (note < MiddleC) {
                for (int i = 0; i < relativeNote + 1; i++) {
                    name += ',';
                }
            } else if (note >= HigherC) {
                for (int i = 0; i < relativeNote; i++) {
                    name += '\'';
                }
            }

            return name;
        }
    }
}