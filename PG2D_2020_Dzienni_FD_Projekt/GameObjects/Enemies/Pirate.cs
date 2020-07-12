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
    class Pirate : Enemy
    {

        SoundEffect arrr;
        SoundEffect sword;

        public Pirate(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.5f;
            scale = 0.4f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/Pirate", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/Pirate.atlas", texture, content);

            sword = content.Load<SoundEffect>(@"SoundEffects/sword-unsheathe");
            arrr = content.Load<SoundEffect>(@"SoundEffects/pirateDie");

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(60, 70);
            boundingBoxWidth = 39;
            boundingBoxHeight = 60;
        }

        public override void Die()
        {
            arrr.Play();
            base.Die();
        }

        public override void Attack(Character target, int dmg)
        {
            sword.Play();
            base.Attack(target, dmg);
        }
    }
}
