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
    /// <summary>
    /// This is the main type for your game.
    /// Please do not modify this or any other class in your pull request, but
    /// feel free to change it in order to experiment or just add your minigame
    /// to the minigame rotation in order to test it.
    /// In order to create a minigame, you have to create a new file with your 
    /// minigame's name as a title such as "MiniGameName.cs". This class 
    /// should herit from the MiniGame class, please refer to the MiniGame 
    /// class and other sample mini games such as KeyboardGame or DabGame to 
    /// get more specifications.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public SpriteFont RouliFont;
        public const int screenWidth = 1280;
        public const int screenHeight = 720;
        public List<MiniGame> miniGames;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // MiniGameManager.AddMiniGame(new KeyboardGame());
            MiniGameManager.AddMiniGame(new DabGame());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            RouliFont = Content.Load<SpriteFont>("Rouli");
            MiniGameManager.LoadContent(Content); // This will call the LoadContent function of your minigame class
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime);
            MiniGameManager.Update(gameTime); // This will call your minigame's update function for you
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            MiniGameManager.Draw(spriteBatch); // And this will call your minigame's draw function!
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
