using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GordonWare
{
    public class MiniGame
    {
        internal Sprite background; // background est le sprite qui va s'afficher en fond, doit être en 1280/720
        internal string name, author, description; // name est le nom du mini jeu, description est la phrase qui va s'afficher au début du mini jeu
        float timer;
        internal SpriteFont Roulifont;
        public MiniGame(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            timer = 0;
            Roulifont = Content.Load<SpriteFont>("Rouli");
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > 7000) ;
                //lose;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (timer < 3000)
            {
                // affichage de l'instruction/la description du mini jeu?
                spriteBatch.DrawString(Roulifont, description,new Vector2(500,10), Color.White,0f, new Vector2(description.Length * 10, 10), 1 + 0.5f * (float)Math.Cos(timer/200f),SpriteEffects.None, 0f);
            }
        }
    }
}
