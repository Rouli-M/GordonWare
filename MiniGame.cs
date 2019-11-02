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
        float timer, time_limit;
        internal SpriteFont Roulifont;
        public MiniGame()
        {
            timer = 0;
            time_limit = 4000;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Roulifont = Content.Load<SpriteFont>("Rouli");
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > time_limit)
                this.Lose();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // affichage de l'instruction/la description du mini jeu?
            spriteBatch.DrawString(Roulifont, description,new Vector2(1280/2,200), Color.White,0f, Roulifont.MeasureString(description)/2, 1.5f + 0.1f * (float)Math.Cos(timer/200f),SpriteEffects.None, 0f);

            float time_left = time_limit - timer;
            string seconds_left = Convert.ToString((time_left - time_left % 1000) / 1000);
            string milliseconds_left = Convert.ToString(999 - timer % 1000);
            spriteBatch.DrawString(Roulifont, seconds_left + "." + milliseconds_left, new Vector2(10,10), Color.White, 0f, new Vector2(), 2, SpriteEffects.None, 0);
        }

        public void Reset()
        {
            timer = 0;
            time_limit = 4000;
        }

        internal void Win()
        {
            MiniGameManager.Win();
        }

        private void Lose()
        {
            MiniGameManager.Lose();
        }

    }
}
