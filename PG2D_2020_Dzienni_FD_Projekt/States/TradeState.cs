using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.npc;
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
        private Texture2D background, moneyIcon;
        protected Player player;
        protected Character npc;
        private SpriteFont font;
        protected string characterName;
        private InventoryItem hovered;

        protected SoundEffect sound;

        public TradeState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Character npc)
          : base(game, graphicsDevice, content)
        {
            characterName = "Trader";
            background = _content.Load<Texture2D>("Other/trade");
            font = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            moneyIcon = content.Load<Texture2D>("Other/money");

            sound = content.Load<SoundEffect>(@"SoundEffects/coin");

            _game.IsMouseVisible = true;
            player = (Player)_game.gameObjects[0];
            this.npc = npc;
            UpdateComponents();
        }

        public virtual void Move(InventoryItem item)
        {
            if(player.Inventory.IndexOf(item) >= 0)
            {
                player.EarnMoney(item.Price);
                player.Inventory.Remove(item);
                npc.Inventory.Add(item);
            }
            else if(npc.Inventory.IndexOf(item) >= 0)
            {
                try
                {
                    player.SpendMoney(item.Price);
                    npc.Inventory.Remove(item);
                    player.Inventory.Add(item);
                }
                catch(NotEnoughMoneyException)
                {
                    _game.gameHUD.PrintMessage("Not enough money!", 100);
                    
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.KeyPressed(Keys.Q))
            {
                sound.Play();
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

            UpdateComponents();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);

            spriteBatch.DrawString(font, "Player", new Vector2(435, 90), Color.Gold);

            spriteBatch.DrawString(font, characterName, new Vector2(730, 90), Color.Gold);

            spriteBatch.DrawString(font, "Press Q to quit", new Vector2(40, 675), new Color(249, 22, 29));

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            if (hovered != null)
            {
                spriteBatch.DrawString(font, "Name: " + hovered.Name, new Vector2(800, 10), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                spriteBatch.DrawString(font, "Price:", new Vector2(1100, 10), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                spriteBatch.DrawString(font, hovered.Price.ToString(), new Vector2(1170, 10), Color.Gold, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                Vector2 offset = font.MeasureString(hovered.Price.ToString());
                spriteBatch.Draw(moneyIcon, new Rectangle((int)(1160 + offset.X), 10, moneyIcon.Width, moneyIcon.Height), Color.Gold);
                spriteBatch.DrawString(font, "Description:", new Vector2(800, 50), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
                spriteBatch.DrawString(font, hovered.Description, new Vector2(945, 50), Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
            }

            spriteBatch.End();

            _game.gameHUD.Draw(spriteBatch);
        }

        public void UpdateComponents()
        {
            _components = new List<Component>();
            Rectangle playerItemSpace = new Rectangle(50, 150, ResolutionManager.VirtualWidth - 750, ResolutionManager.VirtualHeight - 175);
            Rectangle npcItemSpace = new Rectangle(725, 150, ResolutionManager.VirtualWidth - 750, ResolutionManager.VirtualHeight - 175);
            UpdateComponentsFor(player, playerItemSpace);
            UpdateComponentsFor(npc, npcItemSpace);
        }

        public void UpdateComponentsFor(Character character, Rectangle itemSpace)
        {
            Vector2 currentPos = new Vector2(itemSpace.X, itemSpace.Y);

            foreach (InventoryItem item in character.Inventory)
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
