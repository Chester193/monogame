using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Orc
{
    class Orc : Enemy
    {

        Axe axe;
        private int attackDelay;

        public Orc(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;

            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            attackDelay = 0;
            maxSpeed = 1.0f;
            acceleration = 0.2f;
            scale = 0.5f;
            axe = new Axe();
            base.Initialize();
        }
        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            attackDelay--;
            axe.Update(gameObjects, map, gameTime);
            if (!isAttacking && active)
            {
                Fire(gameObjects[0]);
            }
            base.Update(gameObjects, map, gameTime);
        }

        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/Orc", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/Orc.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(AnimatedObject.Animations.WalkingRight);

            axe.Load(content);
            base.Load(content);

            boundingBoxOffset = new Vector2(40, 95);
            boundingBoxWidth = 26;
            boundingBoxHeight = 12;
        }

        public override void Attack(Character target, int dmg)
        {
            //Do nothing
        }

        public void Fire(GameObject player)
        {
            if (axe.active == false && !isDead && Vector2.Distance(player.position, position) <= characterSettings.rangeOfAttack && attackDelay <= 0)
            {
                goblinEffects[1].Play();
                isAttacking = true;
                axe.Fire(this, new Vector2(this.BoundingBox.X, this.BoundingBox.Y), new Vector2(player.BoundingBox.X, player.BoundingBox.Y));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            axe.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        public void resetAttackDelay()
        {
            this.attackDelay = 80;
        }

        public override void hurt()
        {
            goblinEffects[0].Play();
            base.hurt();
        }

        public override void Die()
        {
            goblinEffects[2].Play();
            base.Die();
        }
    }
}
