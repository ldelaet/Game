using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameLorenzo
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        MapLevelGenerator mapGen;
        Player player;
        Enemy enemy;
        Camera camera;
        Texture2D enemyTexture;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            map = new Map();
            mapGen = new MapLevelGenerator();
            player = new Player();
            enemyTexture = Content.Load<Texture2D>("Player");
            enemy = new Enemy(enemyTexture, new Vector2(500, 300), 100, 200);
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tiles.Content = Content;

            mapGen.LoadContent(1, map);
            player.Load(Content);
            
        }
        protected override void UnloadContent() { }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Class updates
            player.Update(gameTime);
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
            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
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
                enemy.Collision(tile.Rectangle, map.Width, map.Height);
                foreach (Bullet b in player.Bullets.ToArray())
                {
                    if (b.rectangle.Intersects(tile.Rectangle))
                    {
                        RemoveBullet(b);
                        b.IsVisible = false;
                        player.Bullets.Remove(b);

                    }
                    else if (b.rectangle.Intersects(enemy.Rectangle) && enemy.IsVisible && b.enemy != enemy)
                    {
                        RemoveBullet(b);
                        enemy.Damage();
                        b.enemy = enemy;
                    }
                }
                camera.Update(player.Postion, map.Width, map.Height);
            }

            if (player.rectangle.Intersects(enemy.Rectangle) && enemy.IsVisible)
            {
                player.Postion = new Vector2(100, 100);
                enemy = new Enemy(enemyTexture, new Vector2(500, 300), 100, 200);
            }
        }

    }
}
