using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace BreakOut{
    class Ball{
        Vector2 motion;
        Vector2 position;

        Rectangle bounds;
        public float default_speed = 4;
        public float speed = 4;
        bool collided;
        
        Texture2D texture;
        Rectangle screenBounds;

        public Rectangle Bounds{
            get{
                this.bounds.X = (int)this.position.X;
                this.bounds.Y = (int)this.position.Y;
                return this.bounds;
            }
        }
        public Ball(Texture2D texture, Rectangle screenBounds){
            this.bounds = new Rectangle(0, 0, texture.Width, texture.Height);
            this.texture = texture;
            this.screenBounds = screenBounds;
        }
        public void Update(SoundEffectInstance wallHit){
            this.collided = false;
            this.position += this.motion * this.speed;
            CheckWallCollision(wallHit);
        }

        public void deflect(Brick brick) {
            if (!this.collided) {
                this.collided = true;
                this.motion.Y *= -1;
            }
        }
        private void CheckWallCollision(SoundEffectInstance wallHit){
            if (this.position.X < 0){
                this.position.X = 0;
                this.motion.X *= -1;
                wallHit.Play();
            }
            if (this.position.X + this.texture.Width > this.screenBounds.Width){
                this.position.X = screenBounds.Width - texture.Width;
                this.motion.X *= -1;
                wallHit.Play();
            }
            if (this.position.Y < 0){
                this.position.Y = 0;
                this.motion.Y *= -1;
                wallHit.Play();
            }
        }
        public void SetInStartPosition(Rectangle paddleLocation){
            this.motion = new Vector2(1, -1);
            this.position.Y = paddleLocation.Y - this.texture.Height;
            this.position.X = paddleLocation.X + (paddleLocation.Width - this.texture.Width) / 2;
        }
        public bool OffBottom(){
            if (this.position.Y > this.screenBounds.Height)
                return true;
            return false;
        }
        public void PaddleCollision(Rectangle paddleLocation,SoundEffectInstance paddleHit){
            Rectangle ballLocation = new Rectangle(
            (int)this.position.X,
            (int)this.position.Y,
            this.texture.Width,
            this.texture.Height);
            if (paddleLocation.Intersects(ballLocation)){
                this.position.Y = paddleLocation.Y - this.texture.Height;
                if (this.motion.X < 0)
                {
                    if ((paddleLocation.X + paddleLocation.Width) - ballLocation.X < 30)
                    {
                        this.motion.X *= -1;
                    }
                }
                else {
                    if (ballLocation.X - paddleLocation.X < 30)
                    {
                        this.motion.X *= -1;
                    }
                }
                
                this.motion.Y *= -1;
                paddleHit.Play();
                
            }
        }
        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}