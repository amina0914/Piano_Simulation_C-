using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PianoSimulation;
using System.Collections.Generic;
using System;

namespace InteractivePiano
{
    public class InteractivePiano : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Texture2D texture;
        private Texture2D texture2;

        List<KeysPiano> whiteKeys;

        List<KeysPiano> blackKeys;

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
            String[] letters = {"A", "B", "C", "D", "E", "F", "G"};
            whiteKeys = new List<KeysPiano>();
            for (int i=0; i<22; i++)
            {
                KeysPiano keysPiano = new KeysPiano(this, whitePosX, whitePosY, whiteSizeX, whiteSizeY, colorWhite, letters[i%letters.Length]);
                whiteKeys.Add(keysPiano);
                Components.Add(keysPiano);
                whitePosX = whitePosX + 70;
            }
            int blackPosX=50;
            int blackPosY=0;
            int blackSizeX=40;
            int blackSizeY=120;
            Color colorBlack=Color.Black;
            String[] lettersSharp = {"A#", "B#", "C#", "D#", "E#", "F#", "G#"};
            blackKeys = new List<KeysPiano>(); 
            for (int j=0; j<15; j++){
                KeysPiano keysPiano = new KeysPiano(this, blackPosX, blackPosY, blackSizeX, blackSizeY, colorBlack,lettersSharp[j%lettersSharp.Length]);
                Components.Add(keysPiano);
                if (j==0 || j==2 || j== 5 || j==7 || j==10 || j==12)
                {
                    blackPosX = blackPosX + 140;
                }
                else {
                    blackPosX = blackPosX + 70;
                }

            }       
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("whiteRectangle");
            texture2 = Content.Load<Texture2D>("blackKey");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                 Exit();
            }

            // if pressed key == current state, or old state, only play key once 

            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (Keys key in pressedKeys)
            {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                Piano piano = new Piano();
                char pianoKey = key.ToString().ToLower()[0];

                // if piano frequency is modulo of 110, call A
                int index = piano.Keys.IndexOf(pianoKey);
                if (piano.Wires[index].NoteFrequency % 110 == 0){
                    foreach(KeysPiano whiteKey in whiteKeys)
                    {
                        if(whiteKey.letter=="A"){
                            whiteKey.color = Color.Gray ;
                        }

                    }
                }

                System.Diagnostics.Debug.WriteLine(pianoKey);
                piano.StrikeKey(pianoKey); 
                // using (Audio audio = Audio.Instance){


                    Audio audio = Audio.Instance; 
                    audio.Reset();     //not sure when to call reset  
                    for (int j=0; j<piano.SamplingRate*3; j++)
                    {
                        audio.Play(piano.Play());
                    }

                // }   
            }            
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Orange);

            base.Draw(gameTime);
        }
    }
}
