using Microsoft.Xna.Framework;
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
    class LavaGolem : Enemy
    {
        public LavaGolem(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.5f;
            scale = 0.5f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/lava_golem", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/lava_golem.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(60, 70);
            boundingBoxWidth = 39;
            boundingBoxHeight = 40;
        }

        public override void Attack(Character target, int dmg)
        {
            if (!isAttacking)
                golemsEffects[new Random().Next(0, 5)].Play();
            base.Attack(target, dmg);
        }

        public override void hurt()
        {
            golemsEffects[new Random().Next(0, 5)].Play();
            base.hurt();
        }
    }
}
