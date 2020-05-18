using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Lizard : Enemy
    {
        public Lizard(Vector2 startingPosition, CharacterSetings setings)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.maxHp = setings.maxHp;
            this.hp = setings.maxHp;
            this.rangeOfAttack = setings.rangeOfAttack;

            SetMode(setings.mode);
            SetRange(setings.range);
            points = setings.points;

            //this.oryginalPosition = BoundingBox.Center.ToVector2(); // new Vector2(startingPosition.X, startingPosition.Y);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 1f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/lizard", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/lizard.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(90, 140);
            boundingBoxWidth = 30;
            boundingBoxHeight = 15;
        }
    }
}