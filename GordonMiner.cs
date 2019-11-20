using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GordonWare
{
    public class GordonMiner : MiniGame
    {
        private Sprite GordoniumOre;
        private Sprite Gordonium;
        private Sprite Gordon;
        private Texture2D LeftArm, RightArm;
        private Texture2D Pickaxe;
        private Sprite MouseSprite;

        private float gordoniumOreSize;
        private Vector2[] gordoniumPosition;
        private float[] gordoniumOpacity;

        private SoundEffect Click;
        private Song song1;

        private int gordonium;

        public GordonMiner()
        {
            name = "Gordon Miner";
            description = "Mine 10 gordonium !";
            description_color = Color.White;
            author = "Efflam";
        }

        public override void Reset()
        {
            gordoniumOreSize = 1;
            gordonium = 0;
            gordoniumPosition = new Vector2[10];
            for (int i = 0; i < gordoniumPosition.Length; i++) gordoniumPosition[i] = new Vector2(0, 0);
            gordoniumOpacity = new float[10];
            for (int i = 0; i < gordoniumOpacity.Length; i++) gordoniumOpacity[i] = 0;

            MediaPlayer.Play(song1, new TimeSpan(0, 0, 1));

            base.Reset();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            background = new Sprite(Content.Load<Texture2D>("gordonminer/background"));
            GordoniumOre = new Sprite(Content.Load<Texture2D>("gordonminer/gordonium_ore"));
            Gordonium = new Sprite(Content.Load<Texture2D>("gordonminer/gordonium"), 2);
            Gordon = new Sprite(Content.Load<Texture2D>("gordonminer/gordon-chocolat"));
            LeftArm = Content.Load<Texture2D>("gordonminer/gordon-bg-alt");
            RightArm = Content.Load<Texture2D>("gordonminer/gordon-bg");
            Pickaxe = Content.Load<Texture2D>("gordonminer/pickaxe");
            MouseSprite = new Sprite(Content.Load<Texture2D>("gordonminer/hand"));

            Click = Content.Load<SoundEffect>("gordonminer/click");
            song1 = Content.Load<Song>("music/Drum n Bass B Drive");

            base.LoadContent(Content);
            
        }
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < gordoniumOpacity.Length; i++)
            {
                if (gordoniumOpacity[i] > 0)
                {
                    gordoniumOpacity[i] -= 0.02f;
                    gordoniumPosition[i].Y -= 3;
                }
            }

            if (game_status == GameStatus.Pending)
            {
                MouseState ms = Mouse.GetState();
                if (ms.X >= 452 && ms.X < 452+349 && ms.Y >= 370 && ms.Y < 370 + 237)
                {
                    if (ms.LeftButton == ButtonState.Pressed)
                    {
                        if (gordoniumOreSize != 0.9f)
                        {
                            gordoniumPosition[gordonium].X = ms.X - 16;
                            gordoniumPosition[gordonium].Y = ms.Y - 32;
                            gordoniumOpacity[gordonium] = 1;
                            gordonium++;
                            Click.Play();
                        }
                        gordoniumOreSize = 0.9f;
                    }
                    else
                    {
                        gordoniumOreSize = 1.1f;
                    }
                }
                else
                {
                    gordoniumOreSize = 1;
                }

                


                if (gordonium >= 10)
                {
                    Win();
                }
            }
            else if (game_status == GameStatus.Win)
            {
                
            }
            else if (game_status == GameStatus.Lose)
            {
                
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            GordoniumOre.DrawFromFeet(spriteBatch, new Vector2(626, 606), 0, gordoniumOreSize);
            for (int i = 0; i < gordoniumOpacity.Length; i++)
            {
                if (gordoniumOpacity[i] != 0)
                    Gordonium.TopLeftDraw(spriteBatch, gordoniumPosition[i], gordoniumOpacity[i]);
            }
            Gordon.DrawFromFeet(spriteBatch, new Vector2(300, 612), 0, 2);
            spriteBatch.Draw(LeftArm, new Rectangle(230, 375, LeftArm.Width * 2, LeftArm.Height * 2), new Rectangle(0,0,LeftArm.Width, LeftArm.Height), Color.White, -1.5f * gordoniumOreSize * (float)Math.PI + 0.55f * (float)Math.PI, new Vector2(60,35), SpriteEffects.None, 1f);
            spriteBatch.Draw(RightArm, new Rectangle(375, 375, RightArm.Width * 2, RightArm.Height * 2), new Rectangle(0, 0, RightArm.Width, RightArm.Height), Color.White, -1.5f * gordoniumOreSize * (float)Math.PI + 0.4f * (float)Math.PI, new Vector2(60, 35), SpriteEffects.None, 1f);
            spriteBatch.Draw(Pickaxe,
                new Rectangle(240 + (int)(62 * Math.Cos(-1.5f * gordoniumOreSize * (float)Math.PI - 0.40f * (float)Math.PI)), 390 + (int)(62 * Math.Sin(-1.5f * gordoniumOreSize * (float)Math.PI - 0.40f * (float)Math.PI)), Pickaxe.Width * 2, Pickaxe.Height * 2),
                new Rectangle(0,0,Pickaxe.Width, Pickaxe.Height), Color.White,
                -0.25f * gordoniumOreSize * (float)Math.PI + 0.60f * (float)Math.PI,
                new Vector2(62, 115), SpriteEffects.None, 1f);

            spriteBatch.DrawString(Roulifont, Convert.ToString(gordonium), new Vector2(600, 150), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 1);
            MouseState ms = Mouse.GetState();
            MouseSprite.TopLeftDraw(spriteBatch, new Vector2(ms.X, ms.Y));

            base.Draw(spriteBatch);
        }
    }
}
