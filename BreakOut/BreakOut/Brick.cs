using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BreakOut
{
    class Brick{

        Texture2D texture;
        Rectangle location;
        Color tint;
        bool alive;
         
        public Rectangle Location{
            get { 
                return location;
            }
        }
        public Brick(Texture2D texture, Rectangle location, Color tint, bool alive) {
            this.texture = texture;
            this.location = location;
            this.tint = tint;
            this.alive = alive;
        }
        public void CheckCollision(Ball ball){

            if (this.alive && ball.Bounds.Intersects(this.location)) { 
                this.alive = false;
                ball.deflect(this);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
                spriteBatch.Draw(this.texture, this.location, this.tint);
        }
    }       
}
