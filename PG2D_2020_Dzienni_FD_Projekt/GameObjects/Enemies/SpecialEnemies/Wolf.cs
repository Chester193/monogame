using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.SpecialEnemies;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    public class Wolf : SpecialEnemy
    {

        SoundEffect roar;

        public Wolf(Vector2 startingPosition, CharacterSettings settings)
            : base(startingPosition, settings)
        {
        }

        public override void Initialize()
        {
            maxSpeed = 3.0f;
            acceleration = 0.5f;
            scale = 0.4f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/Wolf", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/Wolf.atlas", texture, content);

            roar = content.Load<SoundEffect>(@"SoundEffects/roar");

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(90, 124);
            boundingBoxWidth = 35;
            boundingBoxHeight = 35;
        }

        public override void AttackPlayer()
        {
            if(!isAttacking)
                roar.Play();
            base.AttackPlayer();
        }

        public override void Die()
        {
            if(!isDead)
                roar.Play();
            base.Die();
        }
    }
}
