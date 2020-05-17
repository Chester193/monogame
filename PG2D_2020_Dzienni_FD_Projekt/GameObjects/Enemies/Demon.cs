using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Demon : Enemy
    {
        public Demon(Vector2 startingPosition, CharacterSetings setings)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.maxHp = setings.maxHp;
            this.hp = setings.maxHp;
            this.rangeOfAttack = setings.rangeOfAttack;

            SetMode(setings.mode);
            SetRange(setings.range);
        }

        public override void Initialize()
        {
            maxSpeed = 3.0f;
            acceleration = 0.5f;
            scale = 1f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/demon", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/demon.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(80, 170);
            boundingBoxWidth = 30;
            boundingBoxHeight = 15;
        }
    }
}