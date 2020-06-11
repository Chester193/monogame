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
    class Ogre : Enemy
    {
        public Ogre(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.maxHp = settings.maxHp;
            this.hp = settings.maxHp;
            this.rangeOfAttack = settings.rangeOfAttack;

            SetMode(settings.mode);
            SetRange(settings.range);
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

            texture = TextureLoader.Load(@"characters/Ogre", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/Ogre.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(60, 70);
            boundingBoxWidth = 39;
            boundingBoxHeight = 40;
        }
    }
}
