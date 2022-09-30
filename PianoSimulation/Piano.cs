/**
@author: Amina Turdalieva 
@student id: 2035572
@date: 13-09-2022
@description: This class is Piano containing the wires that are associated with the keyboard keys. 
This class is responsible for sampling and the playing method, that will make the piano produce sound.
*/
using System;
using System.Collections.Generic;

namespace PianoSimulation
{
    public class Piano : IPiano
    {
        public string Keys{get;}
        public int SamplingRate;
        public List<IMusicalString> Wires; 
        public Piano(string keys="q2we4r5ty7u8i9op-[=zxdcfvgbnjmk,.;/' ", int samplingRate=44100)
        {
            Keys = keys;       
            SamplingRate = samplingRate;  
            Wires = new List<IMusicalString>();
            double frequency=0;
            PianoWire pianoWire;
            for (int i=0; i<Keys.Length; i++)
            {
                frequency = Math.Pow(2, (i-24)/12.0) * 440;
                pianoWire = new PianoWire(frequency, SamplingRate);
                Wires.Add(pianoWire);
            }
        }

        public List<string> GetPianoKeys()
        {
            List<string> pianoWiresDescription = new List<string>();
            for (int i=0; i<Keys.Length; i++)
            {
                pianoWiresDescription.Add("Key: " + Keys[i].ToString() + " - frequency: " + Wires[i].NoteFrequency);
            }
            return pianoWiresDescription ;
        } 

        public void StrikeKey(char key)
        {
            int index = Keys.IndexOf(key);
            Wires[index].Strike();
        }

        public double Play()
        {
            double sumSamples=0;
            for (int i=0; i<Wires.Count; i++)
            {
                sumSamples = sumSamples + Wires[i].Sample();           
            }
            return sumSamples;
        }

    }

}