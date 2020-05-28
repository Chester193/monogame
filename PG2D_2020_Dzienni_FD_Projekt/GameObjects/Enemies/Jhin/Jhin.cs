using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Jhin
{
    class Jhin : Enemy
    {
        IceCone cone;

        public Jhin(Vector2 startingPosition, CharacterSettings settings)
        {
            this.maxHp = settings.maxHp;
            this.hp = settings.maxHp;
            this.rangeOfAttack = settings.rangeOfAttack;

            SetMode(settings.mode);
            SetRange(settings.range);

            this.position = startingPosition;
            applyGravity = false;
        }

        protected override void UpdateAnimations()
        {
            base.UpdateAnimations();
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            cone.Update(gameObjects, map);
            Fire(gameObjects[0].position);
            base.Update(gameObjects, map);
        }

        public override void Initialize()
        {
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 0.5f;
            cone = new IceCone();
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/jhin", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/jhin.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.IdleRight);

            cone.Load(content);
            base.Load(content);

            boundingBoxOffset = new Vector2(0f, 25f);
            boundingBoxWidth = 26;
            boundingBoxHeight = 12;
        }

        public void Fire(Vector2 playerPosition)
        {
            if (cone.active == false && Vector2.Distance(playerPosition, position) <= rangeOfAttack)
                {
                    isAttacking = true;
                    cone.Fire(this, new Vector2(this.position.X - 50, this.position.Y - 50), playerPosition);
                }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            cone.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
