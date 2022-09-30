using Microsoft.VisualStudio.TestTools.UnitTesting;
using PianoSimulation;
using System;

namespace PianoSimulationTests
{
    [TestClass]
    public class PianoSimulationTests
    {
        [TestMethod]
        public void TestCircularArrayBuffer()
        {
            var buffer = new CircularArray(5);
            double[] arrayDoubles = {0.1, 0.2, 0.3}; 

            //Testing the buffer creation (constructor)
            buffer.Fill(arrayDoubles);
            Assert.AreEqual(3, buffer.Length);
            Assert.AreEqual(0, buffer.FrontIndex);
            Assert.AreEqual(0.1, buffer[0]);


            //Testing the buffer shifting and indexing
            Assert.AreEqual(0.1, buffer.Shift(0));
                // 0 , 0.2 , 0.3
            Assert.AreEqual(1, buffer.FrontIndex);
            Assert.AreEqual(0.2, buffer[0]);
            Assert.AreEqual(0.3, buffer[1]);
            Assert.AreEqual(0, buffer[2]);
            Assert.AreEqual(0.2, buffer.Shift(0.4));
                // 0 , 0.4 , 0.3
            Assert.AreEqual(2, buffer.FrontIndex);
            Assert.AreEqual(0.3, buffer[0]);
            Assert.AreEqual(0, buffer[1]);
            Assert.AreEqual(0.4, buffer[2]);

            //Testing to see if shifting more times than the array length will work and go back to the first index
            buffer.Shift(0.5);
                // 0 , 0.4 , 0.5
            Assert.AreEqual(0, buffer[0]);
            Assert.AreEqual(0.4, buffer[1]);
            Assert.AreEqual(0.5, buffer[2]);

        }

        [TestMethod]
        public void TestPianoWire()
        {
            var pianoWire = new PianoWire(440, 44100); //frequency, sample rate
            //Testing the piano wire creation (constructor)
            Assert.AreEqual(440, pianoWire.NoteFrequency);
            Assert.AreEqual(100, pianoWire.NumberOfSamples);
            Assert.AreEqual(100, pianoWire.CircularArray.Length);


            //Testing the strike method, making sure the values are within the needed range (between -0.5 and 0.5)
            pianoWire.Strike();
            bool inRange=false;
            for (int i=0; i<pianoWire.CircularArray.Length; i++)
            {
                 if (pianoWire.CircularArray[i] >= -0.5 || pianoWire.CircularArray[i] <= 0.5)
                 {
                    inRange = true;
                 }
            }        
            Assert.IsTrue(inRange); 

            //Testing the sample method
            double firstValue = pianoWire.CircularArray[0];
            Assert.AreEqual(firstValue, pianoWire.Sample());  
        }

        [TestMethod]
        public void TestPiano()
        {
            var piano = new Piano();
            //Testing the creation of wires, making sure they correspond to keys
            Assert.AreEqual(piano.Keys.Length, piano.Wires.Count); 
            Assert.AreEqual(110, piano.Wires[0].NoteFrequency); 

            //Testing to see if the wires description is the same length as the number of keys
            Assert.AreEqual(piano.Keys.Length, piano.GetPianoKeys().Count); 
                   
        }
    }
}
