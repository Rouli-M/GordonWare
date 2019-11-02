using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GordonWare
{
    /// <summary>
    /// Un sprite est une image, animée ou non. 
    /// </summary>
    public class Sprite
    {
        public Rectangle Source; // Rectangle permettant de définir la zone de la texture à afficher (la zone de la frame de notre spritesheet)
        public float time; // Durée depuis laquelle la frame est à l'écran (on la remettra à zéro à chaque nouvelle frame, sert à mesurer depuis quand on a affiché la frame actuelle)
        public int frameIndex; // Indice de l'image en cours, de 0 à (nombre de frames - 1)
        public bool isAnimated, reverse;
        public bool isOver, firstFrame, lastFrame;
        public int totalFrames { get; private set; }
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        public int timeBetweenFrames { get; private set; }
        public Texture2D Texture { get; private set; }
        public float scale { get; private set; }
        public SpriteEffects effects;
        public bool loopAnimation { get; private set; }
        public int direction;

        public Sprite()
        {

        }

        public Sprite(Texture2D Texture, float scale = 1f, int direction = 1) // Constructeur d'un sprite non animé, et donc appelé avec uniquement une texture et de manière facultative une échelle de taille.
        {
            this.Texture = Texture;
            this.scale = scale;
            isAnimated = false;
            frameHeight = Convert.ToInt32(scale * Texture.Height);
            frameWidth = Convert.ToInt32(scale * Texture.Width);
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
        }
        public Sprite(int totalAnimationFrames, int frameWidth, int frameHeight, int timeBetweenFrames, Texture2D Texture, float scale = 1f, bool loopAnimation = true, bool reverse = false, int direction = 1, int normalFrameWidth = 0) // Autre constructeur pour quand un gameObject est créé avec les paramètres ci-contre
        {
            this.totalFrames = totalAnimationFrames;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.timeBetweenFrames = timeBetweenFrames;
            this.Texture = Texture;
            this.scale = scale;
            this.loopAnimation = loopAnimation;
            this.direction = direction;
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            this.reverse = reverse;
            isAnimated = true;
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position, float Angle = 0f, float Zoom = 1f) // Dessin du sprite depuis son centre
        {
            spriteBatch.Draw(Texture, Position, Source, Color.White, Angle, new Vector2(Texture.Width/2, Texture.Height/2), scale * Zoom, effects, 1f); // On remarque l'ajoute de l'argument Source qui corresponds à la zone de la spritesheet qu'on dessine (voir comment elle est sélectionnée plus bas)
        }

        public void DrawFromFeet(SpriteBatch spriteBatch, Vector2 FeetPosition, float Angle = 0f, float Zoom = 1f) // Dessin du sprite à partir de la position des pieds (pied = milieu de l'image en bas) s'il y en a une
        {
            spriteBatch.Draw(Texture, FeetPosition + scale * Zoom * new Vector2(-0.5f * frameWidth, -frameHeight), Source, Color.White, Angle, new Vector2(0, 0), scale * Zoom, effects, 1f); // on obtient le point en haut à gauche à partir de la position des pieds
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position, Rectangle Source, float Zoom = 1f) // Pour dessiner une partie d'une image fixe. La position = la position du centre de l'image
        {
            spriteBatch.Draw(Texture, Position - new Vector2(Zoom * this.frameWidth / 2, scale * Zoom * this.frameHeight / 2), Source, Color.White, 0f, new Vector2(0, 0), scale * Zoom, effects, 1f);
        }

        public void TopLeftDraw(SpriteBatch spriteBatch, Vector2 Position, float Opacity = 1f, float Zoom = 1f) // Pour dessiner une partie d'une image, en donnant sa position d'affichage en haut à droite
        {
            spriteBatch.Draw(Texture, Position, Source, Color.White * Opacity, 0f, new Vector2(0, 0), this.scale * Zoom, effects, 1f);
        }

        public void UpdateFrame(GameTime gameTime) // Pour décider quelle frame on affiche
        {
            isOver = false;
            lastFrame = false;
            if (direction == -1) effects = SpriteEffects.FlipHorizontally;
            else effects = SpriteEffects.None;
            if (isAnimated)
            {
                if (time == 0 && frameIndex == 0) firstFrame = true;
                else firstFrame = false;

                time += (float)gameTime.ElapsedGameTime.TotalMilliseconds; // Le temps en millisecondes depuis le dernier changement de frame

                if (time > timeBetweenFrames)
                {
                    if (reverse) frameIndex--; else frameIndex++; // Frame suivante
                    time = 0f; // On remet le temps à 0
                }
                if (time + (float)gameTime.ElapsedGameTime.TotalMilliseconds > timeBetweenFrames && (frameIndex == totalFrames - 1))
                {
                    lastFrame = true;
                }
                if ((frameIndex >= totalFrames || frameIndex < 0)) // Si le numéro de la frame dépasse le nombre de frames
                {
                    if (loopAnimation)
                    {
                        if (reverse) frameIndex = totalFrames;
                        else frameIndex = 0; // On retourne à la première frame
                    }
                    else
                    {
                        if (reverse) frameIndex++;
                        else frameIndex--; // retour à la frame précédente
                    }

                    isOver = true;
                }

                Source = new Rectangle(( // Le rectangle de la frame
                        frameIndex % (Texture.Width / frameWidth)) * frameWidth,
                        ((frameIndex - (frameIndex % (Texture.Width / frameWidth))) / (Texture.Width / frameWidth)) * frameHeight,
                        frameWidth, 
                        frameHeight); 
            }
            else
            {
                if (Texture != null) Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            }
        }

        public void ResetAnimation()
        {
            isOver = false;
            if (!reverse) frameIndex = 0;
            else frameIndex = totalFrames - 1;
            time = 0;
        }

    }
}