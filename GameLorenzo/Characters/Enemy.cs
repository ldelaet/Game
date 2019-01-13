using GameLorenzo.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    //comments komen nog, klasse wordt nog gecleant
    class Enemy
    {
        Player player;
        Texture2D texture, healthTexture;
        Rectangle rectangle, healthRectangle;
        public Vector2 Velocity, origin, HealthPosition;
        //animations
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;
        int playerDistance, playerDistanceY;
        float rotation = 0f;
        bool isAbove = false, HasJumped = false;
        public bool Collides { get; set; }
        private Vector2 _position;
        public Vector2 postion
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null) _animationManager.Position = _position;
            }

        }

        public bool IsVisible { get; set; } = true;


        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }


        public int Health { get; set; } = 10;
        public Enemy(Texture2D newTexture, Vector2 newPosition, Dictionary<string, Animation> animations)
        {
            texture = newTexture;
            _position = newPosition;
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public Enemy(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture;
            _position = newPosition;
            
        }

        public void Update(Vector2 newPosition, GameTime gameTime)
        {

            _position += Velocity;
            origin = new Vector2(100 / 2, 100 / 2);
            playerDistance = (int)newPosition.X - (int)_position.X;
            playerDistanceY = (int)newPosition.Y - (int)_position.Y;
            if ((int)newPosition.Y > (int)_position.Y) isAbove = true;
            else isAbove = false;
            AI();
            SetAnimations(gameTime);
            

            
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
            
                if (playerDistanceY < 160 && playerDistanceY > -160 && !Collides)
                {
                    if (playerDistance < 600 && playerDistance > 0)
                    {
                        Velocity.X = 1.5f;
                    }
                    else if (playerDistance > -600 && playerDistance < 0)
                    {
                        Velocity.X = -1.5f;
                    }
                    else Velocity.X = 0;
                }
                else Velocity.X = 0; ;
            
        }
        private void SetAnimations(GameTime gameTime)
        {
            if (Velocity.X > 0)
                _animationManager.Play(_animations["EnemyWalkingRight"]);
            else if (Velocity.X < 0)
                _animationManager.Play(_animations["EnemyWalkingLeft"]);
            else if (Velocity.X == 0)
                _animationManager.Play(_animations["EnemyIdle"]); 
            _animationManager.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            rectangle = new Rectangle((int)_position.X, (int)_position.Y, 35, 70);
            if (_animationManager != null && IsVisible)
                _animationManager.Draw(spriteBatch, postion);
            
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
                _position.X = newRectangle.X + (newRectangle.Width + 1);
                Jump();
            }
            if (rectangle.isOnRight(newRectangle))
            {
                _position.X = newRectangle.X - (rectangle.Width + 1);
                Jump();
            }
            if (rectangle.isOnBottomOf(newRectangle))
            {
                Velocity.Y = 5f;
            }
            if (_position.X < 0) _position.X = 0;
            if (_position.X > x - rectangle.Width) _position.X = x - rectangle.Width;
            if (_position.Y < 0) Velocity.Y = 1f;
            if (_position.Y > y - rectangle.Height) _position.Y = y - rectangle.Height;
        }

        private void Jump() {
            if (!isAbove || !HasJumped)
            {
                _position.Y -= 5f;
                Velocity.Y = -9f;
                HasJumped = true;
            }
        }
    }
}
