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
    class GingerBandit : Enemy
    {

        SoundEffect sword;
        SoundEffect die;

        public GingerBandit(Vector2 startingPosition, CharacterSettings settings)
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

            texture = TextureLoader.Load(@"characters/ginger_bandit", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/ginger_bandit.atlas", texture, content);

            sword = content.Load<SoundEffect>(@"SoundEffects/sword-unsheathe");
            die = content.Load<SoundEffect>(@"SoundEffects/pirateTreasure");

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(70, 100);
            boundingBoxWidth = 35;
            boundingBoxHeight = 35;
        }

        public override void Attack(Character target, int dmg)
        {
            sword.Play();
            base.Attack(target, dmg);
        }

        public override void Die()
        {
            die.Play();
            base.Die();
        }
    }
}
