using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace GordonWare
{
    class PizzaGame : MiniGame
    {
        Sprite idle, walk, jump, pizza, CurrentSprite;
        Vector2 PlayerPosition, PlayerVelocity;
        int direction, movement_direction; // direction of the player, direction of the movement
        int pizza_count, total_pizza_count;
        float pizza_height; // used for pizza movement animation
        Rectangle player_hitbox; // used for pizza detection
        const int GROUND_LEVEL = 650;
        List<Vector2> PizzaPositions;
        public PizzaGame()
        {
            name = "pizza game";
            description = "Ramasse les tranches de pizza !";
            description_color = Color.Black;
            author = "Martin";
            CurrentSprite = idle;
        }

        public override void Reset()
        {
            PlayerPosition = new Vector2(1280/2, GROUND_LEVEL);
            PlayerVelocity = Vector2.Zero;
            CurrentSprite = idle;
            direction = 0;
            pizza_count = 0;
            movement_direction = 0;
            pizza_height = 0;
            PizzaPositions = new List<Vector2>() { new Vector2(150, 200), new Vector2(450, 200), new Vector2(750, 200), new Vector2(1050, 200), };
            total_pizza_count = PizzaPositions.Count;
            base.Reset();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            idle = new Sprite(2,73,85,300, Content.Load<Texture2D>("pizzagame/idle"), 5);
            walk = new Sprite(2,73,85,150, Content.Load<Texture2D>("pizzagame/walk"), 5);
            jump = new Sprite(Content.Load<Texture2D>("pizzagame/jump"), 5);
            background = new Sprite(Content.Load<Texture2D>("pizzagame/background"), 5);
            pizza = new Sprite(Content.Load<Texture2D>("pizzagame/pizza"), 5);
            base.LoadContent(Content); 
        }
        public override void Update(GameTime gameTime)
        {
            movement_direction = 0;

            if (Input.keyboard.IsKeyDown(Keys.Left) && !Input.keyboard.IsKeyDown(Keys.Right)) movement_direction = -1;
            else if (!Input.keyboard.IsKeyDown(Keys.Left) && Input.keyboard.IsKeyDown(Keys.Right)) movement_direction = 1;

            if (PlayerPosition.Y >= GROUND_LEVEL) // ground or below ground
            {
                if (Input.keyboard.IsKeyDown(Keys.Up) || Input.keyboard.IsKeyDown(Keys.Z))
                {
                    PlayerVelocity.Y = -35;
                    CurrentSprite = jump;
                }
                else
                {
                    PlayerPosition.Y = GROUND_LEVEL;
                    PlayerVelocity.Y = 0;
                    CurrentSprite = idle;
                }

                if (movement_direction != 0)
                {
                    CurrentSprite = walk;
                    PlayerVelocity.X = movement_direction * 9;
                }
            }
            else
            {
                CurrentSprite = jump;
                PlayerVelocity.Y += 2.9f; // gravity

                if (movement_direction != 0)
                {
                    PlayerVelocity.X = movement_direction * 5;
                }
            }

            if (movement_direction == 0) PlayerVelocity.X *= 0.8f;
            else direction = movement_direction;

            if (game_status == GameStatus.Pending)
            {
                player_hitbox = new Rectangle((int)PlayerPosition.X - 100, (int)PlayerPosition.Y - 400, 200, 400);
                Vector2 pizza_to_remove = Vector2.Zero;
                foreach (Vector2 v in PizzaPositions) if (player_hitbox.Contains(v))
                    {
                        PizzaPositions.Remove(v);
                        pizza_count++;
                        if (pizza_count == total_pizza_count) base.Win();
                        pizza_to_remove = v;
                        break;
                    }
                if (pizza_to_remove != Vector2.Zero) PizzaPositions.Remove(pizza_to_remove);
            }
            else if (game_status == GameStatus.Win)
            {

            }
            else if (game_status == GameStatus.Lose)
            {
                
            }
            
            pizza_height = 10 * (float)Math.Cos(timer / 200);

            PlayerPosition += PlayerVelocity;
            CurrentSprite.direction = direction;
            

            CurrentSprite.UpdateFrame(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            CurrentSprite.DrawFromFeet(spriteBatch, PlayerPosition);
            foreach(Vector2 v in PizzaPositions) pizza.DrawFromFeet(spriteBatch, v + new Vector2(0, pizza_height));

            base.Draw(spriteBatch);
        }
    }
}
