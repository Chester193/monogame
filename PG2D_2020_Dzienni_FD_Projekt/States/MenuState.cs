using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        private SpriteFont font;
        private Texture2D background;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isContinuable)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/background");
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            font = _content.Load<SpriteFont>("Fonts/diamondfantasy");
            _game.IsMouseVisible = true;

            int xButtonPosition = ResolutionManager.VirtualWidth / 2 - buttonTexture.Width / 2;
            int yButtonOffset = buttonTexture.Height * 2;
            int yButtonPosition = 0;

            _components = new List<Component>();

            if (isContinuable)
            {
                var continueButton = new Button(buttonTexture, font)
                {
                    Position = new Vector2(xButtonPosition, ResolutionManager.VirtualHeight / 3 + yButtonOffset * yButtonPosition),
                    Text = "Continue",
                };
                yButtonPosition++;
                continueButton.Click += ContinueButton_Click;
                _components.Add(continueButton);
            }

            var newGameButton = new Button(buttonTexture, font)
            {
                Position = new Vector2(xButtonPosition, ResolutionManager.VirtualHeight / 3 + yButtonOffset * yButtonPosition),
                Text = "New Game",
            };
            yButtonPosition++;
            newGameButton.Click += NewGameButton_Click;
            _components.Add(newGameButton);

            var quitGameButton = new Button(buttonTexture, font)
            {
                Position = new Vector2(xButtonPosition, ResolutionManager.VirtualHeight / 3 + yButtonOffset * yButtonPosition),
                Text = "Quit Game",
            };
            yButtonPosition++;
            quitGameButton.Click += QuitGameButton_Click;
            _components.Add(quitGameButton);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);

            Vector2 position = font.MeasureString("TERALG");
            position.X = ResolutionManager.VirtualWidth / 2 - position.X;

            spriteBatch.DrawString(font, "TERALG", position, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.1f);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void ContinueButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
