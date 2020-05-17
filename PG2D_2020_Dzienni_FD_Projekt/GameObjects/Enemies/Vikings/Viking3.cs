﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Viking3 : Enemy
    {
        public Viking3(Vector2 startingPosition, CharacterSetings setings)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.maxHp = setings.maxHp;
            this.hp = setings.maxHp;
            this.rangeOfAttack = setings.rangeOfAttack;

            SetMode(setings.mode);
            SetRange(setings.range);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.05f;
            scale = 0.5f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/viking3", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/viking3.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(60, 100);
            boundingBoxWidth = 40;
            boundingBoxHeight = 20;
        }
    }
}