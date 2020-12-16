using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GordonWare
{
    class RecognitionGame : MiniGame
    {
        const int MODIFIERSNUMBER = 11;
        const int GORDONNUMBER = 6;

        Sprite cursor;
        Sprite baseGordon;
        Sprite select;
        Sprite[] modificationsSprites;
        List<int>[] modifiers;
        Nullable<int> selectedGordon = null;
        int realGordon;

        Random random;

        public RecognitionGame()
        {
            name = "Recognition Game";
            description = "Clique sur le Gordon original";
            description_color = Color.Black;
            author = "Efflam";

            modifiers = new List<int>[GORDONNUMBER];
            modificationsSprites = new Sprite[MODIFIERSNUMBER];
            random = new Random();
            //Reset();
        }

        public override void Reset()
        {
            realGordon = random.Next() % GORDONNUMBER;
            for (int i = 0; i < GORDONNUMBER; i++) {
                if (i != realGordon)
                    modifiers[i] = new List<int> { random.Next(MODIFIERSNUMBER) };
                else
                    modifiers[i] = new List<int>();
            }
            for (int _ = 0; _ < GORDONNUMBER; _++) {
                int i = random.Next(GORDONNUMBER);
                if (i != realGordon)
                    modifiers[i].Add(random.Next(MODIFIERSNUMBER));
            }
            base.Reset();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            base.LoadContent(Content);
            background = new Sprite(Content.Load<Texture2D>("recognitiongame/background"));
            cursor = new Sprite(Content.Load<Texture2D>("recognitiongame/hand"));
            select = new Sprite(Content.Load<Texture2D>("recognitiongame/select"));
            baseGordon = new Sprite(Content.Load<Texture2D>("recognitiongame/baseGordon"));
            for (int i=0; i<MODIFIERSNUMBER; i++) {
                modificationsSprites[i] = new Sprite(Content.Load<Texture2D>("recognitiongame/modif" + i));
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (game_status == GameStatus.Pending)
            {
                MouseState ms = Mouse.GetState();
                selectedGordon = null;
                for (int x = 0; x < 3; x++) {
                    for (int y = 0; y < 2; y++) {
                        Vector2 relativePos = new Vector2(ms.X, ms.Y) - imagePosition(x, y);
                        if (0 <= relativePos.X && relativePos.X < 224 && 0 <= relativePos.Y && relativePos.Y < 260) {
                            selectedGordon = imageId(x, y);
                        }
                    }
                }
                if (ms.LeftButton == ButtonState.Pressed && selectedGordon != null) {
                    if (selectedGordon == realGordon)
                        Win();
                    else
                        Lose();
                }
            }
            else if (game_status == GameStatus.Win)
            {
                // This is mandatory, but you can also allow the player to do something else if he won 
                // (there's a short window of time after win before going to the transition screen)
            }
            else if (game_status == GameStatus.Lose)
            {
                // You can here mock the player for losing for the same short period of time
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            for (int x=0; x<3; x++) {
                for (int y=0; y<2; y++) {
                    Vector2 position = imagePosition(x, y);
                    baseGordon.TopLeftDraw(spriteBatch, position);
                    int i = imageId(x, y);
                    foreach (int modif in modifiers[i]) {
                        modificationsSprites[modif].TopLeftDraw(spriteBatch, position);
                    }
                    if (selectedGordon == i) {
                        select.TopLeftDraw(spriteBatch, position);
                    }
                }
            }
            MouseState ms = Mouse.GetState();
            cursor.TopLeftDraw(spriteBatch, new Vector2(ms.X, ms.Y));

            base.Draw(spriteBatch);
        }

        Vector2 imagePosition(int x, int y) {
            return new Vector2(172 + 336 * x, 100 + 322 * y);
        }
        int imageId(int x, int y) {
            return (x + 3 * y) % GORDONNUMBER;
        }
    }
}
