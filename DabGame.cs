using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GordonWare
{
    class DabGame : MiniGame
    {
        private Sprite gordon_chocolat;
        private Texture2D gordon_bd, gordon_bg;
        private double rotation_bd, rotation_bg;
        private SoundEffect hm;
        public DabGame()
        {
            name = "Dab Game";
            description = "Fais dabber Gordon !";
            description_color = Color.Black;
            author = "Paul";
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            background = new Sprite(Content.Load<Texture2D>("dabgame/background")); // On appelle le constructeur de Sprite avec un argument texture2D
            gordon_chocolat = new Sprite(Content.Load<Texture2D>("dabgame/gordon-chocolat"));
            gordon_bd = Content.Load<Texture2D>("dabgame/gordon-bd");
            gordon_bg = Content.Load<Texture2D>("dabgame/gordon-bg");
            hm = Content.Load<SoundEffect>("dabgame/hm");
        }

        public override void Update(GameTime gameTime)
        {
            //base.Win();
            //base.Lose();*
            double threshold = 0.15;
            if (Math.Abs(2.81 - rotation_bd) < threshold && Math.Abs(2.78 - rotation_bg) < threshold)
            {
                WinSFX();
                base.Win();
            }
            base.Update(gameTime);
        }
        public void WinSFX()
        {
            hm.Play();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));
            gordon_chocolat.TopLeftDraw(spriteBatch, new Vector2(1280 / 2 - gordon_chocolat.frameWidth * 3 / 2, 720 / 2 - gordon_chocolat.frameHeight * 3 / 2),1, 3.0f);

            MouseState ms = Mouse.GetState();
            Vector2 mousePosition = new Vector2(ms.X, ms.Y);

            Vector2 gordonbd_pos = new Vector2(1280 / 2 - gordon_chocolat.frameWidth / 2 - 50, 720 / 2 - gordon_chocolat.frameHeight / 2 - 15);
            Vector2 gordonbg_pos = new Vector2(1280 / 2 + gordon_chocolat.frameWidth / 2 + 65, 720 / 2 - gordon_chocolat.frameHeight / 2 - 5);
            Vector2 dPos = gordonbd_pos - mousePosition;
            rotation_bd = Math.Atan2(dPos.Y, dPos.X);
            Vector2 gPos = gordonbg_pos - mousePosition;
            rotation_bg = Math.Atan2(gPos.Y, gPos.X);
            Vector2 centre_rotation_bd = new Vector2(gordon_bd.Width / 2, gordon_bd.Height / 2);
            Vector2 centre_rotation_bg = new Vector2(gordon_bg.Width / 2, gordon_bg.Height / 2);
            spriteBatch.Draw(gordon_bd, gordonbd_pos, null, null, centre_rotation_bd, (float)rotation_bd, new Vector2(3.0f));
            spriteBatch.Draw(gordon_bg, gordonbg_pos, null, null, centre_rotation_bg, (float)rotation_bg, new Vector2(3.0f));

            base.Draw(spriteBatch); // Par dessus chaque mini jeu est dessiné certains éléments comme la description mais aussi le timer


        }
    }
}
