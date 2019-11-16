using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace GordonWare
{
    class DabGame : MiniGame
    {
        private Sprite gordon_chocolat, hm_img;
        private Texture2D gordon_bd, gordon_bg, gordon_bd_alt, gordon_bg_alt;
        private Song song1;
        private double rotation_bd, rotation_bg;
        private SoundEffect hm, mlg;
        private bool RightDab, mlgPlayed;
        private Random random;
        private int x, y, anim_latency;
        private float zoom;

        public DabGame()
        {
            random = new Random();
            name = "Dab Game";
            description_color = Color.Green;
            author = "Paul";
        }

        public override void Reset()
        {
            base.Reset();
            this.CustomInit();
        }

        private void CustomInit()
        {
            mlgPlayed = false;
            MediaPlayer.Play(song1, new TimeSpan(0, 0, 1));
            anim_latency = 0;
            RightDab = random.Next(100) < 50;
            string orientation_dab = RightDab ? "droite" : "gauche";
            description = "Fais dabber Gordon à " + orientation_dab + " !";
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            background = new Sprite(Content.Load<Texture2D>("dabgame/background")); // On appelle le constructeur de Sprite avec un argument texture2D
            gordon_chocolat = new Sprite(Content.Load<Texture2D>("dabgame/gordon-chocolat"));
            gordon_bd = Content.Load<Texture2D>("dabgame/gordon-bd");
            gordon_bg = Content.Load<Texture2D>("dabgame/gordon-bg");
            gordon_bd_alt = Content.Load<Texture2D>("dabgame/gordon-bd-alt");
            gordon_bg_alt = Content.Load<Texture2D>("dabgame/gordon-bg-alt");
            song1 = Content.Load<Song>("music/elevator");
            hm = Content.Load<SoundEffect>("dabgame/hm");
            mlg = Content.Load<SoundEffect>("dabgame/mlg");
            hm_img = new Sprite(Content.Load<Texture2D>("dabgame/hm_img"));
        }

        public override void Update(GameTime gameTime)
        {
            double threshold = 0.15;
            bool winR = RightDab && Math.Abs(2.81 - rotation_bd) < threshold && Math.Abs(2.78 - rotation_bg) < threshold;
            bool winL = !RightDab && Math.Abs(0.5 - rotation_bd) < threshold && Math.Abs(0.28 - rotation_bg) < threshold;
            if (winR || winL)
            {
                MediaPlayer.Stop();
                base.Win();
            }
            base.Update(gameTime);
        }
        public void WinAnimation(SpriteBatch spriteBatch)
        {
            if (!mlgPlayed)
            {
                mlg.Play();
                mlgPlayed = true;
            }
            if (anim_latency == 0)
            {
                x = random.Next(1280);
                y = random.Next(720);
                zoom = random.Next(100, 500)/100;
                hm.Play();
                anim_latency = random.Next(3,6);
            }
            hm_img.TopLeftDraw(spriteBatch, new Vector2(x, y),1, zoom);
            anim_latency--;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                background.TopLeftDraw(spriteBatch, new Vector2(0, 0));
                gordon_chocolat.TopLeftDraw(spriteBatch, new Vector2(1280 / 2 - gordon_chocolat.frameWidth * 3 / 2, 720 / 2 - gordon_chocolat.frameHeight * 3 / 2), 1, 3.0f);

            Vector2 gordonbd_pos = new Vector2(1280 / 2 - gordon_chocolat.frameWidth / 2 - 50, 720 / 2 - gordon_chocolat.frameHeight / 2 - 15);
            Vector2 gordonbg_pos = new Vector2(1280 / 2 + gordon_chocolat.frameWidth / 2 + 65, 720 / 2 - gordon_chocolat.frameHeight / 2 - 5);
            Vector2 centre_rotation_bd = new Vector2(gordon_bd.Width / 2, gordon_bd.Height / 2);
            Vector2 centre_rotation_bg = new Vector2(gordon_bg.Width / 2, gordon_bg.Height / 2);

            if (game_status == GameStatus.Pending)
            {
                MouseState ms = Mouse.GetState();
                Vector2 mousePosition = new Vector2(ms.X, ms.Y);

                Vector2 dPos = gordonbd_pos - mousePosition;
                rotation_bd = Math.Atan2(dPos.Y, dPos.X);
                Vector2 gPos = gordonbg_pos - mousePosition;
                rotation_bg = Math.Atan2(gPos.Y, gPos.X);
            }
            else if(game_status == GameStatus.Win)
            {
                WinAnimation(spriteBatch);
            }
            Texture2D brasD = RightDab ? gordon_bd : gordon_bd_alt;
            Texture2D brasG = RightDab ? gordon_bg : gordon_bg_alt;
            spriteBatch.Draw(brasD, gordonbd_pos, null, null, centre_rotation_bd, (float)rotation_bd, new Vector2(3.0f));
            spriteBatch.Draw(brasG, gordonbg_pos, null, null, centre_rotation_bg, (float)rotation_bg, new Vector2(3.0f));

            base.Draw(spriteBatch); // Par dessus chaque mini jeu est dessiné certains éléments comme la description mais aussi le timer


        }
    }
}
