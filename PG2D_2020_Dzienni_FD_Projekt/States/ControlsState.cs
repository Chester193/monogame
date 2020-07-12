using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    class ControlsState : State
    {
        private List<Component> _components;
        private SpriteFont font;
        private Texture2D background;
        bool isContinuable;

        public ControlsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, bool isContinuable)
          : base(game, graphicsDevice, content)
        {
            background = _content.Load<Texture2D>("Other/background");
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            font = _content.Load<SpriteFont>("Fonts/diamondfantasy");
            _game.IsMouseVisible = true;
            this.isContinuable = isContinuable;

            int xButtonPosition = ResolutionManager.VirtualWidth / 2 - buttonTexture.Width / 2;
            _components = new List<Component>();

            var backButton = new Button(buttonTexture, font)
            {
                Position = new Vector2(xButtonPosition, 500),
                Text = "Back",
            };
            backButton.Click += BackButton_Click;
            _components.Add(backButton);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, ResolutionManager.VirtualWidth, ResolutionManager.VirtualHeight), Color.White);
            string text = "- Explore with buttons W, S, A, D searching for the chests with treasures, \n" +
                "   and eliminate the obstacles with SPACE button. \n" +
                "- Under TAB you can view your inventory or change weapons / armor. \n" +
                "- Always remember to have the healing and mana potions with you! \n" +
                "- It's worth checking the households, they tend to contain interesting \n" +
                "   stuff! \n" +
                "- Keep in mind that you can find new weapons and fill potions on the town \n" +
                "   market. \n" +
                "- Don't be afraid to die, if you do, the druid will get you up and running in \n" +
                "   his tent! \n" +
                "                                                        Good Luck!";
            spriteBatch.DrawString(font, text, new Vector2(50, 50), Color.White);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, isContinuable));
        }
    }
}
