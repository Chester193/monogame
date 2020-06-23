using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    class InventoryState : State
    {
        private List<Component> _components = new List<Component>();
        private Texture2D background;
        private List<InventoryItem> items;

        public InventoryState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/background");
            _game.IsMouseVisible = true;
            items = ((Player)_game.gameObjects[0]).Inventory;

            Rectangle itemSpace = new Rectangle(250, 50, ResolutionManager.VirtualWidth - 500, ResolutionManager.VirtualHeight - 100);

            Vector2 currentPos = new Vector2(itemSpace.X, itemSpace.Y);

            foreach(InventoryItem item in items)
            {
                item.Position = currentPos;
                _components.Add(item);

                currentPos.X += item.getSize().X * 1.5f;
                if (currentPos.X + item.getSize().X > itemSpace.X + itemSpace.Width)
                {
                    currentPos.X = itemSpace.X;

                    currentPos.Y += item.getSize().Y * 1.5f;
                    if(currentPos.Y + item.getSize().Y > itemSpace.Y + itemSpace.Height)
                    {
                        break;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Escape))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
            Input.Update();

            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
