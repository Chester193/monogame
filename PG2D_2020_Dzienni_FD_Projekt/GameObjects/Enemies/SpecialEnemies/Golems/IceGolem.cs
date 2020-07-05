using Microsoft.Xna.Framework;
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
    class IceGolem : SpecialEnemy
    {
        public IceGolem(Vector2 startingPosition, CharacterSettings settings)
            : base(startingPosition, settings)
        {
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

            texture = TextureLoader.Load(@"characters/Ice Golem", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/Ice Golem.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(60, 70);
            boundingBoxWidth = 39;
            boundingBoxHeight = 40;
        }

        public override void hurt()
        {
            golemsEffects[new Random().Next(0, 5)].Play();
            base.hurt();
        }
    }
}
