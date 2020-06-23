using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    public class GameState : State
    {
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game.IsMouseVisible = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // TODO: Add your drawing code here
            ResolutionManager.BeginDraw();

            var transformMatrix = Camera.GetTransformMatrix();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, transformMatrix);
            _game.tiledMap.Draw(spriteBatch);
            DrawGameObjects(_game.gameObjects, spriteBatch);
            DrawTriggers(_game.triggers, spriteBatch);
            spriteBatch.End();

            _game.gameHUD.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Escape))
            {
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, true));
            }

            if (Input.KeyPressed(Keys.I))
            {
                _game.ChangeState(new InventoryState(_game, _graphicsDevice, _content));
            }

            Input.Update();
            var playerObject = _game.gameObjects[0];

            // TODO: Add your update logic here
            _game.tiledMap.Update(gameTime, playerObject.position);
            UpdateGameObjects(_game.gameObjects, map: _game.tiledMap, gameTime);
            UpdateTriggers(_game.gameObjects, _game.triggers, map: _game.tiledMap, gameTime);
            UpdateCamera(playerObject.position);
        }

        private void UpdateCamera(Vector2 followPosition)
        {
            Camera.Update(followPosition);
        }

        public void UpdateGameObjects(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Update(gameObjects, map, gameTime);    //, gameTime    - aby nie zapomniec
            }

            //Parallel.ForEach(gameObjects, gameObject =>
            //{
            //    gameObject.Update(gameObjects);
            //});

        }

        public void UpdateTriggers(List<GameObject> gameObjects, List<Trigger> triggers, TiledMap map, GameTime gameTime)
        {
            foreach (var trigger in triggers)
            {
                trigger.Update(gameObjects, map, gameTime);
            }
        }

        public void DrawGameObjects(List<GameObject> gameObjects, SpriteBatch spriteBatch)
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

        public void DrawTriggers(List<Trigger> triggers, SpriteBatch spriteBatch)
        {
            float depth = 0.1f;
            foreach (var trigger in triggers)
            {
                trigger.layerDepth = depth;
                trigger.Draw(spriteBatch);
            }

        }
    }
}
