using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PianoSimulation;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

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

        Audio audio;


        public InteractivePiano(Audio audio)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.audio=audio;

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
            for (int j=0; j<15; j++)
            {
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
            previousKeyState = currentKeyState;
            currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();


            int index=0;
            var pressedKeys = Keyboard.GetState().GetPressedKeys();

   
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                 Exit();
            }
                
                foreach (Keys key in pressedKeys)
                {
                   
                    if (currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key)){

                        
                        char pianoKey = key.ToString().ToLower()[0];
                        index = piano.Keys.IndexOf(pianoKey); 
                        allKeys[index].isDown = true;

                        System.Diagnostics.Debug.WriteLine(pianoKey);
                        Task t;
                        t = new Task(() => {
                            Audio.Reset();
                            piano.StrikeKey(pianoKey); 
                            for (int j=0; j<piano.SamplingRate*3; j++)
                            {
                                audio.Play(piano.Play());
                            }
                            });
                        t.Start();
                        t.Wait();
                    } 
               

                }     


               

                int index2;
                for (int a=0; a<allKeys.Length; a++){
                    allKeys[a].isDown=false;
                     for (int i=0; i<pressedKeys.Length; i++)
                    {
                        char pianoKey = pressedKeys[i].ToString().ToLower()[0];
                        index2 = piano.Keys.IndexOf(pianoKey); 
                        allKeys[index2].isDown = true;

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
