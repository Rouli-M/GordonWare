using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace GordonWare
{
    class ChooseServiceGame : MiniGame
    {

        private Song song1;
        private SoundEffect hm, mlg;
        private Random random;
        private string input_string, string_to_input;
        private Sprite backgournd, gordon_chocolat, hm_img, clue0, clue1, madoc, prodoc, extradoc;
        private bool mlgPlayed, goodO;
        private int x, y, anim_latency, string_id;
        private float zoom;
        private Vector2[] position, Startposition;

        public ChooseServiceGame()
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
            clue0 = new Sprite(Content.Load<Texture2D>("choosegame/clue0"));//clin d'oeil
            clue1 = new Sprite(Content.Load<Texture2D>("choosegame/clue1"));// l'autre
            madoc = new Sprite(Content.Load<Texture2D>("choosegame/Madoc"));
            prodoc = new Sprite(Content.Load<Texture2D>("choosegame/prodoc"));
            extradoc = new Sprite(Content.Load<Texture2D>("choosegame/extradoc"));
            song1 = Content.Load<Song>("music/Bach_Prelude");
            mlg = Content.Load<SoundEffect>("dabgame/mlg");
            hm = Content.Load<SoundEffect>("dabgame/hm");
            hm_img = new Sprite(Content.Load<Texture2D>("dabgame/hm_img"));

        }
        public override void Reset()
        {
            random = new Random();
            position = new Vector2[3]; // 0 madoc 1 exrtradoc 2 prodoc
            Startposition = new Vector2[3];
            Startposition[0] = new Vector2(random.Next(0, 1280 - madoc.frameWidth), random.Next(0, 640));
            Startposition[1] = new Vector2(random.Next(0, 1280 - madoc.frameWidth), random.Next(0, 640));
            Startposition[2] = new Vector2(random.Next(0, 1280 - madoc.frameWidth), random.Next(0, 640));

            MediaPlayer.Play(song1, new TimeSpan(0, 1, 0));

            string_id = random.Next(0, 2);

            if (string_id == 0)
            {
                string_to_input = "MADOC";
                position[0] = Startposition[0];
                position[1] = Startposition[1];
                position[2] = Startposition[2];
            }
            else if (string_id == 1)
            {
                string_to_input = "EXTRADOC";
                position[0] = Startposition[0];
                position[1] = Startposition[1];
                position[2] = Startposition[2];
            }
            else if (string_id == 2)
            {
                string_to_input = "PRODOC";
                position[0] = Startposition[0];
                position[1] = Startposition[1];
                position[2] = Startposition[2];
            }

            input_string = "";
            description = "Va sur  " + string_to_input + "  vite !";
            base.Reset();
        }


        
    
        // dans le udpte les position et les trucs a Update
        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
            {
                MouseState ms = Mouse.GetState();
                Vector2 mp = new Vector2(ms.X, ms.Y);
                bool inside = mp.X >= position[string_id].X && mp.Y >= position[string_id].Y && mp.X <= (position[string_id].X + madoc.frameWidth) && mp.Y <= (position[string_id].Y + madoc.frameHeight);

                if (inside && ms.LeftButton == ButtonState.Pressed)
                {
                    goodO = true;
                    Win();
                        ;
                }
                else if (ms.LeftButton == ButtonState.Pressed && !inside)
                {
                    goodO = false;
                    Lose();
                }
                else goodO = false;

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
            madoc.TopLeftDraw(spriteBatch, position[0], 1, 1);
            prodoc.TopLeftDraw(spriteBatch, position[2], 1, 1);
            extradoc.TopLeftDraw(spriteBatch, position[1], 1, 1);

            MouseState ms = Mouse.GetState();
            clue1.Draw(spriteBatch, new Vector2(ms.X, ms.Y));
            if (goodO) clue0.Draw(spriteBatch, new Vector2(ms.X, ms.Y));


           if (game_status == GameStatus.Win)
           {
              WinAnimation(spriteBatch);
           }
            

            base.Draw(spriteBatch); // Par dessus chaque mini jeu est dessiné certains éléments comme la description mais aussi le timer


        }

        
    }
}
