using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class ShaderObject : GameObject
    {
        protected Effect shader;

        public ShaderObject(Vector2 startingPosition)
        {
            this.position = startingPosition;
            isCollidable = false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(blendState: BlendState.AlphaBlend, effect: shader);
            base.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void baseDraw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
