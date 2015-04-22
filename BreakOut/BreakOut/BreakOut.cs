using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;

namespace BreakOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BreakOut : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont GameFont;

        SoundEffect brickSound;
        SoundEffectInstance brickSoundInstance;

        SoundEffect paddleSound;
        SoundEffectInstance paddleSoundInstance;

        SoundEffect wallSound;
        SoundEffectInstance wallSoundInstance;

        SoundEffect lifeSound;
        SoundEffectInstance lifeSoundInstance;
        
        SoundEffect gameoverSound;
        SoundEffectInstance gameoverSoundInstance; 

        Ball ball;
        Paddle paddle;
        Rectangle screen;


        int lifes = 3;
        int level = 1;
        public static int playerScore = 10;
        
        int bricksW = 10;
        int bricksH = 5;
        int brickTotal = 0;
        Texture2D brickImage;

        float timeForSpeedUpgrade = 0f;

        Brick[,] bricks;
        Brick[,] temp_bricks;

        int GameState = 1;
        bool levelChanging = false;

        public BreakOut()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 750;
            graphics.PreferredBackBufferHeight = 600;

            screen = new Rectangle(
                0,
                0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);
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

            Texture2D tempTexture = Content.Load<Texture2D>("paddle_normal");
            paddle = new Paddle(tempTexture, screen);

            tempTexture = Content.Load<Texture2D>("ball_normal");
            ball = new Ball(tempTexture, screen);

            brickImage = Content.Load<Texture2D>("brick");

            GameFont = Content.Load<SpriteFont>("GameFont");

            brickSound = Content.Load<SoundEffect>("brick_sound");
            brickSoundInstance = brickSound.CreateInstance();

            paddleSound = Content.Load<SoundEffect>("paddle_hit");
            paddleSoundInstance = paddleSound.CreateInstance();

            wallSound = Content.Load<SoundEffect>("wall_hit");
            wallSoundInstance = wallSound.CreateInstance();

            lifeSound = Content.Load<SoundEffect>("life_lost");
            lifeSoundInstance = lifeSound.CreateInstance();

            gameoverSound = Content.Load<SoundEffect>("dead");
            gameoverSoundInstance = gameoverSound.CreateInstance();

            StartGame(level);
        }

        private void StartGame(int level){
            brickTotal = bricksW * (bricksH);
            paddle.SetInStartPosition();
            ball.SetInStartPosition(paddle.GetBounds());
            Random rnd = new Random();
            temp_bricks = new Brick[bricksW, bricksH];

            for (int y = 0; y < bricksH; y++){
                brickImage = Content.Load<Texture2D>("brick");
                bool alive = true;
                int type = 1;
                int max = 3;
                for (int x = 0; x < 10; x++){
                    Color tint = new Color(247,247,247);
                    switch (level) { 
                        case 1:
                            type = rnd.Next(1, 2);
                            break;
                        case 2:
                            type = rnd.Next(1, 4);
                            break;
                        case 3:
                            type = rnd.Next(1, 5);
                            break;
                        case 4:
                            type = rnd.Next(1, 6);
                            break;
                    }
                    if (max <= 0)
                        type = 1;
                    else if(x <= rnd.Next(0,4))
                        type = 1; 
                    switch (type) { 
                        case 2:
                            tint = new Color(255, 85, 43);
                            break;
                        case 3:
                            tint = new Color(76,76,76);
                            break;
                        case 4:
                            tint = new Color(165,0,170);
                            break;
                        case 5:
                            tint = new Color(132, 102, 50);
                            break;
                    }
                    if (type > 1)
                        max--;
                    temp_bricks[x, y] = new Brick(
                    brickImage,
                    new Rectangle(
                    x * brickImage.Width,
                    y * brickImage.Height,
                    brickImage.Width,
                    brickImage.Height),
                    tint,
                    alive,
                    type); 
                }
            }
            bricks = new Brick[bricksW, bricksH + (level - 1)];
            bricks = temp_bricks;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            switch (GameState) {
                case 1:
                    UpdateStarted(gameTime);
                    break;
                case 2:
                    UpdatePlaying(gameTime);
                    break;
                case 3:
                    UpdateChanging(gameTime);
                    break;
                case 4:
                    UpdateEnded(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(249,249,247));

            switch (GameState) {
                case 1:
                    DrawStarted(gameTime);
                    break;
                case 2:
                    DrawPlaying(gameTime);
                    break;
                case 3:
                    DrawChangingLevel(gameTime);
                    break;
                case 4:
                    DrawEnded(gameTime);
                    break;
            }
            base.Draw(gameTime);
        }


        public void DrawStarted(GameTime currentTime){
            spriteBatch.Begin();
            spriteBatch.DrawString(GameFont, "breakout.", new Vector2(60, 200), Color.Black);
            spriteBatch.DrawString(GameFont, "this game has 4 levels, on each one I'll introduce new challenges! good luck.", new Vector2(60, 250), Color.Black);
            spriteBatch.DrawString(GameFont, "press 's' to start", new Vector2(60, 300), Color.Black);
            spriteBatch.DrawString(GameFont, "by Rafael Pato.", new Vector2(60, 550), Color.Black);
            spriteBatch.End();
        }
        public void DrawChangingLevel(GameTime currentTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(GameFont, "level " + level + "." , new Vector2(60, 200), Color.Black);
            
            switch (level) { 
                case 2:
                    spriteBatch.DrawString(GameFont, "red bricks will make you faster and grey bricks will make you slower.", new Vector2(60, 250), Color.Black);
                    spriteBatch.DrawString(GameFont, "choose wisely when to use them...", new Vector2(60, 270), Color.Black);
                      
                    spriteBatch.DrawString(GameFont, "press 'r' when ready.", new Vector2(60, 300), Color.Black);
                    break;
                case 3:
                    spriteBatch.DrawString(GameFont, "pink makes everything slower, don't get bored.", new Vector2(60, 250), Color.Black);
                    spriteBatch.DrawString(GameFont, "press 'r' when ready.", new Vector2(60, 300), Color.Black);
                    break;
                case 4:
                    spriteBatch.DrawString(GameFont, "brown is like wood, it's hard to chop.", new Vector2(60, 250), Color.Black);
                    spriteBatch.DrawString(GameFont, "press 'r' when ready.", new Vector2(60, 300), Color.Black);
                    break;
            }
            spriteBatch.End();
        }
        public void DrawEnded(GameTime currentTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(GameFont, "your final score is " + playerScore +".", new Vector2(60, 200), Color.Black);
            if(level==5)
                spriteBatch.DrawString(GameFont, "congratulations! wanna play again ?", new Vector2(60, 250), Color.Black);
            else
                spriteBatch.DrawString(GameFont, "ahhh almost there! wanna play again ?", new Vector2(60, 250), Color.Black);
            spriteBatch.DrawString(GameFont, "press 's' to start.", new Vector2(60, 300), Color.Black);
            spriteBatch.DrawString(GameFont, "press 'x' to close breakout.", new Vector2(60, 350), Color.Black);
            spriteBatch.End();
        }

        public void UpdateChanging(GameTime currentTime)
        {
            if(levelChanging)
                StartGame(level);
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                GameState = 2;
            }
        }

        public void UpdateEnded(GameTime currentTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                level = 1;
                lifes = 3;
                playerScore = 0;
                bricksH = 5;
                GameState = 2;
                StartGame(level);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                this.Exit();

            }
        } 

        public void UpdateStarted(GameTime currentTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                GameState = 2;
            }
        } 

        public void UpdatePlaying(GameTime gameTime) {
            paddle.Update();
            ball.Update(wallSoundInstance);
            int result = 0;
            foreach (Brick brick in bricks)
            {
                result = brick.CheckCollision(ball, paddle, brickTotal, Content, brickSoundInstance);
                if (result > 0)
                {
                    playerScore += result;
                    if (result != 25)
                    {
                        brickTotal--;
                    }
                }

            }

            if (brickTotal == 0)
            {
                level++;
                if (level < 5)
                {
                    GameState = 3;
                }
                else {
                    GameState = 4;
                }
                bricksH++;
                paddle.paddleSpeed = paddle.default_speed;
                ball.speed = ball.default_speed;
                levelChanging = true;
                return;
            }


            ball.PaddleCollision(paddle.GetBounds(),paddleSoundInstance);

            timeForSpeedUpgrade += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Increase ball spead
            if (timeForSpeedUpgrade > 10.5f)
            {
                ball.speed += 1;
                timeForSpeedUpgrade = 0f;
            }

            if (ball.OffBottom())
            {
                lifes--;
                if (lifes == 0)
                {
                    gameoverSoundInstance.Play();
                    paddle.paddleSpeed = paddle.default_speed;
                    ball.speed = ball.default_speed;
                    GameState = 4;
                }
                else {
                    lifeSoundInstance.Play();
                    paddle.paddleSpeed = paddle.default_speed;
                    ball.speed = ball.default_speed;
                    ball.SetInStartPosition(paddle.GetBounds());
                }
                    
            }
        }
        public void DrawPlaying(GameTime gameTime) {
            spriteBatch.Begin();
            foreach (Brick brick in bricks)
                brick.Draw(spriteBatch);
            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            spriteBatch.DrawString(GameFont, String.Format("score. {0}", playerScore), new Vector2(600, 540), Color.Black);
            spriteBatch.DrawString(GameFont, String.Format("lifes. {0}", lifes), new Vector2(60, 540), Color.Black);
            spriteBatch.End();

        }
    }
}
