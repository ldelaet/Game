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
        List<Enemy> enemies = new List<Enemy>();
        List<Spike> spikes = new List<Spike>();
        Camera camera;
        Texture2D enemyTexture;
        Prisoner prisoner;
        Key key;
        Bullet bullet;
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
            player = new Player();
            key = new Key(new Vector2(1300, 600));
            prisoner = new Prisoner(new Vector2(1280,160));
            enemyTexture = Content.Load<Texture2D>("Player");
            //spikes.Add(new Spike(new Vector2(320, 440)));
            spikes.Add(new Spike(new Vector2(1320, 920)));
            spikes.Add(new Spike(new Vector2(1280, 920)));
            spikes.Add(new Spike(new Vector2(1240, 920)));
            spikes.Add(new Spike(new Vector2(1200, 920)));
            spikes.Add(new Spike(new Vector2(960, 920)));
            spikes.Add(new Spike(new Vector2(660, 920)));
            spikes.Add(new Spike(new Vector2(380, 920)));


            enemies.Add(new Enemy(enemyTexture, new Vector2(500, 300)));
            enemies.Add(new Enemy(enemyTexture, new Vector2(700, 300)));
            enemies.Add(new Enemy(enemyTexture, new Vector2(900, 300)));
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tiles.Content = Content;
            foreach (Spike spike in spikes) spike.Load(Content);
            prisoner.Load(Content);
            mapGen.LoadContent(1, map);
            player.Load(Content);
            key.Load(Content);
            
            
            
        }
        protected override void UnloadContent() { }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Class updates
            player.Update(gameTime);
            foreach (Enemy enemy in enemies)
            enemy.Update(player.Postion);

            

            //colissions and intersects
            ColllisionsAndIntersects();

            base.Update(gameTime);
        }    

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.matrix);
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
        private void RemoveBullet(Bullet b) {
            player.Bullets.Remove(b);
            b.IsVisible = false;
        }
        private void ColllisionsAndIntersects()
        {
            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                player.Collision(tile.Rectangle, map.Width, map.Height);
                foreach (Enemy enemy in enemies) {
                    enemy.Collision(tile.Rectangle, map.Width, map.Height);
                    //if (enemy.Rectangle.Intersects(enemy.Rectangle)) enemy.Collides = true;
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
            if (key.rectangle.Intersects(player.rectangle)) {
                key.collected = true;
                key.isVisible = false;

            }
            if (prisoner.rectangle.Intersects(player.rectangle) && key.collected)
            {
                prisoner.texture = Content.Load<Texture2D>("unlocked");
            }

        }

    }
}
