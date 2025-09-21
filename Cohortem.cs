using System;
using System.Collections.Generic;
using System.Linq;

namespace Cohortem {
    /// <summary>
    /// Entry point for Cohortem.
    /// </summary>
    public class Cohortem {
        public Cohortem() {
            _lg = new LilyPondGenerator();
            _r = new Random();
        }

        public void Run() {
            List<int> durations = [1, 2, 4, 8];

            int max = 8;

            //For now, we just want to make sure we can send Midi events.
            List<(int Pitch, int Duration)> rightNotes = [];
            List<(int Pitch, int Duration)> leftNotes = [];

            foreach (var d in GenerateMeasure(max)) {
                rightNotes.Add((_r.Next(60, 72), d));
            }
            foreach (var d in GenerateMeasure(max)) {
                rightNotes.Add((_r.Next(60, 72), d));
            }
            foreach (var d in GenerateMeasure(max)) {
                rightNotes.Add((_r.Next(60, 72), d));
            }

            // for (int i = 36; i <= 60; i++) {
            //     leftNotes.Add((i, durations[_r.Next(0, durations.Count)]));
            // }

            string right = string.Join(' ', rightNotes.Select(NoteTranslator.GetSharpNoteName));
            string left = string.Join(' ', leftNotes.Select(NoteTranslator.GetSharpNoteName));

            _lg.SaveOutput(left, right);
        }

        private List<int> GenerateMeasure(int measureLength) {
            List<int> result = [];
            List<int> durations = [1, 2, 4, 8];
            int currentLength = measureLength;
            do {
                int maxIndex = (int)Math.Log2(currentLength) + 1;
                int duration = durations[_r.Next(0, maxIndex)];
                currentLength -= duration;
                result.Add(measureLength / duration);
            } while (currentLength > 0);

            return result;
        }

        private LilyPondGenerator _lg;
        private Random _r;
    }
}
