using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using CrypticWizard.RandomWordGenerator;
using Scriban;
using static CrypticWizard.RandomWordGenerator.WordGenerator;

namespace Cohortem {
    public class LilypondGenerator {
        public LilypondGenerator() {
            WordGenerator w = new();

            _title = GenerateTitle(w);
        }
        public void SaveOutput(string right, string left) {
            string LilyPondTemplate = Template.Parse(File.ReadAllText("./LilypondTemplate/Template.ly")).Render(new { Title = _title, Author = "Cohortem", Year = DateTime.Now.Year, Right = right, Left = left });

            Directory.CreateDirectory("./Output/");
            File.WriteAllText("./Output/Test.ly", LilyPondTemplate);
            File.WriteAllText("./Output/definitions.ly", File.ReadAllText("./LilyPondTemplate/definitions.ly"));

            Process.Start(new ProcessStartInfo("lilypond", "Test.ly") { UseShellExecute = true, WorkingDirectory = "./Output", WindowStyle = ProcessWindowStyle.Hidden });
        }

        private static string GenerateTitle(WordGenerator w) {
            string name = w.GetPattern([PartOfSpeech.adj, PartOfSpeech.adj, PartOfSpeech.noun], ' ');
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }

        private string _title;
    }
}
