using System;
using PianoSimulation;

namespace InteractivePiano
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new InteractivePiano())
                game.Run();

        }
    }
}
