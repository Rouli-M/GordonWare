using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace GordonWare
{
    public static class MiniGameManager
    {
        static private MiniGame currentMiniGame;
        static private List<MiniGame> minigames = new List<MiniGame>();
        static private int life_counter = 3;
        static private int score = 0;

        public static void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            foreach (MiniGame minigame in minigames) minigame.LoadContent(Content);
            TransitionScreen.LoadContent(Content);
            currentMiniGame = minigames[0];
            Transition();
        }
        public static void Update(GameTime gameTime)
        {
            if (!TransitionScreen.is_over) TransitionScreen.Update(gameTime);
            else currentMiniGame.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!TransitionScreen.is_over) TransitionScreen.Draw(spriteBatch);
            else currentMiniGame.Draw(spriteBatch);
        }


        public static void Win()
        {
            Transition();
        }
        public static void Lose()
        {
            if (life_counter > 0)
            {
                life_counter -= 1;
            }
            else
            {
               // GameOver();
            }
            Transition();
        }

        public static void AddMiniGame(MiniGame minigame)
        {
            minigames.Add(minigame);
        }

        public static void Transition()
        {
            int life_time = 2000;
            TransitionScreen.Transition(life_counter, score, life_time);
        }
         public static void NextMiniGame()
        {
            currentMiniGame = minigames[0];
            currentMiniGame.Reset();
        }
        internal static void GameOver()
        {

        }
    }
}
