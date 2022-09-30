/**
@author: Amina Turdalieva 
@student id: 2035572
@date: 13-09-2022
@description: This class is PianoWire that creates the Circular Array and fills it with randoms between - 0.5 and 0.5.
*/

using System;

namespace PianoSimulation
{
    public class PianoWire : IMusicalString
    {
        public CircularArray CircularArray;
        public double NoteFrequency {get;}
        public int NumberOfSamples {get;}

        public PianoWire(double frequency, double sampleRate)
        {
            NumberOfSamples = (int)Math.Round(sampleRate/frequency);
            CircularArray = new CircularArray(NumberOfSamples);
            NoteFrequency = frequency;
        }

        public void Strike()
        {
            Random rand = new Random();
            double[] arrayRandoms = new double[NumberOfSamples];
            for (int i=0; i<arrayRandoms.Length; i++)
            {
                arrayRandoms[i] = (rand.NextDouble() * (0.5 - (-0.5)) + (-0.5));
            }
            CircularArray.Fill(arrayRandoms);
        }

        /**The shift method in circular array updates the index, and the indexing in circular array gets the value at the right position.
        This is what allows to use index at position 0 and position 1 in sample method. 
        */
        public double Sample(double decay=0.996){
            double average = (CircularArray[0] + CircularArray[1]) * 0.5;
            return CircularArray.Shift(average*decay);
        }
    }
}