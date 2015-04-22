using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
namespace BreakOut
{
    class Brick{

        Texture2D texture;
        Rectangle location;
        Color tint;
        bool alive;
        bool damaged;

        // Brick Type 
        // 1 - Normal
        // 2 - Extra Fast for 10 seconds [red]
        // 3 - Extra Slow for 10 seconds [black]
        // 4 - Drops bonus score [pink]
        int type;
        public Rectangle Location{
            get { 
                return location;
            }
        }
        public Brick(Texture2D texture, Rectangle location, Color tint, bool alive, int type) {
            this.texture = texture;
            this.location = location;
            this.tint = tint;
            this.alive = alive;
            this.type = type;
            this.damaged = false;
        }
        public int CheckCollision(Ball ball, Paddle paddle, int brickTotal, ContentManager content, SoundEffectInstance brickSoundEffect){
            if (this.alive && ball.Bounds.Intersects(this.location)) {
                int brickScore = 10;
                this.alive = false;
                ball.deflect(this);
                switch (this.type) { 
                    case 1:
                        break;
                    case 2:
                        paddle.paddleSpeed += 5;
                        brickScore += 20;
                        break;
                    case 3:
                        if (paddle.paddleSpeed - 5 >= 5)
                            paddle.paddleSpeed -= 5;
                        else
                            paddle.paddleSpeed = 5;
                        brickScore += 20;
                        break;
                    case 4:
                        ball.speed -= 2;
                        brickScore += 30;
                        break;
                    case 5:
                        if (!this.damaged)
                        {
                            this.alive = true;
                            this.texture = content.Load<Texture2D>("brick_damaged");
                            this.damaged = true;
                            brickScore += 25;
                        }
                        else {
                            this.alive = false;
                            brickScore += 15;
                        }
                        break;
                }
                brickTotal--;
                brickSoundEffect.Play();
                return brickScore;
            }
            return 0;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
                spriteBatch.Draw(this.texture, this.location, this.tint);
        }
    }       
}
