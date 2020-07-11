using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class PortalFrame : GameObject
    {
        public PortalFrame(Vector2 startingPosition)
        {
            this.position = startingPosition;
            scale = 0.5f;
        }

        public override void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("Other/portal_frame");
            base.Load(content);
            boundingBoxHeight = 0;
        }
    }
}
