using System;

namespace Cohortem {
    /// <summary>
    /// Goal: Given a midi note, returns the note's name in Lilypond notation.
    /// </summary>
    public static class NoteTranslator {
        private static int middleC = 60;
        private static int higherC = 72;
        private static int octaveRange = 12;
        private static string[] sharpNames = {
            "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b"
        };
        private static string[] flatNames = {
            "c", "des", "d", "ees", "e", "f", "ges", "g", "aes", "a", "bes", "b"
        };

        public static string GetSharpNoteName(int note) {
            return getNoteName(note, sharpNames);
        }
        public static string GetFlatNoteName(int note) {
            return getNoteName(note, flatNames);
        }

        private static string getNoteName(int note, string[] nameSet) {
            int absoluteNote = note - middleC;
            int relativeNote = Math.Abs(absoluteNote) / octaveRange;
            string name = nameSet[note % octaveRange];

            if (note < middleC) {
                for (int i = 0; i < relativeNote + 1; i++) {
                    name += ',';
                }
            } else if (note >= higherC) {
                for (int i = 0; i < relativeNote; i++) {
                    name += '\'';
                }
            }

            return name;
        }
    }
}