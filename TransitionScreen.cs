using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GordonWare
{
    public static class TransitionScreen
    {
        private static Sprite background, life, life_broken, bande, bande_reverse;
        private static int life_counter, score, lifetime, transition_timer;
        public static bool is_over { get; private set; } = false;

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
            background = new Sprite(Content.Load<Texture2D>("transition_screen/background"));
            life = new Sprite(Content.Load<Texture2D>("transition_screen/life"));
            life_broken = new Sprite(Content.Load<Texture2D>("transition_screen/life_broken"));
            bande = new Sprite(Content.Load<Texture2D>("transition_screen/bande_vague"));
        }

        public static void Update(GameTime gameTime)
        {
            lifetime -= gameTime.ElapsedGameTime.Milliseconds;
            transition_timer += gameTime.ElapsedGameTime.Milliseconds;
            if (lifetime <= 0 && life_counter > 0)
            {
                is_over = true;
                MiniGameManager.NextMiniGame();
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));
            bande.Draw(spriteBatch, new Vector2((transition_timer / 2 + 1280) % (2*1280) - 1280, 530));
            bande.Draw(spriteBatch, new Vector2(transition_timer / 2 % (2*1280) - 1280, 530));
            bande.Draw(spriteBatch, new Vector2(1280 / 2 + 1280 - (transition_timer / 2) % (2 * 1280), 70), new Rectangle(0, 0, 1280, 200), 1, SpriteEffects.FlipVertically);
            bande.Draw(spriteBatch, new Vector2(1280 / 2 + 1280 - ((transition_timer / 2 + 1280) % (2 * 1280)), 70), new Rectangle(0, 0, 1280, 200), 1, SpriteEffects.FlipVertically);
            for (int i=1; i<4; i++)
            {
                Sprite display_hearth;
                if (i <= life_counter) display_hearth = life;
                else display_hearth = life_broken;
                display_hearth.Draw(spriteBatch, new Vector2(450 + i * 100, 300));
            }
        }
    }
}
