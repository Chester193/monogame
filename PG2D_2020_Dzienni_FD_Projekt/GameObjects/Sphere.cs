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
    class Sphere : ShaderObject
    {
        float timer;

        public Sphere(Vector2 startingPosition)
            : base(startingPosition)
        {
            timer = 0f;
            scale = 0.6f;
            isCollidable = false;
        }

        public override void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>("Other/sphere");
            Texture2D lightmap = content.Load<Texture2D>("visualEffects/lightmap");
            shader = content.Load<Effect>("visualEffects/sphere");
            shader.Parameters["Lightmap"].SetValue(lightmap);
            base.Load(content);
            boundingBoxHeight = 0;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            timer = (timer + 0.01f) % 1;
            shader.Parameters["Timer"].SetValue(timer);
            base.Update(gameObjects, map, gameTime);
        }
    }
}
