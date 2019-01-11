using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    class Enemy
    {
        Player player;
        Texture2D texture, healthTexture;
        Rectangle rectangle, healthRectangle;
        Vector2 position, Velocity, origin, HealthPosition;
        int playerDistance, playerDistanceY;
        float rotation = 0f;
        bool isAbove = false, HasJumped = false;
        

        public bool IsVisible { get; set; } = true;


        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }


        public int Health { get; set; } = 5;

        public Enemy(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;
            
        }

        public void Update(Vector2 newPosition)
        {

            position += Velocity;
            origin = new Vector2(100 / 2, 100 / 2);
            playerDistance = (int)newPosition.X - (int)position.X;
            playerDistanceY = (int)newPosition.Y - (int)position.Y;
            if ((int)newPosition.Y > (int)position.Y) isAbove = true;
            else isAbove = false;
            AI();
            

            
        }
        public void Damage() {
            Health--;
            if (Health <= 0)
            {

                IsVisible = false;
            }
        }
        
        public void AI()
        {
            if (Velocity.Y < 10)
                Velocity.Y += 0.4f;
            
                if (playerDistanceY < 160 && playerDistanceY > -160)
                {
                    if (playerDistance < 600 && playerDistance > 0)
                    {
                        Velocity.X = 2f;
                    }
                    else if (playerDistance > -600 && playerDistance < 0)
                    {
                        Velocity.X = -2f;
                    }
                    else Velocity.X = 0;
                }
                else Velocity.X = 0; ;
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 70, 70);

            if (IsVisible)
            {
                if (Velocity.X < 0)
                    spriteBatch.Draw(texture, rectangle, null, Color.White, rotation, origin, SpriteEffects.FlipHorizontally, 0);
                else spriteBatch.Draw(texture, rectangle, null, Color.White, rotation, origin, SpriteEffects.None, 0);
            }
        }

        public void Collision(Rectangle newRectangle, int x, int y)
        {
            if (rectangle.isOnTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                Velocity.Y = 0f;
                HasJumped = false;
            }

            if (rectangle.isOnLeft(newRectangle))
            {
                position.X = newRectangle.X + (newRectangle.Width + 1);
                Jump();
            }
            if (rectangle.isOnRight(newRectangle))
            {
                position.X = newRectangle.X - (rectangle.Width + 1);
                Jump();
            }
            if (rectangle.isOnBottomOf(newRectangle))
            {
                Velocity.Y = 5f;
            }
            if (position.X < 0) position.X = 0;
            if (position.X > x - rectangle.Width) position.X = x - rectangle.Width;
            if (position.Y < 0) Velocity.Y = 1f;
            if (position.Y > y - rectangle.Height) position.Y = y - rectangle.Height;
        }

        private void Jump() {
            if (!isAbove || !HasJumped)
            {
                position.Y -= 5f;
                Velocity.Y = -9f;
                HasJumped = true;
            }
        }
    }
}
