using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;

namespace GordonWare
{
    class BananeGame : MiniGame
    {
        public enum GordonState { Attente = 0, Heureux = 1, Colere = 2}
        public enum Destination { Gauche = 0, Droite = 1, None = 2}

        // Add your local variable with their type here (example : int Speed)
        private string[] inWords, outWords, allWords;
        private string mot;
        private GordonState gordonState;
        private Destination dest, input;
        private float tailleTexte;
        private Sprite banane, gordonAttente, gordonHeureux, gordonColere, croix, check, signLeft, signRight;
        private Vector2 offsetTexte;
       
        private Random random;
        private SpriteFont arial;

        public BananeGame()
        {
            name = "Banane Game";
            description = "Aide Gordon a ranger les bons objets dans la banane PT";
            description_color = Color.White; // Color of the description/timer, black or white is better
            author = "martial";

            inWords = new string[] { "Raclette", "Algorithme", "Pecha Kucha", "Université", "Internet", "Extradoc"};
            outWords = new string[] { "Banane", "Gordon", "Fromage", "Bière", "Linux", "Madoc" };
            allWords = new string[inWords.Length + outWords.Length];
            inWords.CopyTo(allWords, 0);
            outWords.CopyTo(allWords, inWords.Length);
        }

        
        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) input = Destination.Gauche; //( à remplacer par souris ptetre)
                if (Keyboard.GetState().IsKeyDown(Keys.Right)) input = Destination.Droite;

                if(input != Destination.None)
                {
                    if (input == dest) base.Win();
                    else base.Lose();
                }
            }
            else if (game_status == GameStatus.Win)
            {  
                if (dest == Destination.Gauche)
                {
                    offsetTexte.X-=5;
                    offsetTexte.Y+=5;
                    //translation vers banane
                }
                else
                {
                    offsetTexte.X+=5;
                    offsetTexte.Y+=5;
                    gordonState = GordonState.Heureux;
                    //translation vers gordon
                }
            }
            else if (game_status == GameStatus.Lose)
            {
                if(dest == Destination.Gauche){ //si destination banane + erreur
                    gordonState = GordonState.Colere;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // The final function. Here's where you decide what to display, how to display it and where to display it.
            // Life in the Update function, you can create a seperate draw depending on the game_status parameter if you want.
            // first, you have to draw the background, then everything that's above (from back to front)
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            //banane
            banane.TopLeftDraw(spriteBatch, new Vector2(200, 250), 1f, 1.7f);

            //gordon

            Vector2 posGordon = new Vector2(800, 350);
            if (gordonState == GordonState.Attente) gordonAttente.TopLeftDraw(spriteBatch, posGordon, 1f, 4.5f);
            else if (gordonState == GordonState.Heureux) gordonHeureux.TopLeftDraw(spriteBatch, posGordon, 1f, 4.5f);
            else gordonColere.TopLeftDraw(spriteBatch, posGordon, 1f, 4.5f);

            //mot à classer
            drawString(spriteBatch, arial, mot, offsetTexte, Color.White);

            signLeft.TopLeftDraw(spriteBatch, new Vector2(150, 100), 1f, 1f);
            signRight.TopLeftDraw(spriteBatch, new Vector2(1280-150-signRight.frameWidth, 100), 1f, 1f);

            if (game_status == GameStatus.Win)
            {
                check.TopLeftDraw(spriteBatch, new Vector2(400, 100), 1f, 2f);
            }
            else if (game_status == GameStatus.Lose)
            {
                croix.TopLeftDraw(spriteBatch, new Vector2(400, 100), 1f, 2f);
            }

            base.Draw(spriteBatch); // Above your minigame, the description and timer are drawn so donc forget to call MiniGame.Draw() with this.
        }

        private void drawString(SpriteBatch spriteBatch, SpriteFont font, string texte, Vector2 offset, Color couleur){
            Vector2 size = font.MeasureString(texte); // donne les dimentions en px du texte

            Vector2 position = new Vector2(1280/2-2*size.X/3 +offset.X, 200+offset.Y); //fine tuning the position

             
            spriteBatch.DrawString(font, texte, position, couleur, 0f, new Vector2(0, 0), 1.5f, SpriteEffects.None, 0f);
        }

        public override void Reset()
        {
            // Please add the resetting of your game here, such as new random generation

            base.Reset(); // This calls MiniGame.Reset() to reset other things

            random = new Random();
            int mot_index = random.Next(0, allWords.Length);
            mot = allWords[mot_index]; //mot à classer

            if (inWords.Contains(mot)) dest = Destination.Gauche; //appartient à la banane
            else dest = Destination.Droite;

            input = Destination.None;

            gordonState = GordonState.Attente;
            tailleTexte = 1f;
            offsetTexte = new Vector2(0, 0);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content); // This calls MiniGame.LoadContent() in oreder to load stuff mandatory for every minigame, such as the font.

            background = new Sprite(Content.Load<Texture2D>("bananegame/background"));
            banane = new Sprite(Content.Load<Texture2D>("bananegame/banane_t"));
            gordonHeureux = new Sprite(Content.Load<Texture2D>("bananegame/gordon_heureux"));
            gordonColere = new Sprite(Content.Load<Texture2D>("bananegame/gordon_colere"));
            gordonAttente = new Sprite(Content.Load<Texture2D>("bananegame/gordon_attente"));
            croix = new Sprite(Content.Load<Texture2D>("bananegame/red-cross"));
            check = new Sprite(Content.Load<Texture2D>("bananegame/green-check"));

            signLeft = new Sprite(Content.Load<Texture2D>("bananegame/sign-left"));
            signRight = new Sprite(Content.Load<Texture2D>("bananegame/sign-right"));

            arial = Content.Load<SpriteFont>("keyboardgame/arial");
        }
    }
}
