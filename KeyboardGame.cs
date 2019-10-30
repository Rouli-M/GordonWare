using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GordonWare
{
    class KeyboardGame : MiniGame
    {
        public KeyboardGame(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            background = new Sprite(Content.Load<Texture2D>("keyboardgame/background")); // On appelle le constructeur de Sprite avec un argument texture2D
        }

        new public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        new public void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            base.Draw(spriteBatch); // Par dessus chaque mini jeu est dessiné certains éléments comme la description mais aussi le timer
        }
    }
}
