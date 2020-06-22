using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Orc
{
    class Axe : AnimatedObject
    {
        private const float Speed = 6.0f;

        Orc owner;

        private int destroyTimer;
        private const int TimeToLive = 180;

        public Axe()
        {
            scale = 0.12f;
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
            texture = TextureLoader.Load(@"characters/axe", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/axe.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.OrcAxe);
            base.Load(content);

            boundingBoxOffset = new Vector2(55, 55);
            boundingBoxWidth = 100;
            boundingBoxHeight = 100;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
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
            base.Update(gameObjects, map);
        }

        internal void Fire(Orc inputOwner, Vector2 inputPosition, Vector2 inputDirection)
        {
            owner = inputOwner;
            resetAnimation();
            //TODO fix this line with position(I have no idea ho to adjust this without hardcoding this vector2 parameter)
            position = inputPosition - boundingBoxOffset + new Vector2(50, 50);
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
            ChangeAnimation(Animations.OrcAxe);
        }
    }
}