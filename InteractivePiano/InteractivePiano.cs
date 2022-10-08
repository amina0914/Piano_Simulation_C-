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
        private KeyboardState _currentKeyState;
        private KeyboardState _previousKeyState;
        protected GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Texture2D texture;
        // private Texture2D texture2;

        private List<KeysPiano> _whiteKeys;

        private  List<KeysPiano> _blackKeys;
        private Piano _piano = new Piano();

        private KeysPiano[] _allKeys;

        private Audio _audio;

        public SpriteFont font;

        private String[] _lettersAssociated;


        public InteractivePiano(Audio audio)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this._audio=audio;

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

            _lettersAssociated = new String[_piano.Keys.Length];
            String[] letters = {"A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#"};

                
            for (int a=0; a<_piano.Keys.Length; a++){
                _lettersAssociated[a] = letters[a%letters.Length];
            }

            _whiteKeys = new List<KeysPiano>();
            for (int i=0; i<22; i++)
            {
                KeysPiano keysPiano = new KeysPiano(this, whitePosX, whitePosY, whiteSizeX, whiteSizeY, colorWhite);
                _whiteKeys.Add(keysPiano);
                Components.Add(keysPiano);
                whitePosX = whitePosX + 70;
            }
            _blackKeys = new List<KeysPiano>(); 
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
                _blackKeys.Add(keysPiano);
            }    
            _allKeys = new KeysPiano[_piano.Keys.Length];   
            int indexBlack = 0 ;
            int indexWhite = 0;
            for (int b=0; b<_piano.Keys.Length; b++){

                if(b%12==1 || b%12==4 || b%12==6 || b%12==9 || b%12==11)
                {
                    _allKeys[b] = _blackKeys[indexBlack];
                    indexBlack++;
                }
                else 
                {
                    _allKeys[b] = _whiteKeys[indexWhite];
                    indexWhite++;
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = Content.Load<Texture2D>("whiteRectangle");
            // texture2 = Content.Load<Texture2D>("blackKey");

            _currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            _previousKeyState = _currentKeyState;
            
            font = Content.Load<SpriteFont>("Letter");

        }   

        protected override void Update(GameTime gameTime)
        {              
            _previousKeyState = _currentKeyState;
            _currentKeyState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            int index=0;
            var pressedKeys = Keyboard.GetState().GetPressedKeys(); 
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) 
            {
                 Exit();
            }
                
                foreach (Keys key in pressedKeys)
                {
                   
                    if (_currentKeyState.IsKeyDown(key) && _previousKeyState.IsKeyUp(key)){

                       
                        Console.WriteLine("key obj " + key);
                        char pianoKey = CheckKey(key);
                        index = _piano.Keys.IndexOf(pianoKey); 
                        Console.WriteLine("index in piano" + index);
                        _allKeys[index].letter = _lettersAssociated[index];
                        Console.WriteLine("letter " + _allKeys[index].letter);
                        _allKeys[index].isDown = true;


                        //    Task t;
                        // t = new Task(() => {
                            Task.Run(()=>
                            {
                            Audio.Reset();
                            _piano.StrikeKey(pianoKey); 
                            for (int j=0; j<_piano.SamplingRate*3; j++)
                            {
                                _audio.Play(_piano.Play());
                            }
                            });
                
                        // t.Start();
                        // t.Wait();
                    } 
               
                }     

                int index2;
                for (int a=0; a<_allKeys.Length; a++){
                    _allKeys[a].isDown=false;
                     for (int i=0; i<pressedKeys.Length; i++)
                    {
                        char pianoKey = CheckKey(pressedKeys[i]);
                        index2 = _piano.Keys.IndexOf(pianoKey); 
                        _allKeys[index2].isDown = true;

                    }     
                }   
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Orange);     

            base.Draw(gameTime);
        }
    

        protected char CheckKey(Keys key){
            char keyToStrike;
            if (key == Keys.OemMinus){
                keyToStrike='-';
            } else if (key == Keys.OemOpenBrackets){
                keyToStrike='[';
            }else if (key == Keys.OemPlus){
                keyToStrike='=';
            }else if (key == Keys.OemComma){
                keyToStrike=',';
            }else if (key == Keys.OemPeriod){
                keyToStrike='.';
            }else if (key == Keys.OemSemicolon){
                keyToStrike=';';
            }else if (key == Keys.OemQuestion){
                keyToStrike='/';
            }else if (key == Keys.OemQuotes){
                keyToStrike='\'';
            }else if (key == Keys.Space){
                keyToStrike=' ';
            }else if (key == Keys.D2){
                keyToStrike='2';
            }else if (key == Keys.D4){
                keyToStrike='4';
            }else if (key == Keys.D5){
                keyToStrike='5';
            }else if (key == Keys.D7){
                keyToStrike='7';
            }else if (key == Keys.D8){
                keyToStrike='8';
            }else if (key == Keys.D9){
                keyToStrike='9';
            } else {
                keyToStrike = key.ToString().ToLower()[0];;
            }
            return keyToStrike;
        }

    }

}


//left to do:
//error handling, if this key ...
//check and correct certain keys like 2
//correct code from assignment 1
//go over, make it all safe, private where possible
//make cords work