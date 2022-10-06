using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PianoSimulation;
using System;

namespace InteractivePiano
{
    public class KeysPiano : DrawableGameComponent
    {
        private InteractivePiano interactivePiano;
        private int posX;
        private int posY;

        private int sizeX;
        private int sizeY;

        public Color color{get; set;}

        public String letter;

        public KeysPiano(InteractivePiano piano, int posX, int posY, int sizeX, int sizeY, Color color): base(piano)
        {
           this.interactivePiano = piano;
           this.posX = posX;
           this.posY = posY;
           this.sizeX = sizeX;
           this.sizeY = sizeY;
           this.color = color;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            interactivePiano.spriteBatch.Begin();
            interactivePiano.spriteBatch.Draw(interactivePiano.texture, new Rectangle(posX, posY, sizeX, sizeY), color);
            interactivePiano.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}