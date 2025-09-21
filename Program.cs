using System;

namespace Cohortem {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Starting Cohortem!");
            var cohortem = new Cohortem();
            cohortem.Run();
            Console.WriteLine("Over and out.");
        }
    }
}
