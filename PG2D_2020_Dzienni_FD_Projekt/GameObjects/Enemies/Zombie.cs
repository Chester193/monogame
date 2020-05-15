using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies
{
    class Zombie : Enemy
    {
        public Zombie(Vector2 startingPosition, int maxHp,int mode)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.maxHp = maxHp;
            this.hp = maxHp;

            SetMode(mode);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 0.3f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/zombie", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/zombie.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(40, 150);
            boundingBoxWidth = 70;
            boundingBoxHeight = 40;
        }
    }
}
