using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Lizard : Enemy
    {
        public Lizard(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 1f;
            base.Initialize();
        }

        protected override void UpdateAnimations()
        {
            if (isAttacking) currentAnimation.animationSpeed = 12;
            base.UpdateAnimations();
        }


        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/lizard", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/lizard.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(110, 127);
            boundingBoxWidth = 30;
            boundingBoxHeight = 30;
        }
    }
}