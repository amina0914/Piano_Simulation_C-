/**
@author: Amina Turdalieva 
@student id: 2035572
@date: 08-10-2022
@description: This the main class that runs the program. 
It created an audio instance and insures that the audio is properly disposed at the end.
*/

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
