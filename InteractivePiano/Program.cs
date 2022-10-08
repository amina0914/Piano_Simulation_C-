using System;
using PianoSimulation;

namespace InteractivePiano
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var audio = Audio.Instance)
            {        
                using (var game = new InteractivePiano(audio))
                    game.Run();
            }
        }
    }
}
