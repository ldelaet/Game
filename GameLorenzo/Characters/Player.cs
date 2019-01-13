using GameLorenzo.Characters;
using GameLorenzo.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    class Player
    {
        //classes
        public Bediening bediening;
        public Enemy enemy;
        private Texture2D texture, bulletTexture;
        private Vector2 position = new Vector2(100, 100);
        private Vector2 spawnPosition = new Vector2(100, 100);
        private Vector2 velocity;
        public Rectangle rectangle;
        private bool HasJumped = false;
        private float bulletDelay;
        public List<Bullet> Bullets;
        Vector2 origin;
        int lives = 5;
        public int level = 1;
        Values values = new Values();
        //animations
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;



        public bool Die { get; set; } = false;
        public bool NextLevel { get; set; } = false;
        public Vector2 Postion
        {
            get { return position; }
            set { position = value;
                if (_animationManager != null) _animationManager.Position = position;
            }
            
        }

        //Constructors
        public Player(Dictionary<string, Animation> animations) {
            
            bediening = new BedieningPijltjes();
            Bullets = new List<Bullet>();
            bulletDelay = 5;
            origin = new Vector2(100 / 2, 100 / 2);
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }
        public Player()
        {
            bediening = new BedieningPijltjes();
            Bullets = new List<Bullet>();
            bulletDelay = 5;
            origin = new Vector2(100 / 2, 100 / 2);
            
        }

        public void Load(ContentManager Content)
        {
             
            texture = Content.Load<Texture2D>("Player");
            bulletTexture = Content.Load<Texture2D>("bullet");
            
            

        }
        public void Update(GameTime gameTime)
        {
            position += velocity;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            if (velocity.Y < 10)
                velocity.Y += 0.4f;

            if (Die && level == 1)
            {

                position = new Vector2(100, 100);
                lives--;
                Die = false;

            }
            else if (Die && level == 2)
            {
                position = new Vector2(220, 1460);
                lives--;
                Die = false;

            }
            Input(gameTime);
            

            



            //animations
            SetAnimations(gameTime);

        }

        private void SetAnimations(GameTime gameTime)
        {
              if (velocity.X > 0 && !HasJumped && bediening.Shoot)
                _animationManager.Play(_animations["ShootWalkRight"]);
            else if (velocity.X < 0 && !HasJumped && bediening.Shoot)
                _animationManager.Play(_animations["ShootWalkLeft"]);
            else if (bediening.IdleLeft && bediening.Shoot)
                _animationManager.Play(_animations["ShootLeft"]);
            else if (bediening.IdleRight && bediening.Shoot)
                _animationManager.Play(_animations["ShootRight"]);
           
            else if (velocity.X > 0 && !HasJumped)
                _animationManager.Play(_animations["WalkRight"]);
            else if (velocity.X < 0 && !HasJumped)
                _animationManager.Play(_animations["WalkLeft"]);

            else if (bediening.IdleRight && velocity.X == 0 && !bediening.Jump)
                _animationManager.Play(_animations["IdleRight"]);
            else if (bediening.IdleLeft && velocity.X == 0 && !bediening.Jump)
                _animationManager.Play(_animations["IdleLeft"]);

            else if ((velocity.Y != 0 && bediening.IdleRight) || (velocity.X > 0 && velocity.Y != 0))

                _animationManager.Play(_animations["JumpRight"]);
            else if ((velocity.Y != 0 && bediening.IdleLeft) || (velocity.X < 0 && velocity.Y != 0))
                _animationManager.Play(_animations["JumpLeft"]);
            
            _animationManager.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (texture != null)
            //    spriteBatch.Draw(texture, rectangle, Color.White);
            if (_animationManager != null)
                _animationManager.Draw(spriteBatch, Postion);
            
                
            
            foreach (Bullet b in Bullets)
                b.Draw(spriteBatch);
        }

        //Extra methods
        private void Input(GameTime gameTime)
        {
            bediening.Update();
            if (bediening.Shoot)
            {
                if (bediening.Right || bediening.IdleRight)
                {
                    Shoot(20);
                }

                else if (bediening.Left || bediening.IdleLeft)
                {
                    Shoot(-20);
                }
                
            }
            UpdateBullets();
            //left gedrag
            if (bediening.Left)
            {
                velocity.X = -3;
            }
            //right gedrag
            else if (bediening.Right)
            {
                velocity.X = 3;
            }
            else velocity.X = 0;
            if (bediening.Sprint && velocity.X != 0)
                velocity.X *= 1.3f;
            
            //jump gedrag
            if (bediening.Jump && !HasJumped)
            {
                position.Y -= 5f;
                velocity.Y = -9f;
                HasJumped = true;
            }
        }

        public void Collision(Rectangle newRectangle, int x, int y)
        {
            if (rectangle.isOnTopOf(newRectangle))
            {
                rectangle.Y = newRectangle.Y - rectangle.Height;
                velocity.Y = 0f;
                HasJumped = false;
            }

            if (rectangle.isOnLeft(newRectangle))
            {

                position.X = newRectangle.X + (newRectangle.Width + 2);
            }
            if (rectangle.isOnRight(newRectangle))
            {
               position.X = newRectangle.X - (rectangle.Width + 2);
            }
            if (rectangle.isOnBottomOf(newRectangle))
            {
                velocity.Y = 5f;
            }
            if (position.X < 0) position.X = 0;
            if (position.X > x - rectangle.Width) position.X = x - rectangle.Width;
            if (position.Y < 0) velocity.Y = 1f;
            if (position.Y > y - rectangle.Height) position.Y = y - rectangle.Height;

            //if (rectangle.isOnBottomOf(newRectangle) | rectangle.isOnLeft(newRectangle) | rectangle.isOnRight(newRectangle) | rectangle.isOnTopOf(newRectangle))
            //{
            //    collision = true;
            //}
            //else collision = false;
        }

        //start positie bullet instellen
        public void Shoot(int speed) {
            //timer
            if (bulletDelay >= 0) bulletDelay--;
            //on timer 0
            if (bulletDelay <= 0) {
                Bullet newBullet = new Bullet(bulletTexture, speed);
                newBullet.Position = new Vector2(Postion.X + 32 - 20 /2, Postion.Y +10);
                newBullet.IsVisible = true;

                //max 30 bullets
                if (Bullets.Count() < 30)
                    Bullets.Add(newBullet);
            }
            //reset timer
            if (bulletDelay == 0) bulletDelay = 10;
        }

        public void UpdateBullets() {

            foreach (Bullet b in Bullets)
            {

                b.Velocity.X = b.Speed;
                b.Position.X += b.Velocity.X;
                if (b.Position.X > Postion.X + 500 || b.Position.X < Postion.X - 500) b.IsVisible = false;
            }
            //remove invisible bullets
            for (int i = 0; i < Bullets.Count(); i++)
            {
                if (!Bullets[i].IsVisible) {
                    Bullets.RemoveAt(i);
                    i--;
                }
            }

        }

    }
}
