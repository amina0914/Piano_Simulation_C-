/**
@author: Amina Turdalieva 
@student id: 2035572
@date: 13-09-2022
@description: This class is the circular array representing the buffer. 
There is a font index keeping track of the first position in the buffer.
*/

using System;

namespace PianoSimulation
{
    public class CircularArray : IRingBuffer
    {
        private double[] _buffer;
        public int FrontIndex;

        public CircularArray(int lengthOfArray)
        {
            _buffer = new double[lengthOfArray]; 
            FrontIndex = 0;
        }

        public int Length
        {
            get
            {
                return _buffer.Length;
            }
        }

        public double Shift(double value)
        {
            double firstEl = _buffer[FrontIndex%Length];
            _buffer[FrontIndex%Length] = value;
            FrontIndex ++;
            return firstEl; 
        }

        public double this[int i]
        {
            get
            {   
                return _buffer[(i+FrontIndex)%Length];
            }
        }

        public void Fill(double[] array)
        {
            _buffer = array;
        }

    }
}