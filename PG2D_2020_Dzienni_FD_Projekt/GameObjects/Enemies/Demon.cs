using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Demon : Enemy
    {
        public Demon(Vector2 startingPosition)
        {
            this.position = startingPosition;
            applyGravity = false;
        }

        public override void Initialize()
        {
            maxSpeed = 3.0f;
            acceleration = 0.5f;
            scale = 1f;
            base.Initialize();
        }

        protected override void UpdateAnimations()
        {
            if (direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
            {
                ChangeAnimation(Animations.WalkingLeft);
            }
            if (direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
            {
                ChangeAnimation(Animations.WalkingRight);
            }
            base.UpdateAnimations();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/demon", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/demon.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(0f, 25f);
            boundingBoxWidth = 26;
            boundingBoxHeight = 12;
        }
    }
}