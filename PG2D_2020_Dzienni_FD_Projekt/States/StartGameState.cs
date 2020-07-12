using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    class StartGameState : State
    {
        private List<Component> _components;
        private SpriteFont font;
        private Texture2D background;

        public StartGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/background");
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            font = _content.Load<SpriteFont>("Fonts/diamondfantasy");
            _game.IsMouseVisible = true;

            int xButtonPosition = ResolutionManager.VirtualWidth / 2 - buttonTexture.Width / 2;
            _components = new List<Component>();

            var continueButton = new Button(buttonTexture, font)
            {
                Position = new Vector2(xButtonPosition, 500),
                Text = "Continue",
            };
            continueButton.Click += ContinueButton_Click;
            _components.Add(continueButton);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);
            Statistics stats = ((Player)_game.gameObjects[0]).stats;
            spriteBatch.DrawString(font, "TEST", Vector2.Zero, Color.White);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }
    }
}
