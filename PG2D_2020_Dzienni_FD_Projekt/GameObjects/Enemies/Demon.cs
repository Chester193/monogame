using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Demon : Enemy
    {
        public Demon(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            maxSpeed = 3.0f;
            acceleration = 0.5f;
            scale = 0.9f;
            base.Initialize();
        }

        protected override void UpdateAnimations()
        {
            if (isAttacking) currentAnimation.animationSpeed = 12;
            base.UpdateAnimations();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/demon", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/demon.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(110, 150);
            boundingBoxWidth = 35;
            boundingBoxHeight = 35;
        }
    }
}