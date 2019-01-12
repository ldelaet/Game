using GameLorenzo.Characters;
using GameLorenzo.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameLorenzo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        MapLevelGenerator mapGen;
        Player player;
        Background background;
        List<Enemy> enemies = new List<Enemy>();
        List<Spike> spikes = new List<Spike>();
        Camera camera;
        Texture2D enemyTexture;
        Prisoner prisoner;
        Key key;
        Bullet bullet;
        Values values = new Values();


        int prevLevel;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            map = new Map();
            
            mapGen = new MapLevelGenerator();
            background = new Background();
            key = new Key(new Vector2(1300, 600));
            prisoner = new Prisoner(new Vector2(1280, 160));
            enemyTexture = Content.Load<Texture2D>("Player");
            //spikes.Add(new Spike(new Vector2(320, 440)));
            spikes.Add(new Spike(new Vector2(1320, 920)));
            spikes.Add(new Spike(new Vector2(1280, 920)));
            spikes.Add(new Spike(new Vector2(1240, 920)));
            spikes.Add(new Spike(new Vector2(1200, 920)));
            spikes.Add(new Spike(new Vector2(960, 920)));
            spikes.Add(new Spike(new Vector2(660, 920)));
            spikes.Add(new Spike(new Vector2(380, 920)));


            //enemies.Add(new Enemy(enemyTexture, new Vector2(500, 300)));
            //enemies.Add(new Enemy(enemyTexture, new Vector2(700, 300)));
            //enemies.Add(new Enemy(enemyTexture, new Vector2(900, 300)));
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tiles.Content = Content;
            foreach (Spike spike in spikes) spike.Load(Content);
            prisoner.Load(Content);
            background.Load(Content);

            //animations
            var animations = new Dictionary<string, Animation>()
            {
                {"JumpRight", new Animation(Content.Load<Texture2D>("JumpRight"), 1 )},
                {"JumpLeft", new Animation(Content.Load<Texture2D>("JumpLeft"), 1 )},
                {"WalkLeft", new Animation(Content.Load<Texture2D>("WalkLeft"), 6 )},
                {"WalkRight", new Animation(Content.Load<Texture2D>("WalkRight"), 6 )},
                {"ShootWalkRight", new Animation(Content.Load<Texture2D>("ShootWalkRight"), 6 )},
                {"ShootWalkLeft", new Animation(Content.Load<Texture2D>("ShootWalkLeft"), 6 )},
                {"ShootRight", new Animation(Content.Load<Texture2D>("ShootRight"), 1 )},
                {"ShootLeft", new Animation(Content.Load<Texture2D>("ShootLeft"), 1 )},
                 {"IdleRight", new Animation(Content.Load<Texture2D>("IdleRight"), 8 )},
                {"IdleLeft", new Animation(Content.Load<Texture2D>("IdleLeft"), 8 )},
                {"EnemyWalkingRight", new Animation(Content.Load<Texture2D>("EnemyWalkingRight"), 6 )},
                 {"EnemyWalkingLeft", new Animation(Content.Load<Texture2D>("EnemyWalkingLeft"), 6 )},
                {"EnemyIdle", new Animation(Content.Load<Texture2D>("EnemyIdle"), 5 )},

            };
            player = new Player(animations);

            enemies.Add(new Enemy(enemyTexture, new Vector2(500, 300),animations));
            enemies.Add(new Enemy(enemyTexture, new Vector2(700, 300),animations));
            enemies.Add(new Enemy(enemyTexture, new Vector2(900, 300),animations));


            mapGen.LoadContent(values.level, map);
            player.Load(Content);
            key.Load(Content);



        }
        protected override void UnloadContent() {
            
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Class updates
            player.Update(gameTime);


            foreach (Enemy enemy in enemies)
                enemy.Update(player.Postion, gameTime);

            if (prevLevel != values.level)
            {
                mapGen.LoadContent(values.level, map);
            }
            prevLevel = values.level;

            //colissions and intersects
            ColllisionsAndIntersects();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.matrix);
            background.Draw(spriteBatch,map.Width, map.Height);
            map.Draw(spriteBatch);
            prisoner.Draw(spriteBatch);
            key.Draw(spriteBatch);
            player.Draw(spriteBatch);
            

            foreach (Enemy enemy in enemies) enemy.Draw(spriteBatch);
            foreach (Spike spike in spikes) spike.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }


        //Core methods:
        private void RemoveBullet(Bullet b)
        {
            player.Bullets.Remove(b);
            b.IsVisible = false;
        }
        private void ColllisionsAndIntersects()
        {
            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                player.Collision(tile.Rectangle, map.Width, map.Height);
                foreach (Enemy enemy in enemies)
                {
                    enemy.Collision(tile.Rectangle, map.Width, map.Height);
                    //if (enemy.Rectangle.Intersects(enemy.Rectangle)) enemy.Collides = true;
                    foreach (Enemy enemy2 in enemies)
                    {
                        if (enemy != enemy2)
                        {
                            if (enemy.Rectangle.Intersects(enemy2.Rectangle))
                            {
                                //enemy.position.X -= 4;
                                //enemy2.position.X += 4;
                                enemy.Collides = true;
                                
                            }
                            else enemy.Collides = false;
                        }
                    }
                    foreach (Bullet b in player.Bullets.ToArray())
                    {
                        if (b.rectangle.Intersects(tile.Rectangle))
                        {
                            RemoveBullet(b);
                            b.IsVisible = false;
                            player.Bullets.Remove(b);

                        }



                    }
                    camera.Update(player.Postion, map.Width, map.Height);
                }
                foreach (Enemy enemy in enemies)
                {
                    foreach (Bullet b in player.Bullets.ToArray())
                    {

                        if (b.rectangle.Intersects(enemy.Rectangle) && enemy.IsVisible && b.enemy != enemy)
                        {
                            RemoveBullet(b);
                            enemy.Damage();
                            b.enemy = enemy;
                        }
                    }
                    if (player.rectangle.Intersects(enemy.Rectangle) && enemy.IsVisible)
                    {
                        player.Die = true;

                    }
                }
                foreach (Spike spike in spikes)
                {
                    if (spike.rectangle.Intersects(player.rectangle)) player.Die = true;
                }
                if (key.rectangle.Intersects(player.rectangle))
                {
                    key.collected = true;
                    key.isVisible = false;

                }
                if (prisoner.rectangle.Intersects(player.rectangle) && key.collected)
                {
                    prisoner.texture = Content.Load<Texture2D>("unlocked");
                }
                if (player.Postion.X == map.Width - 50)
                {
                    values.level = 2;
                    player.Die = true;
                }

            }

        }
    }
}
