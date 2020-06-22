using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Jhin
{
    class IceCone : AnimatedObject
    {
        private const float Speed = 6.0f;

        Jhin owner;

        private int destroyTimer;
        private const int TimeToLive = 180;

        public IceCone()
        {
            active = false;
        }

        protected override void UpdateAnimations()
        {
            if (IsAnimationComplete)
            {
                Destroy();
            }
            base.UpdateAnimations();
        }

        public override void Load(ContentManager content)
        {
            texture = TextureLoader.Load(@"characters/jhin", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/jhin.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.IceCone);
            base.Load(content);

            boundingBoxOffset = new Vector2(110, 110);
            boundingBoxWidth = 50;
            boundingBoxHeight = 50;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            if (active == false)
                return;
   
            position += direction * Speed;

            CheckCollisions(gameObjects, map);

            destroyTimer--;
            if (destroyTimer <= 0 && active == true)
            {               
                Destroy();
            }
            base.Update(gameObjects, map, gameTime);
        }

        internal void Fire(Jhin inputOwner, Vector2 inputPosition, Vector2 inputDirection)
        {
            owner = inputOwner;
            resetAnimation();
            //TODO fix this line with position(I have no idea ho to adjust this without hardcoding this vector2 parameter)
            position = inputPosition - boundingBoxOffset + new Vector2(20, 20);
            direction = Vector2.Normalize(Vector2.Subtract(inputDirection, inputPosition));
            active = true;
            destroyTimer = TimeToLive;
        }

        private void Destroy()
        {
            owner.resetAttackDelay();
            active = false;
        }

        private void CheckCollisions(List<GameObject> gameObjects, TiledMap map)
        {
            if (gameObjects[0].CheckCollision(BoundingBox))
            {
                Destroy();
                gameObjects[0].BulletResponse(25);
                return;
            } 
         
        }

        private void resetAnimation()
        {
            ChangeAnimation(Animations.IceCone);
        }
    }
}
