using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut{
    class Ball{
        Vector2 motion;
        Vector2 position;

        Rectangle bounds;

        float speed = 4;
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
        public void Update(){
            this.collided = false;
            this.position += this.motion * this.speed;
            CheckWallCollision();
        }

        public void deflect(Brick brick) {
            if (!this.collided) {
                this.collided = true;
                this.motion.Y *= -1;
            }
        }
        private void CheckWallCollision(){
            if (this.position.X < 0){
                this.position.X = 0;
                this.motion.X *= -1;
            }
            if (this.position.X + this.texture.Width > this.screenBounds.Width){
                this.position.X = screenBounds.Width - texture.Width;
                this.motion.X *= -1;
            }
            if (this.position.Y < 0){
                this.position.Y = 0;
                this.motion.Y *= -1;
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
        public void PaddleCollision(Rectangle paddleLocation){
            Rectangle ballLocation = new Rectangle(
            (int)this.position.X,
            (int)this.position.Y,
            this.texture.Width,
            this.texture.Height);
            if (paddleLocation.Intersects(ballLocation)){
                this.position.Y = paddleLocation.Y - this.texture.Height;
                this.motion.Y *= -1;
            }
        }
        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}