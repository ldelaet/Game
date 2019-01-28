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
        //Managers
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Classes
        Map map;
        MapLevelGenerator mapGen;
        Player player;
        Background background;
        List<Enemy> enemies = new List<Enemy>();
        List<Spike> spikes = new List<Spike>();
        Camera camera;
        Prisoner prisoner;
        Key key;
        Button button;
        WinningScreen winning;

        //GameStates
        enum GameState {
            Menu,
            Playing
        }
        GameState CurrentGameState = GameState.Menu;

        //Lokale, hardcoded variabele
        private int prevLevel;
        Texture2D enemyTexture;

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
            winning = new WinningScreen();
            mapGen = new MapLevelGenerator();
            background = new Background();
            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            button = new Button(Content.Load<Texture2D>("ButtonStart"), graphics.GraphicsDevice);
            button.Positioning(new Vector2(397,524));
            Tiles.Content = Content;
            winning.Load(Content);
            background.Load(Content);

            //animations:
            //Dictionary referentie
            Dictionary<string, Animation> animations = AnimationDictInit();
            player = new Player(animations);
            key = new Key(new Vector2(1300, 600));
            prisoner = new Prisoner(new Vector2(1280, 160));
            //Loop voor het spawnen van Spikes
            for (int i = 380; i < 1320; i+=40)
            {
                int j = 920;
                spikes.Add(new Spike(new Vector2(i, j)));

            }
            //enemies spawnen
            enemies.Add(new Enemy(enemyTexture, new Vector2(310, 400), animations));
            enemies.Add(new Enemy(enemyTexture, new Vector2(700, 400), animations));
            enemies.Add(new Enemy(enemyTexture, new Vector2(900, 400), animations));
            foreach (Spike spike in spikes) spike.Load(Content);
            prisoner.Load(Content);
            mapGen.LoadContent(player.level, map);
            player.Load(Content);
            key.Load(Content);
        }
        //De dictionary bevat een Alias en verder laad hier gewoon de content van de bijhorende media. Zie Animation class
        private Dictionary<string, Animation> AnimationDictInit()
        {
            return new Dictionary<string, Animation>()
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
        }

        protected override void UnloadContent() {}
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Update gamestate als knop wordt ingedrukt
            GameStateUpdate();
            //Class updates
            player.Update(gameTime);
            LevelLoader();
            foreach (Enemy enemy in enemies) enemy.Update(player.Postion, gameTime);
            //Hergenereer map als het level veranderd is
            RegenerateMap();

            //colissions and intersects
            ColllisionsAndIntersects();

            base.Update(gameTime);
        }

        private void RegenerateMap()
        {
            if (prevLevel != player.level)
            {
                mapGen.LoadContent(player.level, map);
            }
            prevLevel = player.level;
        }

        private void GameStateUpdate()
        {
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case GameState.Menu:
                    if (button.isClicked == true) CurrentGameState = GameState.Playing;
                    button.Update(mouse);
                    break;
                case GameState.Playing:
                    break;
                default:
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Beige);
            //zorgt ervoor dat enkel de zaken binnen de viewport van de camera getekend worden om zo geuheugen te besparen
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.matrix);
            background.Draw(spriteBatch, map.Width, map.Height);
            map.Draw(spriteBatch);
            prisoner.Draw(spriteBatch);
            key.Draw(spriteBatch);
            player.Draw(spriteBatch);
            winning.Draw(spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            foreach (Enemy enemy in enemies) enemy.Draw(spriteBatch);
            foreach (Spike spike in spikes) spike.Draw(spriteBatch);
            //teken de huidige game state
            GameStateDraw();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void GameStateDraw()
        {
            switch (CurrentGameState)
            {
                case GameState.Menu:
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    button.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    break;
                default:
                    break;
            }
        }

        //Cleart en genereerd al de nodige objecten en variabelen als het level veranderd
        public void LevelLoader()
        {
            if (player.level == 2 && (prevLevel != player.level))
            {
                prisoner.Unlocked = false; 
                key.collected = false;
                Dictionary<string, Animation> animations = AnimationDictInit();
                spikes.Clear();
                enemies.Clear();
                key = new Key(new Vector2(80, 200));
                prisoner = new Prisoner(new Vector2(1280, 80));
                for (int i = 400; i < 1120; i+= 40)
                {
                    int j = 1560;
                    spikes.Add(new Spike(new Vector2(i, j)));

                }
                enemies.Add(new Enemy(enemyTexture, new Vector2(120, 180), animations));
                enemies.Add(new Enemy(enemyTexture, new Vector2(200, 180), animations));
                enemies.Add(new Enemy(enemyTexture, new Vector2(280, 180), animations));
                foreach (Spike spike in spikes) spike.Load(Content);
                prisoner.Load(Content);
                key.Load(Content);
            }
            
        }

        
        private void RemoveBullet(Bullet b)
        {
            player.Bullets.Remove(b);
            b.IsVisible = false;
        }
        private void ColllisionsAndIntersects()
        {
            //collision van speler met tiles, kogels en tiles, enemies tegen enemies en enemies met de tiles
            foreach (CollisionTiles tile in map.CollisionTiles)
            {
                player.Collision(tile.Rectangle, map.Width, map.Height);
                foreach (Enemy enemy in enemies)
                {
                    enemy.Collision(tile.Rectangle, map.Width, map.Height);
                    //Collision, zorgt ervoor dat mannetjes niet meer kunnen bewegen:
                    //if (enemy.Rectangle.Intersects(enemy.Rectangle)) enemy.Collides = true;
                    foreach (Enemy enemy2 in enemies)
                    {
                        if (enemy != enemy2)
                        {
                            if (enemy.Rectangle.Intersects(enemy2.Rectangle))
                            {
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
                //Collision detection met kogels tegen enemies, verwijderd de kogel en de enemy uit de lijst.
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
                    //Speler sterft bij een intersect met een enemy
                    if (player.rectangle.Intersects(enemy.Rectangle) && enemy.IsVisible)
                    {
                        player.Die = true;
                    }
                }
                //Speler sterft bij een intersect met een Spike
                foreach (Spike spike in spikes)
                {
                    if (spike.rectangle.Intersects(player.rectangle)) player.Die = true;

                }
                //collect key
                if (key.rectangle.Intersects(player.rectangle))
                {
                    key.collected = true;
                    key.isVisible = false;
                }
                //free prisoner
                if (prisoner.rectangle.Intersects(player.rectangle) && key.collected)
                {
                    prisoner.texture = Content.Load<Texture2D>("unlocked");
                    if (player.level == 2) winning.HasWon = true;
                    prisoner.Unlocked = true;

                }
                //initialiseer level 2 en respawn -> enkel als gevangene vrij is
                if (player.Postion.X == map.Width - 50 && prisoner.Unlocked)
                {
                    player.Die = true;
                    player.level = 2;
                }

            }

        }
    }
}
