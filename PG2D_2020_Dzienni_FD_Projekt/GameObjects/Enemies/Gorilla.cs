using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Gorilla : Enemy
    {
        SoundEffect hitting;
        SoundEffect hurting;
        SoundEffect dying;

        public Gorilla(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
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

            texture = TextureLoader.Load(@"characters/Gorilla", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/Gorilla.atlas", texture, content);

            hitting = content.Load<SoundEffect>(@"SoundEffects/gorillaHit");
            hurting = content.Load<SoundEffect>(@"SoundEffects/gorillaHurt");
            dying = content.Load<SoundEffect>(@"SoundEffects/gorillaDie");

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(90, 100);
            boundingBoxWidth = 50;
            boundingBoxHeight = 50;
        }

        public override void hurt()
        {
            hurting.Play();
            base.hurt();
        }

        public override void Attack(Character target, int dmg)
        {
            hitting.Play();
            base.Attack(target, dmg);
        }

        public override void Die()
        {
            dying.Play();
            base.Die();
        }
    }
}
