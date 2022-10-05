using Microsoft.Xna.Framework;
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

        private Texture2D texture;
        private Texture2D texture2;


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
            texture = Content.Load<Texture2D>("whiteRectangle");
            texture2 = Content.Load<Texture2D>("blackKey");
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

            _graphics.PreferredBackBufferWidth = 1540; 
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.ApplyChanges();

            GraphicsDevice.Clear(Color.Orange);

            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, new Rectangle(0, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(70, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(140, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(210, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(280, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(350, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(420, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(490, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(560, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(630, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(700, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(770, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(840, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(910, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(980, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1050, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1120, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1190, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1190, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1260, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1330, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1400, 0, 70, 220), Color.White);
            _spriteBatch.Draw(texture, new Rectangle(1470, 0, 70, 220), Color.White);

            _spriteBatch.Draw(texture2, new Rectangle(50, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(190, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(260, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(400, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(470, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(540, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(680, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(750, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(890, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(960, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(1030, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(1170, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(1240, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(1380, 0, 40, 120), Color.Black);
            _spriteBatch.Draw(texture2, new Rectangle(1450, 0, 40, 120), Color.Black);



            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
