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
        }

        public void Run() {
            //For now, we just want to make sure we can send Midi events.
            List<int> notes = [];

            for (int i = 36; i <= 84; i++) {
                notes.Add(i);
            }

            string right = string.Join(' ', notes.Select(NoteTranslator.GetSharpNoteName));
            string left = "";

            _lg.SaveOutput(left, right);
        }

        private LilyPondGenerator _lg;
    }
}
