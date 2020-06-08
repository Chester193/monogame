﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies;

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

        public List<GameObject> gameObjects = new List<GameObject>();

        public TiledMap renderingMap;
        public TiledMap mainWorld;
        public TiledMap blacksmith;

        GameHUD gameHUD = new GameHUD();

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
            // TODO: Add your initialization logic here
            mainWorld = new TiledMap(vResWidth, vResHeight);
            blacksmith = new TiledMap(vResWidth, vResHeight);

            renderingMap = mainWorld;

            Vector2 realMapBeginning = new Vector2(renderingMap.tileSize * 31, renderingMap.tileSize * 31);
            Player player = new Player();
            int tileSpawnPointX = 10;
            int tielSpawnPointY = 14;
            player.position = new Vector2(tileSpawnPointX * 32, tielSpawnPointY * 32);
            gameObjects.Add(player);

            gameHUD.Player(player);

            CharacterSettings characterSettings = new CharacterSettings();
            characterSettings.maxHp = 100;
            characterSettings.mode = CharcterMode.Guard;
            characterSettings.range = 300;
            characterSettings.rangeOfAttack = 80;

            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(650, 970));
            points.Add(new Vector2(650, 1070));
            points.Add(new Vector2(850, 1070));

            characterSettings.points = points;

            gameObjects.Add(new Zombie(new Vector2(-100, -100), characterSettings));     //z jakiegoś powodu pierwszy przeciwnik jest zawsze niesmiertelny;
            gameObjects.Add(new Lizard(new Vector2(realMapBeginning.X + 650, realMapBeginning.Y + 550), characterSettings));

            //characterSettings.mode = 0;
            //gameObjects.Add(new Lizard(new Vector2(realMapBeginning.X + 400, realMapBeginning.Y + 600), characterSettings));
            //characterSettings.rangeOfAttack = 30;
            //gameObjects.Add(new Zombie(new Vector2(realMapBeginning.X + 400, realMapBeginning.Y + 650), characterSettings));
            //gameObjects.Add(new Viking1(new Vector2(realMapBeginning.X + 400, realMapBeginning.Y + 700), characterSettings));
            //gameObjects.Add(new Viking2(new Vector2(realMapBeginning.X + 500, realMapBeginning.Y + 700), characterSettings));
            //gameObjects.Add(new Viking3(new Vector2(realMapBeginning.X + 550, realMapBeginning.Y + 650), characterSettings));
            //characterSettings.mode = CharcterMode.FollowPlayer;
            //gameObjects.Add(new Demon(new Vector2(realMapBeginning.X + 500, realMapBeginning.Y + 700), characterSettings));

            gameHUD.Enemy((Enemy)gameObjects[2]);
            
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
            renderingMap.Load(Content, @"Map/map.tmx");
            blacksmith.Load(Content, @"Map/blacksmith.tmx");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.T)) {
                renderingMap = blacksmith;
            }

            Input.Update();
            var playerObject = gameObjects[0];

            // TODO: Add your update logic here
            renderingMap.Update(gameTime, playerObject.position);
            UpdateGameObjects(gameObjects, map: renderingMap);
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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, transformMatrix);
            renderingMap.Draw(spriteBatch);
            DrawGameObjects(gameObjects);
            spriteBatch.End();

            gameHUD.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void UpdateCamera(Vector2 followPosition)
        {
            Camera.Update(followPosition);
        }

        public void DrawGameObjects(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Draw(spriteBatch);
            }

        }

        public void UpdateGameObjects(List<GameObject> gameObjects, TiledMap map)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameObjects, map);
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
