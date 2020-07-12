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
    class EndGameState : State
    {
        private List<Component> _components;
        private SpriteFont font;
        private Texture2D background;

        public EndGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/background");
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            font = _content.Load<SpriteFont>("Fonts/diamondfantasy");
            _game.IsMouseVisible = true;

            int xButtonPosition = ResolutionManager.VirtualWidth / 3;
            int halfButtonWidth = buttonTexture.Width / 2;
            _components = new List<Component>();

            var continueButton = new Button(buttonTexture, font)
            {
                Position = new Vector2(xButtonPosition - halfButtonWidth, 450),
                Text = "Continue",
            };
            continueButton.Click += ContinueButton_Click;
            _components.Add(continueButton);

            var quitGameButton = new Button(buttonTexture, font)
            {
                Position = new Vector2(2 * xButtonPosition - halfButtonWidth, 450),
                Text = "Quit Game",
            };
            quitGameButton.Click += QuitGameButton_Click;
            _components.Add(quitGameButton);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);
            Statistics stats = ((Player)_game.gameObjects[0]).stats;

            WriteCenter(spriteBatch, "Deaths :" + stats.Deaths.ToString(), 1);
            WriteCenter(spriteBatch, "Damage taken :" + stats.DamageTaken.ToString(), 2);
            WriteCenter(spriteBatch, "Damage dealt :" + stats.DamageDealt.ToString(), 3);
            WriteCenter(spriteBatch, "Mana used :" + stats.ManaUsed.ToString(), 4);
            WriteCenter(spriteBatch, "Gained Experience :" + stats.GainedExperience.ToString(), 5);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void WriteCenter(SpriteBatch spriteBatch, string text, int lineNumber)
        {
            int yOffset = 100;
            Vector2 position = font.MeasureString(text);
            position.X = ResolutionManager.VirtualWidth / 2 - position.X / 2;
            position.Y = position.Y * 1.2f * lineNumber + yOffset;

            spriteBatch.DrawString(font, text, position, Color.White);
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

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
