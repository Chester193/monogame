using Microsoft.Xna.Framework;
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
        private Texture2D background;
        private Player player;
        private Character npc;
        private SpriteFont font;
        private int playerItemCount;

        public TradeState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Character npc)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/trade");
            font = content.Load<SpriteFont>("Fonts\\diamondfantasy");

            _game.IsMouseVisible = true;
            player = (Player)_game.gameObjects[0];
            this.npc = npc;
            playerItemCount = player.Inventory.Count;
            UpdateComponents();
        }

        public void Move(InventoryItem item)
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
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
            Input.Update();

            foreach (var component in _components)
                component.Update(gameTime);

            if (player.Inventory.Count != playerItemCount)
            {
                UpdateComponents();
                playerItemCount = player.Inventory.Count;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);

            spriteBatch.DrawString(font, "Player", new Vector2(435, 90), Color.Gold);

            spriteBatch.DrawString(font, "Trader", new Vector2(730, 90), Color.Gold);

            spriteBatch.DrawString(font, "Press Q to quit", new Vector2(40, 675), new Color(249, 22, 29));

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

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
