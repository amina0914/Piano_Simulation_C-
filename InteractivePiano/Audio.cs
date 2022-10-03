﻿using System;
using NAudio.Wave;

namespace InteractivePiano
{
    /// <summary>
    /// This class is used to play a stream of doubles that represent audio samples
    /// </summary>
    public sealed class Audio
    {
        private static readonly Audio instance = new Audio();
        private WaveOutEvent _waveOut;  //audio output in separate thread
        private WaveFormat _waveFormat;
        private BufferedWaveProvider _bufferedWaveProvider;  //used for streaming audio

        private int _bufferCount = 0;
        private byte[] _buffer;



        /// <summary>
        /// Audio constructor
        /// </summary>
        /// <param name="bufferSize">Length of buffer held in this class, default is 4096</param>
        /// <param name="samplingRate">Audio sampling rate,, default value is 44100</param>
        
        static Audio()
        {
        }
        private Audio(int bufferSize = 4096 * 16, int samplingRate = 44100)
        {
            _waveOut = new WaveOutEvent();
            _waveFormat = new WaveFormat(samplingRate, 16, 1);
            _bufferedWaveProvider = new BufferedWaveProvider(_waveFormat);
            //Let NAudio decide the buffer length
            //_bufferedWaveProvider.BufferLength = bufferSize; //Why * 16? 
            _bufferedWaveProvider.DiscardOnBufferOverflow = true;
            _buffer = new byte[bufferSize];

            _waveOut.Init(_bufferedWaveProvider);
            _waveOut.Play();
        }

        public static Audio Instance
        {
            get 
            {
                instance.Reset();
                return instance;
            }
        }

        private void Reset()
        {
            _bufferCount = 0;
            _bufferedWaveProvider.ClearBuffer();
        }

        /// <summary>
        /// Used to play a double representing an audio sample. The double will be added to the buffer
        /// </summary>
        /// <param name="input">Sample to be played</param>
        public void Play(double input)
        {
            // clip if outside [-1, +1]
            short s = ConvertToShort(input);
            byte[] temp = BitConverter.GetBytes(s);
            _buffer[_bufferCount++] = temp[0];
            _buffer[_bufferCount++] = temp[1]; //little Endian

            // send to sound card if buffer is full        
            if (_bufferCount >= _buffer.Length)
            {
                _bufferCount = 0;
                _bufferedWaveProvider.AddSamples(_buffer, 0, _buffer.Length);
            }

        }

        /// <summary>
        /// Adda a sample to the buffered wave provider. Note, has audio distortion affects.
        /// </summary>
        /// <param name="input"></param>
        public void PlayNow(double input)
        {
            // clip if outside [-1, +1]
            short s = ConvertToShort(input);
            byte[] temp = BitConverter.GetBytes(s);
            // send to sound card     
            _bufferedWaveProvider.AddSamples(temp, 0, temp.Length);
        }

        /// <summary>
        /// Plays the entire array of data in a single shot
        /// </summary>
        /// <param name="data"></param>
        public void Play(double[] data)
        {
            short[] samples = new short[data.Length];
            for(int i=0; i < data.Length; i++)
            {
                var sample = ConvertToShort(data[i]);
                samples[i] = sample;
            }
            byte[] buffer = new byte[samples.Length*2]; //Twice the length since 2 bytes per short
            long bufferCount = 0;
            foreach (var sample in samples)
            {
                byte[] temp = BitConverter.GetBytes(sample);
                buffer[bufferCount++] = temp[0];
                buffer[bufferCount++] = temp[1];
            }
            _bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Converts the given input to a short clipped at -1, 1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static short ConvertToShort(double input)
        {
            if (input < -1.0)
            {
                input = -1.0;
            }
            if (input > +1.0)
            {
                input = +1.0;
            }
            short s = (short)(short.MaxValue * input);
            return s;
        }

        /// <summary>
        /// Writes the provided array to a wave file.
        /// </summary>
        /// <param name="data">Data to be written to the audio file</param>
        /// <param name="audioFilePath">Path of the audio file.</param>
        public void WriteWave(double[] data, string audioFilePath)
        {
            using (var waveWriter = new WaveFileWriter(audioFilePath, _waveFormat))
            {
                short[] samples = new short[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    var sample = ConvertToShort(data[i]);
                    samples[i] = sample;
                }
                waveWriter.WriteSamples(samples,0, samples.Length);
            }
        }

    }
}