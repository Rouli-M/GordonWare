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
        private Random r;
        private SoundEffect key1, key2, key3;
        private string input_string, string_to_input;
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
            key1 = Content.Load<SoundEffect>("keyboardgame/key1");
            key2 = Content.Load<SoundEffect>("keyboardgame/key2");
            key3 = Content.Load<SoundEffect>("keyboardgame/key3");
        }

        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
            {
                if (Input.keyboard.GetPressedKeys().Length > 0)
                {
                    var keyValue = Input.keyboard.GetPressedKeys()[0].ToString();
                    if (!input_string.EndsWith(keyValue))
                    {
                        input_string += keyValue;
                        if (r.Next(0, 2) == 0) key1.Play();
                        else if (r.Next(0, 2) == 0) key2.Play();
                        else key3.Play();
                    }
                    if (input_string.EndsWith("Left")) input_string = input_string.Remove(input_string.Length - 4);
                    if (input_string.EndsWith("LeftShift")) input_string = input_string.Remove(input_string.Length - 9);
                }
                if (input_string == string_to_input) base.Win();
                else
                {
                    bool wrong_combinaison = false;
                    int char_index = 0;
                    foreach (char c in input_string)
                    {
                        if (char_index > string_to_input.Length - 1) wrong_combinaison = true;
                        else if (c != string_to_input[char_index]) wrong_combinaison = true;
                        char_index++;
                    }

                   // for (int i = 0; i < input_string.Length; i++)
                    //    if (input_string.Substring(0, i) == string_to_input.Substring(0, i))
                   //         wrong_combinaison = false;

                    if (wrong_combinaison) base.Lose();
                }

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
            r = new Random();
            MediaPlayer.Play(song1,new TimeSpan(0,0,1));
            int string_id = r.Next(0, 12);
            if (string_id == 0) string_to_input = "GORDON";
            else if (string_id == 1) string_to_input = "NODROG";
            else if (string_id == 2) string_to_input = "UNIV";
            else if (string_id == 3) string_to_input = "WEBSERVICE";
            else if (string_id == 4) string_to_input = "MADOC";
            else if (string_id == 5) string_to_input = "ZDNEV";
            else if (string_id == 6) string_to_input = "JUBILER";
            else if (string_id == 7) string_to_input = "AMLBG";
            else if (string_id == 8) string_to_input = "WEBSEVRICE";
            else if (string_id == 9) string_to_input = "EXTRADOC";
            else if (string_id == 10) string_to_input = "PRODOC";
            else string_to_input = "AZERTYUIOP";
            input_string = "";
            description = "Tape " + string_to_input + " au clavier !";
            base.Reset();
        }
    }
}
