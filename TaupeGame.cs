using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GordonWare
{
    class TaupeGame : MiniGame
    {
        private Sprite Marteau;
        private Sprite Gordon, GordonTriste;

        private const int showTime = 60;
        private const int loopTime = 120;
        private int[] TimerGordon;
        private bool[] killedGordon;
        private Vector2[] holes;
        private int gordonCount;

        private Random random;

        public TaupeGame()
        {
            name = "Taupe Game";
            description = "Tape 3 Gordons !";
            description_color = Color.Black;
            author = "Efflam";
        }

        public override void Reset()
        {
            random = new Random();

            gordonCount = 0;

            holes = new Vector2[8];
            holes[3] = new Vector2(324, 590);
            holes[0] = new Vector2(352, 333);
            holes[2] = new Vector2(520, 476);
            holes[1] = new Vector2(617, 302);
            holes[4] = new Vector2(690, 601);
            holes[5] = new Vector2(775, 437);
            holes[6] = new Vector2(951, 588);
            holes[7] = new Vector2(981, 384);

            TimerGordon = new int[8];
            for (int i = 0; i < TimerGordon.Length; i++) TimerGordon[i] = random.Next(0, loopTime - 1);
            killedGordon = new bool[8];
            for (int i = 0; i < killedGordon.Length; i++) killedGordon[i] = false;

            base.Reset();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            background = new Sprite(Content.Load<Texture2D>("taupegame/background"));
            Marteau = new Sprite(Content.Load<Texture2D>("taupegame/marteau"));
            Gordon = new Sprite(Content.Load<Texture2D>("taupegame/gordon"));
            GordonTriste = new Sprite(Content.Load<Texture2D>("taupegame/gordon_triste"));
        }

        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
            {
                MouseState ms = Mouse.GetState();
                for (int i = 0; i < TimerGordon.Length; i++)
                {
                    if (!killedGordon[i])
                        TimerGordon[i]++;
                    if (TimerGordon[i] > loopTime)
                        TimerGordon[i] = 0;
                }
                if (ms.LeftButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < TimerGordon.Length; i++)
                    {
                        if (!killedGordon[i] && TimerGordon[i] < showTime && (new Vector2(ms.X, ms.Y) - holes[i]).Length() <= 50)
                        {
                            killedGordon[i] = true;
                            gordonCount++;
                        }
                    }
                }
                if (gordonCount >= 3)
                {
                    if (game_status == GameStatus.Pending) Win();
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            for (int i = 0; i < TimerGordon.Length; i++)
            {
                if (killedGordon[i])
                {
                    GordonTriste.Draw(spriteBatch, holes[i]);
                }
                else if (TimerGordon[i] < showTime)
                {
                    Gordon.Draw(spriteBatch, holes[i]);
                }
            }

                MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed) Marteau.Draw(spriteBatch, new Vector2(ms.X+80, ms.Y), -(float)Math.PI/6);
            else Marteau.Draw(spriteBatch, new Vector2(ms.X+80, ms.Y), (float)Math.PI / 6);

            base.Draw(spriteBatch);
        }
    }
}
