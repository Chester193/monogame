using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Scripts;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int vResWidth = 1280, vResHeight = 720;
        int resWidth = 1280, resHeight = 720;

        bool gameStarted = false;
        bool gamePaused = false;

        public List<GameObject> gameObjects = new List<GameObject>();

        public TiledMap tiledMap;

        GameHUD gameHUD = new GameHUD();

        public List<ScriptsController> scriptsList = new List<ScriptsController>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ResolutionManager.Init(ref graphics);
            ResolutionManager.SetVirtualResolution(vResWidth, vResHeight);
            ResolutionManager.SetResolution(resWidth, resHeight, false);
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Scripts scripts = new Scripts(gameObjects, gameHUD);
            scriptsList.Add(new ScriptsController(scripts.TeleportTo1000_1000));
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationA));
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationB));
            scriptsList.Add(new ScriptsController(scripts.FastTravel));


            // TODO: Add your initialization logic here
            tiledMap = new TiledMap(vResWidth, vResHeight);

            int tileSpawnPointX = 59;
            int tielSpawnPointY = 52;
            Player player = new Player(new Vector2(tileSpawnPointX * 32, tielSpawnPointY * 32), scripts);

            Vector2 realMapBeginning = new Vector2(tiledMap.tileSize * 31, tiledMap.tileSize * 31);
            
            gameObjects.Add(player);
            gameHUD.Player(player);

            CharacterSettings characterSettings = new CharacterSettings
            {
                maxHp = 100,
                mode = CharcterMode.Guard,
                range = 300,
                rangeOfAttack = 30,
                weaponAttack = 20,
            };


            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(650, 970));
            points.Add(new Vector2(650, 1070));
            points.Add(new Vector2(850, 1070));

            characterSettings.points = points;

            gameObjects.Add(new Zombie(new Vector2(1000, 1000), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(720, 1000), characterSettings));

            /*
            characterSettings.mode = 0;
            gameObjects.Add(new Lizard(new Vector2(400, 600), characterSettings));
            characterSettings.rangeOfAttack = 30;
            gameObjects.Add(new Zombie(new Vector2(300, 400), characterSettings));
            gameObjects.Add(new Viking1(new Vector2(300, 300), characterSettings));
            gameObjects.Add(new Viking2(new Vector2(300, 200), characterSettings));
            gameObjects.Add(new Viking3(new Vector2(300, 100), characterSettings));
            characterSettings.mode = CharcterMode.FollowPlayer;
            gameObjects.Add(new Demon(new Vector2(290, 000), characterSettings));
            */

            gameObjects.Add(new Trigger(new Vector2(250, 0), new Vector2(200, 30), 1, scriptsList));
            gameObjects.Add(new Trigger(new Vector2(1100, 1570), new Vector2(200, 30), 2, scriptsList));
            gameObjects.Add(new Trigger(new Vector2(345, 665), new Vector2(75), 3, scriptsList));
            gameObjects.Add(new Trigger(new Vector2(890, 1300), new Vector2(75), 3, scriptsList));
            gameObjects.Add(new Trigger(new Vector2(1465, 25), new Vector2(75), 3, scriptsList));


            Camera.Initialize(zoomLevel: 1.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadInitializeGameObjects(gameObjects);

            // TODO: use this.Content to load your game content here
            tiledMap.Load(Content, @"Map/map.tmx");

            gameHUD.Load(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!gameStarted && Input.KeyPressed(Keys.Enter))
            {
                gameStarted = true;
                gameHUD.StartGame();
            }
            if (Input.KeyPressed(Keys.P))
            {
                gameHUD.TogglePause();
                gamePaused = !gamePaused;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            Input.Update();
            var playerObject = gameObjects[0];

            // TODO: Add your update logic here
            tiledMap.Update(gameTime, playerObject.position);
            UpdateGameObjects(gameObjects, map: tiledMap);
            UpdateCamera(playerObject.position);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            ResolutionManager.BeginDraw();

            var transformMatrix = Camera.GetTransformMatrix();

            if (gameStarted)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, transformMatrix);
                tiledMap.Draw(spriteBatch);
                DrawGameObjects(gameObjects);
                spriteBatch.End();
            }

            gameHUD.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void UpdateCamera(Vector2 followPosition)
        {
            Camera.Update(followPosition);
        }

        public void DrawGameObjects(List<GameObject> gameObjects)
        {
            List<GameObject> sortedGameObjects = new List<GameObject>(gameObjects);
            sortedGameObjects.Sort((a, b) => a.BoundingBox.Y.CompareTo(b.BoundingBox.Y));
            float depth = 0.1f;

            foreach (var gameObject in sortedGameObjects)
            {
                gameObject.layerDepth = depth;
                gameObject.Draw(spriteBatch);
                depth -= 0.001f;
            }

        }

        public void UpdateGameObjects(List<GameObject> gameObjects, TiledMap map)
        {
            if (gameStarted)
            {
                if (!gamePaused)
                {
                    foreach (var gameObject in gameObjects)
                    {
                        gameObject.Update(gameObjects, map);    //, gameTime    - aby nie zapomniec
                    }
                }
            }

            //Parallel.ForEach(gameObjects, gameObject =>
            //{
            //    gameObject.Update(gameObjects);
            //});

        }
        public void LoadInitializeGameObjects(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Initialize();
                gameObject.Load(content: Content);
            }


            //Parallel.ForEach(gameObjects, gameObject =>
            //{
            //    gameObject.Initialize();
            //    gameObject.Load(content: Content);
            //});
        }


    }
}
