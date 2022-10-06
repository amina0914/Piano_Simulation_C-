﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PianoSimulation;
using System.Collections.Generic;
using System;
using System.Threading;

namespace InteractivePiano
{
    public class InteractivePiano : Game
    {
        KeyboardState currentKeyState;
        KeyboardState previousKeyState;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Texture2D texture;
        private Texture2D texture2;

        List<KeysPiano> whiteKeys;

        List<KeysPiano> blackKeys;
        Piano piano = new Piano();

        KeysPiano[] allKeys;



        public InteractivePiano()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {        
            graphics.PreferredBackBufferWidth = 1540; 
            graphics.PreferredBackBufferHeight = 500;
            graphics.ApplyChanges();      

                int whitePosX=0;
                int whitePosY=0;
                int whiteSizeX=70;
                int whiteSizeY=220;
                Color colorWhite=Color.White;

                int blackPosX=50;
                int blackPosY=0;
                int blackSizeX=40;
                int blackSizeY=120;
                Color colorBlack=Color.Black;

            String[] lettersAssociated = new String[piano.Keys.Length];
            String[] letters = {"A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"};

                
            for (int a=0; a<piano.Keys.Length; a++){
                lettersAssociated[a] = letters[a%letters.Length];
            }

            whiteKeys = new List<KeysPiano>();
            for (int i=0; i<22; i++)
            {
                KeysPiano keysPiano = new KeysPiano(this, whitePosX, whitePosY, whiteSizeX, whiteSizeY, colorWhite);
                whiteKeys.Add(keysPiano);
                Components.Add(keysPiano);
                whitePosX = whitePosX + 70;
            }
            blackKeys = new List<KeysPiano>(); 
            for (int j=0; j<15; j++){
                KeysPiano keysPiano = new KeysPiano(this, blackPosX, blackPosY, blackSizeX, blackSizeY, colorBlack);
                Components.Add(keysPiano);
                if (j==0 || j==2 || j== 5 || j==7 || j==10 || j==12)
                {
                    blackPosX = blackPosX + 140;
                }
                else 
                {
                    blackPosX = blackPosX + 70;
                }
                blackKeys.Add(keysPiano);
                }    
            allKeys = new KeysPiano[piano.Keys.Length];   
            int indexBlack = 0 ;
            int indexWhite = 0;
            for (int b=0; b<piano.Keys.Length; b++){

                if(b%12==1 || b%12==4 || b%12==6 || b%12==9 || b%12==11)
                {
                    allKeys[b] = blackKeys[indexBlack];
                    indexBlack++;
                }
                else 
                {
                    allKeys[b] = whiteKeys[indexWhite];
                    indexWhite++;
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("whiteRectangle");
            texture2 = Content.Load<Texture2D>("blackKey");

            currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            previousKeyState = currentKeyState;

        }

        protected override void Update(GameTime gameTime)
        {
            foreach(var key in whiteKeys)
            {
                key.color = Color.White;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                 Exit();
            }
            previousKeyState = currentKeyState;
            currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            // if pressed key == current state, or old state, only play key once      

            List<Keys> allPressedKeys=new List <Keys>();
            int index=0;
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
                allPressedKeys.Add(key);
                if (Keyboard.GetState().IsKeyDown(key))
                {
                    char pianoKey = key.ToString().ToLower()[0];

                    // if piano frequency is modulo of 110, call A
                    index = piano.Keys.IndexOf(pianoKey);
                    
                    allKeys[index].color = Color.Gray ;
    
                    // put task for stike key (strike and play)

                    System.Diagnostics.Debug.WriteLine(pianoKey);
                    piano.StrikeKey(pianoKey); 
                    // using (Audio audio = Audio.Instance){

                    Audio audio = Audio.Instance; 
                    audio.Reset();     //not sure when to call reset  
                    for (int j=0; j<piano.SamplingRate*3; j++)
                    {
                        audio.Play(piano.Play());
                    }

                }
                // if (Keyboard.GetState().IsKeyUp(key))
                // {
                //     allKeys[index].color = Color.Yellow ;
                // }

            }


            //Here turning back to white, for now yellow for testing purposes

            // foreach (Keys pressedKey in allPressedKeys)
            // {
            //     // if (Keyboard.GetState().IsKeyUp(pressedKey)){
            //         if (previousKeyState.IsKeyDown(pressedKey) && currentKeyState.IsKeyDown(pressedKey) ){
            //             Console.WriteLine("true");
            //             index = piano.Keys.IndexOf(pressedKey.ToString().ToLower()[0]);
            //             allKeys[index].color = Color.Yellow ;
            //     }   
                // Console.WriteLine("false");

                // allKeys[index].color = Color.Yellow;


            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Orange);

            base.Draw(gameTime);
        }
    }
}
