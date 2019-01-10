using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    class Bullet
    {
        

        public Rectangle rectangle;
        public Texture2D bullet;
        public Vector2 Velocity, Position, Origin;
        public int Speed;
        public Enemy enemy;



        public bool IsVisible;

        public Bullet(Texture2D myTexture, int speed)
        {
            Speed = speed;
            bullet = myTexture;
            IsVisible = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rectangle = new Rectangle((int)Position.X, (int)Position.Y, 20, 10);
            spriteBatch.Draw(bullet, rectangle, Color.White);
        }
    }
    
}
