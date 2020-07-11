using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class BloodShader : ShaderObject
    {

        Player player;

        public BloodShader(Vector2 startingPosition, GameObject player) 
            : base(startingPosition)
        {
            this.player = (Player)player;
            scale = 1.0f;
        }

        public override void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("visualEffects/bloodMap1");
            shader = content.Load<Effect>("visualEffects/blood");
            base.Load(content);
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            shader.Parameters["playerHealth"].SetValue((float)player.characterSettings.hp);
            base.Update(gameObjects, map, gameTime);
        }

    }
}
