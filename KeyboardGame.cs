using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace GordonWare
{
    class KeyboardGame : MiniGame
    {
        private Sprite gordon_down, gordon_up;
        private SpriteFont arial;
        private Song song1;
        private string input_string;
        public KeyboardGame()
        {
            name = "Keyboard Game";
            description = "Tape 'GORDON' au clavier !";
            description_color = Color.Black;
            author = "Martin";

            input_string = "";
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            background = new Sprite(Content.Load<Texture2D>("keyboardgame/background")); // On appelle le constructeur de Sprite avec un argument texture2D
            gordon_up = new Sprite(Content.Load<Texture2D>("keyboardgame/gordon_up"));
            gordon_down = new Sprite(Content.Load<Texture2D>("keyboardgame/gordon_down"));
            arial = Content.Load<SpriteFont>("keyboardgame/arial");
            song1 = Content.Load<Song>("music/Drum n Bass B Drive");
        }

        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
            {
                if (Input.keyboard.GetPressedKeys().Length > 0)
                {
                    var keyValue = Input.keyboard.GetPressedKeys()[0].ToString();
                    if (!input_string.EndsWith(keyValue)) input_string += keyValue;
                }
                if (input_string == "GORDON") base.Win();
                else if (!(input_string == "" ||
                    input_string == "G" ||
                    input_string == "GO" ||
                    input_string == "GOR" ||
                    input_string == "GORD" ||
                    input_string == "GORDO")) base.Lose();

                // Console.WriteLine(input_string); utilisé pour le debug
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
            if (Input.AnyKeyPressed()) gordon_down.TopLeftDraw(spriteBatch, new Vector2(50, 50));
            else gordon_up.TopLeftDraw(spriteBatch, new Vector2(50, 50));
            spriteBatch.DrawString(arial, input_string, new Vector2(700, 300), Color.Black,0f, new Vector2(0,0),2f,SpriteEffects.None, 0f);
            base.Draw(spriteBatch); // Par dessus chaque mini jeu est dessiné certains éléments comme la description mais aussi le timer
        }

        public override void Reset()
        {
            MediaPlayer.Play(song1,new TimeSpan(0,0,1));
            input_string = "";
            base.Reset();
        }
    }
}
