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
        Sprite idle, walk, jump, pizza, dark_pizza, CurrentSprite, win;
        Vector2 PlayerPosition, PlayerVelocity;
        int direction, movement_direction; // direction of the player, direction of the movement
        int pizza_count, total_pizza_count;
        float pizza_height; // used for pizza movement animation
        Rectangle player_hitbox; // used for pizza detection
        const int GROUND_LEVEL = 650;
        List<Vector2> PizzaPositions;
        List<Tuple<Vector2, Vector2, Sprite>> DisplayObjects; // first item of tuple is feet location, second item is velocity and third is sprite
        SoundEffect yay, pizza_collected, jump_sound;
        Random r;
        Song song;
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
            DisplayObjects = new List<Tuple<Vector2, Vector2, Sprite>>() { };
            PlayerPosition = new Vector2(1280/2, GROUND_LEVEL);
            PlayerVelocity = Vector2.Zero;
            CurrentSprite = idle;
            direction = 0;
            pizza_count = 0;
            movement_direction = 0;
            pizza_height = 0;
            PizzaPositions = new List<Vector2>() { new Vector2(100, 200), new Vector2(250, 200), new Vector2(400, 200), new Vector2(550, 200)
                , new Vector2(1280 - 550, 200),new Vector2(1280 - 400, 200), new Vector2(1280 - 250, 200), new Vector2(1280 - 100, 200)};
            int index = new Random().Next(0, 4); // 0, 1, 2
            Console.WriteLine(index);
            r = new Random();

            List<Vector2> SelectedPizzaPositions = new List<Vector2>() { };
            for (int i = index; i < index + 4; i++)
            {
                Vector2 v = PizzaPositions[i];
                if (Math.Abs(v.X - 1280 / 2) < 200) v.Y = r.Next(150, 350);
                else v.Y = r.Next(150, 500);
                SelectedPizzaPositions.Add(v);
            }

            PizzaPositions = SelectedPizzaPositions;

            total_pizza_count = SelectedPizzaPositions.Count;
            MediaPlayer.Play(song, new TimeSpan(0, 0, 15));
            base.Reset();
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            idle = new Sprite(2,73,85,300, Content.Load<Texture2D>("pizzagame/idle"), 5);
            walk = new Sprite(2,73,85,150, Content.Load<Texture2D>("pizzagame/walk"), 5);
            jump = new Sprite(Content.Load<Texture2D>("pizzagame/jump"), 5);
            background = new Sprite(3, 256, 144, 300, Content.Load<Texture2D>("pizzagame/scene"), 5);
            pizza = new Sprite(Content.Load<Texture2D>("pizzagame/pizza"), 5);
            jump_sound = Content.Load<SoundEffect>("pizzagame/jump_s");
            yay = Content.Load<SoundEffect>("pizzagame/yay");
            pizza_collected = Content.Load<SoundEffect>("pizzagame/collected");
            dark_pizza = new Sprite(Content.Load<Texture2D>("pizzagame/pizza_dead"), 5);
            win= new Sprite(2, 51, 77, 180, Content.Load<Texture2D>("pizzagame/party"), 5);
            song = Content.Load<Song>("music/Anttis instrumentals - 1345 Instrumental");

            base.LoadContent(Content); 
        }
        public override void Update(GameTime gameTime)
        {
            movement_direction = 0;

            if (Input.keyboard.IsKeyDown(Keys.Left) && !Input.keyboard.IsKeyDown(Keys.Right)) movement_direction = -1;
            else if (!Input.keyboard.IsKeyDown(Keys.Left) && Input.keyboard.IsKeyDown(Keys.Right)) movement_direction = 1;

            if (PlayerPosition.Y >= GROUND_LEVEL) // ground or below ground
            {
                if (game_status == GameStatus.Win)
                {
                    CurrentSprite = win;
                    PlayerPosition.Y = GROUND_LEVEL;
                    PlayerVelocity.Y = 0;
                    PlayerVelocity.X = 0;
                }
                else
                {
                    if (Input.keyboard.IsKeyDown(Keys.Up) || Input.keyboard.IsKeyDown(Keys.Z))
                    {
                        PlayerVelocity.Y = -35;
                        CurrentSprite = jump;
                        jump_sound.Play();
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
                        int speed = 9;
                        if(MiniGameManager.GetScore() > 9) speed = MiniGameManager.GetScore();
                        PlayerVelocity.X = movement_direction * speed;
                    }
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
                player_hitbox = new Rectangle((int)PlayerPosition.X - 100, (int)PlayerPosition.Y - 300, 200, 300);
                Vector2 pizza_to_remove = Vector2.Zero;
                foreach (Vector2 v in PizzaPositions) if (player_hitbox.Contains(v))
                    {
                        PizzaPositions.Remove(v);
                        pizza_count++;
                        if (pizza_count == total_pizza_count)
                        {
                            Win();
                            yay.Play();
                        }
                        pizza_to_remove = v;
                        pizza_collected.Play();
                        break;
                    }
                if (pizza_to_remove != Vector2.Zero) PizzaPositions.Remove(pizza_to_remove);
            }
            else if (game_status == GameStatus.Lose)
            {
                if(PizzaPositions.Count > 0)
                {
                    foreach (Vector2 v in PizzaPositions) DisplayObjects.Add(new Tuple<Vector2, Vector2, Sprite>(v, new Vector2(r.Next(-5,5), -10), dark_pizza));
                    PizzaPositions.Clear();
                }
            }
            
            pizza_height = 10 * (float)Math.Cos(timer / 200);

            PlayerPosition += PlayerVelocity;
            CurrentSprite.direction = direction;

            List<Tuple<Vector2, Vector2, Sprite>> // this is dirty I'm sorry if you're reading this
                UpdatedObjects = new List<Tuple<Vector2, Vector2, Sprite>>() { };

            foreach (Tuple<Vector2, Vector2, Sprite> o in DisplayObjects)
            {
                Vector2 NewPosition = o.Item1 + o.Item2;
                Vector2 NewVelocity = o.Item2 + new Vector2(0, 1); // Gravity
                Tuple<Vector2, Vector2, Sprite> UpdatedObject = new Tuple<Vector2, Vector2, Sprite>(NewPosition, NewVelocity, o.Item3);
                UpdatedObjects.Add(UpdatedObject);
            }
            DisplayObjects.Clear();
            DisplayObjects.AddRange(UpdatedObjects);

            CurrentSprite.UpdateFrame(gameTime);
            background.UpdateFrame(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End(); // to get some crispy pixels <e're gonna call the spritebatch with new parameters
            spriteBatch.Begin(SpriteSortMode.Deferred,
              BlendState.AlphaBlend,
              SamplerState.PointClamp,
              null, null, null, null);

            background.TopLeftDraw(spriteBatch, new Vector2(0, 0));

            CurrentSprite.DrawFromFeet(spriteBatch, PlayerPosition);
            foreach(Vector2 v in PizzaPositions) pizza.DrawFromFeet(spriteBatch, v + new Vector2(0, pizza_height));
            foreach (Tuple<Vector2, Vector2, Sprite> o in DisplayObjects) o.Item3.DrawFromFeet(spriteBatch, o.Item1);

            spriteBatch.End();
            spriteBatch.Begin();

            base.Draw(spriteBatch);
        }
    }
}
