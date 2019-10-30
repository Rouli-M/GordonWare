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
        public int i; // Variable utile pour les boucles for
        public Rectangle Source, TextureSize; // Rectangle permettant de définir la zone de la texture à afficher (la zone de la frame de notre spritesheet)
        public float time; // Durée depuis laquelle la frame est à l'écran (on la remettra à zéro à chaque nouvelle frame, sert à mesurer depuis quand on a affiché la frame actuelle)
        public int frameIndex; // Indice de l'image en cours, de 0 à (nombre de frames - 1)
        public bool isAnimated, reverse;
        public bool isOver, firstFrame, lastFrame;
        public Vector2 CurrentPivotPosition;

        private int _totalFrames; // Nombre total de frames du GameObject que l'on récupère quand il est créé
        public int totalFrames
        {
            get { return _totalFrames; }
        }
        private int _frameWidth; // Lagueur de la frame du GameObject que l'on récupère quand il est créé
        public int frameWidth
        {
            get { return _frameWidth; }
        }
        private int _frameHeight; // Hauteur de la frame du GameObject que l'on récupère quand il est créé
        public int frameHeight
        {
            get { return _frameHeight; }
        }
        private int _timeBetweenFrames; // durée entre chaque frame du GameObject que l'on récupère quand il est créé
        public int timeBetweenFrames
        {
            get { return _timeBetweenFrames; }
        }
        private Texture2D _Texture;
        public Texture2D Texture
        {
            get { return _Texture; }
        }
        private float _scale;
        public float scale
        {
            get { return _scale; }
        }
        private bool _loopAnimation;
        public bool loopAnimation
        {
            get { return _loopAnimation; }
        }
        private int _direction;
        public int direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private int _normalFrameWidth;
        public int normalFrameWidth
        {
            get { return _normalFrameWidth; }
        }
        private List<Vector2> _pivot;
        public List<Vector2> pivot
        {
            get { return _pivot; }
        }

        public Sprite()
        {

        }

        public Sprite(Texture2D Texture, float scale = 1f) // Constructeur d'un sprite non animé, et donc appelé avec uniquement une texture et de manière facultative une échelle de taille.
        {
            this._Texture = Texture;
            this._scale = scale;
            isAnimated = false;
            _frameHeight = Convert.ToInt32(scale * _Texture.Height);
            _frameWidth = Convert.ToInt32(scale * _Texture.Width);
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            TextureSize = new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
        public Sprite(int totalAnimationFrames, int frameWidth, int frameHeight, int timeBetweenFrames, Texture2D Texture, float scale = 1f, bool loopAnimation = true, bool reverse = false, int direction = 1, int normalFrameWidth = 0) // Autre constructeur pour quand un gameObject est créé avec les paramètres ci-contre
        {
            _totalFrames = totalAnimationFrames;
            _frameWidth = frameWidth;
            _frameHeight = frameHeight;
            _timeBetweenFrames = timeBetweenFrames;
            _Texture = Texture;
            _scale = scale;
            _loopAnimation = loopAnimation;
            this._direction = direction;
            _normalFrameWidth = normalFrameWidth;
            _pivot = null;
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            this.reverse = reverse;
            if (normalFrameWidth == 0) _normalFrameWidth = frameWidth;
            isAnimated = true;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position, float Angle = 0f, float Zoom = 1f, SpriteEffects effects = SpriteEffects.None, bool UISprite = false) // Pour dessiner l'animation s'il y en a une
        {
            if (isAnimated) spriteBatch.Draw(Texture, (Position), Source, Color.White, Angle, new Vector2(0, 0), scale * Zoom, (_direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f); // On remarque l'ajoute de l'argument Source qui corresponds à la zone de la spritesheet qu'on dessine (voir comment elle est sélectionnée plus bas)
            else spriteBatch.Draw(Texture, Position, new Rectangle(0, 0, frameWidth, frameHeight), Color.White, Angle, new Vector2(0, 0), scale * Zoom, effects, 1f);
        }

        public void DrawFromFeet(SpriteBatch spriteBatch, Vector2 FeetPosition, float Angle = 0f, float Zoom = 1f, SpriteEffects effects = SpriteEffects.None, bool UISprite = false) // Pour dessiner l'animation à partir de la position des pieds (pied = milieu de l'image en bas) s'il y en a une
        {
            if (isAnimated) spriteBatch.Draw(Texture, FeetPosition + scale * Zoom * new Vector2(-0.5f * frameWidth, -frameHeight), Source, Color.White, Angle, new Vector2(0, 0), scale * Zoom, (_direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f); // on obtient le point en haut à gauche à partir de la position des pieds
            else spriteBatch.Draw(Texture, FeetPosition + Zoom * new Vector2(-0.5f * frameWidth, -frameHeight), Source, Color.White, Angle, new Vector2(0, 0), scale * Zoom, (_direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f); // on obtient le point en haut à gauche à partir de la position des pieds
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position, Rectangle Source, float Zoom = 1f, SpriteEffects effects = SpriteEffects.None) // Pour dessiner une partie d'une image fixe. La position = la position du centre de l'image
        {
            spriteBatch.Draw(Texture, Position - new Vector2(Zoom * _frameWidth / 2, scale * Zoom * _frameHeight / 2), Source, Color.White, 0f, new Vector2(0, 0), scale * Zoom, effects, 1f);
        }

        public void TopLeftDraw(SpriteBatch spriteBatch, Vector2 Position, float Opacity = 1f, float Zoom = 1f, SpriteEffects effects = SpriteEffects.None) // Pour dessiner une partie d'une image, en donnant sa position d'affichage en haut à droite
        {
            spriteBatch.Draw(Texture, Position, TextureSize, Color.White * Opacity, 0f, new Vector2(0, 0), _scale * Zoom, (_direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 1f);
        }

        public void UpdateFrame(GameTime gameTime) // Pour décider quelle frame on affiche
        {
            isOver = false;
            lastFrame = false;
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
                                            // Les exemples ci-dessous sont données pour le cas d'une spritesheet avec 5 frames de longueur, comme c'est le cas de arno_run.png, mais il marche également pour d'autres grilles.
                        frameIndex % (Texture.Width / frameWidth)) * frameWidth, // L'abscisse du point en haut à gauche de chaque frame de la spritesheet. Parcours gloabalement comme ceci en fonction de frameIndex : (longueur d'une frame) * 1, *2 , *3, *4, *5 puis re *1, *2, *3 etc.
                        ((frameIndex - (frameIndex % (Texture.Width / frameWidth))) / (Texture.Width / frameWidth)) * frameHeight, // L'ordonnée de ce point. Parcours comme ceci : (hauteur d'une frame) * 0, *0, *0, *0, *0, *1, *1, *1, *1, *1, *2, *2
                        frameWidth, // Largeur de la frame à découper à partir du point en haut à gauche (positif donc)
                        frameHeight); // Hauteur à "découper"

                if (pivot != null) CurrentPivotPosition = pivot[frameIndex];

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