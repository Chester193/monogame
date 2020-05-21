using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Jhin
{
    class IceCone : Character
    {

        public List<Vector2> path;
        const float speed = 5.0f;

        Character owner;

        int destroyTimer;
        const int timeToLive = 180;

        public IceCone(Vector2 position)
        {
            active = false;
            path = new List<Vector2>();
            this.position = position;
        }

        public IceCone()
        {
            active = false;
        }

        protected override void UpdateAnimations()
        {
            if (IsAnimationComplete) this.active = false;
            base.UpdateAnimations();
        }

        public override void Load(ContentManager content)
        {
            texture = TextureLoader.Load(@"characters/jhin", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/jhin.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.IceCone);
            base.Load(content);
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            if (active == false)
                return;
            position += countDirection(direction) * speed;
            
            destroyTimer--;
            if (destroyTimer <= 0 && active == true)
            {
                Destroy();
            }
            base.Update(gameObjects, map);
        }

        internal void Fire(Character inputOwner, Vector2 inputPosition, Vector2 inputDirection)
        {
            Console.WriteLine(inputDirection);
            owner = inputOwner;
            position = inputPosition;
            direction = inputDirection;
            active = true;
            destroyTimer = timeToLive;
        }

        private void Destroy()
        {
            active = false;
        }

        private Vector2 countDirection(Vector2 initialDirection)
        {
            float x = initialDirection.X, y = initialDirection.Y;
            while (x > 1)
                x *= 0.10f;
            while (y > 1)
                y *= 0.10f;
            return new Vector2(x, y);
        }
    }
}
