using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Jhin
{
    class Jhin : Enemy
    {
        IceCone cone;
        private int attackDelay;

        public Jhin(Vector2 startingPosition)
        {
            this.position = startingPosition;
            applyGravity = false;
        }

        protected override void UpdateAnimations()
        {
            if (isAttacking)
            {
                velocity = Vector2.Zero;
                currentAnimation.animationSpeed = 15;

                if (direction.X < 0 && AnimationIsNot(Animations.SlashLeft))
                {
                    ChangeAnimation(Animations.SlashLeft);
                }
                if (direction.X > 0 && AnimationIsNot(Animations.SlashRight))
                {
                    ChangeAnimation(Animations.SlashRight);
                }
                if (IsAnimationComplete)
                {
                    isAttacking = false;
                }
            }
            if (!isAttacking && AnimationIsNot(Animations.IdleRight))
            {
                ChangeAnimation(Animations.IdleRight);
            }
            base.UpdateAnimations();
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            attackDelay--;
            cone.Update(gameObjects, map);
            shootAtPlayer(gameObjects);
            base.Update(gameObjects, map);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 0.5f;
            attackDelay = 40;
            cone = new IceCone(this.position);
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/jhin", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/jhin.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.IdleRight);

            cone.Load(content);
            base.Load(content);

            boundingBoxOffset = new Vector2(0f, 25f);
            boundingBoxWidth = 26;
            boundingBoxHeight = 12;
        }

        public void Fire(Vector2 playerPosition)
        {
                if (cone.active == false)
                {
                    attackDelay = 300;
                    cone.Fire(this, new Vector2(this.position.X, this.position.Y - 40), playerPosition);
                }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            cone.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        private void shootAtPlayer(List<GameObject> gameObjects)
        {
            GameObject player = gameObjects[0];
            Vector2 playerPosition = player.position;

            if (Vector2.Distance(playerPosition, position) < 300)
            {
                Fire(playerPosition);
                isAttacking = true;
            }
        }
    }
}
