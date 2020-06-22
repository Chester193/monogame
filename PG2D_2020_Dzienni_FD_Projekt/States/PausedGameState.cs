using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    public class PausedGameState : GameState
    {
        public PausedGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            //no Update
            if (Input.KeyPressed(Keys.I))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }

            Input.Update();
        }
    }
}
