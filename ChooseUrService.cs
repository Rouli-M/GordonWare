using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace GordonWare
{
    class ChooseUrService : MiniGame
    {
        public ChooseUrService()
        {
            name = "Choose Your WebService";
            description = "Va sur Madoc";
            description_color = Color.Black;
            author = "Théophane";

            input_string = "";

        }
        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            background = new Sprite(Content.Load<Texture2D>("choosegame/background")); // On appelle le constructeur de Sprite avec un argument texture2D
            clue0 = new Sprite(Content.Load<Texture2D>("choosegame/clue0"));
            clue1 = new Sprite(Content.Load<Texture2D>("choosegame/clu1"));
            madoc = new Sprite(Content.Load<Texture2D>("choosegame/Madoc"));
            prodoc = new Sprite(Content.Load<Texture2D>("choosegame/prodoc"));
            extradoc = new Sprite(Content.Load<Texture2D>("choosegame/extradoc"));
            song1 = Content.Load<Song>("music/Bach Prelude");
            mlg = Content.Load<SoundEffect>("choosegame/mlgsound");
            hm = Content.Load<SoundEffect>("dabgame/hm");
            hm_img = new Sprite(Content.Load<Texture2D>("dabgame/hm_img"));

        }
        public override void Reset()
        {
            random = new Random();

            position = new Vector2[3];
            Sartposition = new Vector2[3];
            Sartpostion[0] = new Vector2(215,320);
            Sartpostion[1] = new Vector2(645, 480);
            Sartpostion[2] = new Vector2(860, 640);

            MediaPlayer.Play(song1, new TimeSpan(0, 1, 0));

            int string_id = r.Next(0, 2);

            if (string_id == 0)
            {
                string_to_input = "MADOC";
                position[string_id] = Startposition[1]
            }
            else if (string_id == 1)
            {
                string_to_input = "EXTRADOC";
                position[string_id] = Startposition[1]
            }
            else if (string_id == 2)
            {
                string_to_input = "PRODOC";
                position[string_id] = Startposition[1]
            }


            input_string = "";
            description = "Va sur " + string_to_input + " vite !";
            base.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
                MouseState ms = Mouse.GetState();
                Vector2 mp = new vector2(ms.X, ms.Y);



           



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
                zoom = random.Next(100, 500) / 100;
                hm.Play();
                anim_latency = random.Next(3, 6);
            }
            hm_img.TopLeftDraw(spriteBatch, new Vector2(x, y), 1, zoom);
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
            else if (game_status == GameStatus.Win)
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
