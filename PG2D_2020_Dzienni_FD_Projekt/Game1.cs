using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int vResWidth = 1280, vResHeight = 720;
        public static int resWidth = 1280, resHeight = 720;

        public List<GameObject> gameObjects = new List<GameObject>();

        public TiledMap tiledMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ResolutionManager.Init(ref graphics);
            ResolutionManager.SetVirtualResolution(vResWidth, vResHeight);
            ResolutionManager.SetResolution(resWidth, resHeight, false);
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            tiledMap = new TiledMap(vResWidth, vResHeight);
            GameObject player = new Player();
            player.position = new Vector2(15*32 + 1200, 15*32 + 2000);
            gameObjects.Add(player);

            GameObject enemy = new Enemy(new Vector2(200, 300));
            //gameObjects.Add(enemy);

            Camera.Initialize(zoomLevel: 1.0f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadInitializeGameObjects(gameObjects);

            tiledMap.Load(Content, @"Map/map.tmx");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            ResolutionManager.BeginDraw();

            var transformMatrix = Camera.GetTransformMatrix();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, transformMatrix);

            tiledMap.Draw(spriteBatch);
            DrawGameObjects(gameObjects);
            spriteBatch.End();

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
