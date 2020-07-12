using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Zombie : Enemy
    {

        SoundEffect groan;

        public Zombie(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 0.28f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/zombie", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/zombie.atlas", texture, content);

            groan = content.Load<SoundEffect>(@"SoundEffects/zombie");

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(30, 110);
            boundingBoxWidth = 80;
            boundingBoxHeight = 80;
        }
        protected override void UpdateAnimations()
        {
            currentAnimation.animationSpeed = 8;
            base.UpdateAnimations();
        }

        public override void hurt()
        {
            groan.Play();
            base.hurt();
        }

        public override void Attack(Character target, int dmg)
        {
            groan.Play();
            base.Attack(target, dmg);
        }
    }
}
