﻿using System.Collections.Generic;
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

        public Jhin(Vector2 startingPosition, CharacterSettings settings)
        {
            this.maxHp = settings.maxHp;
            this.hp = settings.maxHp;
            this.rangeOfAttack = settings.rangeOfAttack;

            SetMode(settings.mode);
            SetRange(settings.range);

            this.position = startingPosition;
            applyGravity = false;
        }

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            attackDelay--;
            cone.Update(gameObjects, map);
            Fire(gameObjects[0]);
            base.Update(gameObjects, map);
        }

        public override void Initialize()
        {
            attackDelay = 0;
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 0.5f;
            cone = new IceCone();
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

            boundingBoxOffset = new Vector2(40, 95);
            boundingBoxWidth = 26;
            boundingBoxHeight = 12;
        }

        public void Fire(GameObject player)
        {
            if (cone.active == false && Vector2.Distance(player.position, position) <= rangeOfAttack && attackDelay <= 0)
                {
                    isAttacking = true;
                    cone.Fire(this, new Vector2(this.BoundingBox.X, this.BoundingBox.Y), new Vector2(player.BoundingBox.X, player.BoundingBox.Y));
                }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            cone.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public void resetAttackDelay()
        {
            this.attackDelay = 80;
        }
    }
}
