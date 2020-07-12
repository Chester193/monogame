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
        private List<Component> _components;
        private Texture2D background, moneyIcon, potionEffect;
        private Player player;
        private InventoryItem hovered;
        private SpriteFont font;

        Effect hPotion;

        public InventoryState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/inventory");
            font = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            moneyIcon = content.Load<Texture2D>("Other/money");

            potionEffect = _content.Load<Texture2D>("Other/mPotionEff");
            hPotion = content.Load<Effect>("VisualEffects/HPotion");

            _game.IsMouseVisible = true;
            player = (Player)_game.gameObjects[0];

            UpdateComponents();
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Tab))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
            Input.Update();

            hovered = null;
            foreach (var component in _components)
            {
                component.Update(gameTime);
                InventoryItem item = (InventoryItem)component;
                if (item.IsHovering)
                    hovered = item;
            }

            hPotion.Parameters["on"].SetValue(true);
            hPotion.Parameters["mPotion"].SetValue(false);

            UpdateComponents();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);

            player.DrawAnimation(spriteBatch, new Vector2(8, 160), 2f);

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            Texture2D weaponTexture = player.Weapon.Texture, armourTexture = player.Armour.Texture;
            spriteBatch.Draw(weaponTexture, new Rectangle(40, 390, weaponTexture.Width, weaponTexture.Height), Color.White);
            spriteBatch.Draw(armourTexture, new Rectangle(145, 390, armourTexture.Width, armourTexture.Height), Color.White);

            if (hovered != null)
            {
                spriteBatch.DrawString(font, "Name:\n" + hovered.Name, new Vector2(1000, 160), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                spriteBatch.DrawString(font, "Price:", new Vector2(1000, 230), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                spriteBatch.DrawString(font, hovered.Price.ToString(), new Vector2(1000, 260), Color.Gold, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                Vector2 offset = font.MeasureString(hovered.Price.ToString());
                spriteBatch.Draw(moneyIcon, new Rectangle((int)(985 + offset.X), 260, moneyIcon.Width, moneyIcon.Height), Color.Gold);
                spriteBatch.DrawString(font, "Description:\n" + hovered.Description, new Vector2(1000, 300), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
            }

            spriteBatch.End();

            spriteBatch.Begin(effect: hPotion);
            spriteBatch.Draw(potionEffect, new Rectangle(35, 177, 165, 164), Color.White);
            spriteBatch.End();

            _game.gameHUD.Draw(spriteBatch);
        }

        public void UpdateComponents()
        {
            Rectangle itemSpace = new Rectangle(250, 50, ResolutionManager.VirtualWidth - 500, ResolutionManager.VirtualHeight - 100);
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
