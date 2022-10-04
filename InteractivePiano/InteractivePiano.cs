﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PianoSimulation;
using System;

namespace InteractivePiano
{
    public class InteractivePiano : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D background;

        public InteractivePiano()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // Piano piano = new Piano();
            // Audio audio = new Audio.Instance();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("blackKey");
            // TODO: use this.Content to load your game content here

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
 
            _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.Orange);
 
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
