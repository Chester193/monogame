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
    class TradeState : State
    {
        private List<Component> _components;
        private Texture2D background;
        private Player player;
        private SpriteFont font;

        public TradeState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/trade");
            font = content.Load<SpriteFont>("Fonts\\diamondfantasy");

            _game.IsMouseVisible = true;
            player = (Player)_game.gameObjects[0];

            UpdateComponents();
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.R))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
            Input.Update();

            foreach (var component in _components)
                component.Update(gameTime);

            if (player.Inventory.Count != _components.Count)
            {
                UpdateComponents();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);

            spriteBatch.DrawString(font, "Player", new Vector2(435, 90), Color.Gold);

            spriteBatch.DrawString(font, "Trader", new Vector2(730, 90), Color.Gold);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            _game.gameHUD.Draw(spriteBatch);
        }

        public void UpdateComponents()
        {
            Rectangle itemSpace = new Rectangle(50, 150, ResolutionManager.VirtualWidth - 750, ResolutionManager.VirtualHeight - 175);
            Vector2 currentPos = new Vector2(itemSpace.X, itemSpace.Y);

            _components = new List<Component>();

            foreach (InventoryItem item in player.Inventory)
            {
                item.Position = currentPos;
                _components.Add(item);

                currentPos.X += item.getSize().X * 1.5f;
                if (currentPos.X + item.getSize().X > itemSpace.X + itemSpace.Width)
                {
                    currentPos.X = itemSpace.X;

                    currentPos.Y += item.getSize().Y * 1.5f;
                    if (currentPos.Y + item.getSize().Y > itemSpace.Y + itemSpace.Height)
                    {
                        break;
                    }
                }
            }
        }
    }
}
