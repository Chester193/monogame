using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Demon : Enemy
    {

        SoundEffect dying;
        SoundEffect hurting;
        SoundEffect hitting;

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
            if (isAttacking) currentAnimation.animationSpeed = 13;
            base.UpdateAnimations();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/demon", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/demon.atlas", texture, content);

            dying = content.Load<SoundEffect>(@"SoundEffects/demonDie");
            hurting = content.Load<SoundEffect>(@"SoundEffects/demonHurt");
            hitting = content.Load<SoundEffect>(@"SoundEffects/demonHit");

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(110, 150);
            boundingBoxWidth = 35;
            boundingBoxHeight = 35;
        }

        public override void Attack(Character target, int dmg)
        {
            hitting.Play();
            base.Attack(target, dmg);
        }

        public override void hurt()
        {
            hurting.Play();
            base.hurt();
        }

        public override void Die()
        {
            dying.Play();
            base.Die();
        }
    }
}