using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GordonWare
{
    public static class TransitionScreen
    {
        private static Sprite background, life, life_broken, bande, bande_reverse;
        private static SpriteFont MicrosoftSansFont;
        private static int previous_life_counter, life_counter, display_life_counter, score, lifetime, transition_timer;
        public static bool is_over { get; private set; } = false;
        private static Texture2D bande_texture;

        public static void Transition (int life_counter, int score, int lifetime)
        {
            TransitionScreen.life_counter = life_counter;
            TransitionScreen.score = score;
            TransitionScreen.lifetime = lifetime;
            transition_timer = 0;
            is_over = false;
        }

        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            MicrosoftSansFont = Content.Load<SpriteFont>("MicrosoftSans");
            background = new Sprite(Content.Load<Texture2D>("transition_screen/background"));
            life = new Sprite(Content.Load<Texture2D>("transition_screen/life"));
            life_broken = new Sprite(Content.Load<Texture2D>("transition_screen/life_broken"));
            bande = new Sprite(Content.Load<Texture2D>("transition_screen/bande_vague"));
            bande_reverse = new Sprite(Content.Load<Texture2D>("transition_screen/bande_vague_reverse"));
            previous_life_counter = 3;
        }

        public static void Update(GameTime gameTime)
        {
            lifetime -= gameTime.ElapsedGameTime.Milliseconds;
            transition_timer += gameTime.ElapsedGameTime.Milliseconds;
            if (lifetime <= 0 && life_counter > 0)
            {
                is_over = true;
                MiniGameManager.NextMiniGame();
                previous_life_counter = life_counter;
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));
            bande.TopLeftDraw(spriteBatch, new Vector2((transition_timer / 2 + 1280) % (2*1280) - 1280, 530));
            bande.TopLeftDraw(spriteBatch, new Vector2(transition_timer / 2 % (2*1280) - 1280, 530));
            bande_reverse.TopLeftDraw(spriteBatch, new Vector2(1280 - (transition_timer / 2) % (2 * 1280), 0));
            bande_reverse.TopLeftDraw(spriteBatch, new Vector2(1280 - ((transition_timer / 2 + 1280) % (2 * 1280)), 0));

            spriteBatch.DrawString(MicrosoftSansFont, Convert.ToString(score), new Vector2(1280/2, 720/2 - 50), Color.White, 0.2f *  (float)Math.Cos(Convert.ToDouble(transition_timer * 0.004f)), new Vector2(MicrosoftSansFont.MeasureString(Convert.ToString(score)).X/2, MicrosoftSansFont.MeasureString(Convert.ToString(score)).Y * 0.5f), 1f, SpriteEffects.None, 0f);

            if (transition_timer < 700) display_life_counter = previous_life_counter;
            else display_life_counter = life_counter;

            for (int i=1; i<4; i++)
            {
                Sprite display_hearth;
                if (i <= display_life_counter) display_hearth = life;
                else display_hearth = life_broken;
                if (transition_timer < 700 && i > life_counter && i == previous_life_counter) display_hearth.Draw(spriteBatch, new Vector2(450 + 10 * (float)Math.Cos(Convert.ToDouble(transition_timer * 0.04f)) + i * 100, 475));
                else display_hearth.Draw(spriteBatch, new Vector2(450 + i * 100, 475));
            }
            
        }
    }
}
