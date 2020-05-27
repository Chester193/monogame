using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class Player : Character
    {
        public Player()
        {
            applyGravity = false;
        }

        public Player(Vector2 startingPosition)
        {
            this.position = startingPosition;
            applyGravity = false;

        }

        public override void Initialize()
        {
            maxHp = 1000;
            hp = 1000;
            maxMp = 10;
            mp = 10;

            rangeOfAttack = 150;

            base.Initialize();
        }


        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/warrior", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/warrior.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(Animations.IdleRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(40, 75);
            boundingBoxWidth = 30;
            boundingBoxHeight = 15;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            if(!isAttacking)
                CheckInput(gameObjects, map);
            base.Update(gameObjects, map);
        }

        protected override void UpdateAnimations()
        {
            if (currentAnimation == null)
                return;

            base.UpdateAnimations();

            if (isAttacking)
            {
                velocity = Vector2.Zero;
                if (direction.Y < 0 && AnimationIsNot(Animations.SlashBack))
                {
                    ChangeAnimation(Animations.SlashBack);
                }
                if (direction.Y > 0 && AnimationIsNot(Animations.SlashFront))
                {
                    ChangeAnimation(Animations.SlashFront);
                }
                if (direction.X < 0 && AnimationIsNot(Animations.SlashLeft))
                {
                    ChangeAnimation(Animations.SlashLeft);
                }
                if (direction.X > 0 && AnimationIsNot(Animations.SlashRight))
                {
                    ChangeAnimation(Animations.SlashRight);
                }
                if (IsAnimationComplete)
                {
                    isAttacking = false;
                }
            }

            if (velocity != Vector2.Zero && isJumping == false && isAttacking == false)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
                {
                    ChangeAnimation(Animations.WalkingLeft);
                }
                else if (direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
                {
                    ChangeAnimation(Animations.WalkingRight);
                }

                if (direction.Y < 0 && AnimationIsNot(Animations.WalkingBack))
                {
                    ChangeAnimation(Animations.WalkingBack);
                }
                else if (direction.Y > 0 && AnimationIsNot(Animations.WalkingFront))
                {
                    ChangeAnimation(Animations.WalkingFront);
                }

            }

            else if (velocity == Vector2.Zero && isJumping == false && isAttacking == false)
            {
                if (direction.X < 0 && AnimationIsNot(Animations.IdleLeft))
                {
                    ChangeAnimation(Animations.IdleLeft);
                }
                else if (direction.X > 0 && AnimationIsNot(Animations.IdleRight))
                {
                    ChangeAnimation(Animations.IdleRight);
                }
                if (direction.Y < 0 && AnimationIsNot(Animations.IdleBack))
                {
                    ChangeAnimation(Animations.IdleBack);
                }
                else if (direction.Y > 0 && AnimationIsNot(Animations.IdleFront))
                {
                    ChangeAnimation(Animations.IdleFront);
                }
            }

            if (hp <= 0)
            {
                if (direction.X < 0)
                {
                    ChangeAnimation(Animations.DieLeft);
                }
                else if (direction.X > 0)
                {
                    ChangeAnimation(Animations.DieRight);
                }
                if (direction.Y < 0)
                {
                    ChangeAnimation(Animations.DieBack);
                }
                else if (direction.Y > 0)
                {
                    ChangeAnimation(Animations.DieFront);
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }


        private void CheckInput(List<GameObject> gameObjects, TiledMap map)
        {
            if (Input.IsKeyDown(Keys.D) == true)
                MoveRight();
            if (Input.IsKeyDown(Keys.A) == true)
                MoveLeft();
            if (Input.IsKeyDown(Keys.S) == true)
                MoveDown();
            if (Input.IsKeyDown(Keys.W) == true)
                MoveUp();

            if (Input.KeyPressed(Keys.Space))
            {
                Fire(gameObjects);
            }

            //HUD tests:
            if (Input.KeyPressed(Keys.H) == true)
                Heal();
            if (Input.KeyPressed(Keys.J) == true)
                Heal(15);
            if (Input.KeyPressed(Keys.K) == true)
               MaxHpAdd(50);
        }

        private void Fire(List<GameObject> gameObjects)
        {
            Character enemyInRange = NearestEnemy(gameObjects);
            if(enemyInRange != null) Attack(enemyInRange, 1000);

            //Console.WriteLine("enmyInRange" + enemyInRange.ToString());
            

            //Console.WriteLine("Fire()");
            //HUD test
            try
            {
                ManaUse(1);
            }
            catch(NotEnoughMpException e)
            {
                Damage(20);
            }
            
        }

        private Character NearestEnemy(List<GameObject> gameObjects)
        {
            float distans = 0, distansPrev = 0;
            Character character;
            Character target = null;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                character = (Character)gameObjects[i];
                if(!character.IsDead())
                { 
                    distans = Vector2.Distance(character.realPositon, realPositon);
                    if (distansPrev == 0) distansPrev = distans;
                    if (distans < distansPrev)
                    {
                        distansPrev = distans;
                        target = character;
                        Console.WriteLine("NarestEnemy " + target.ToString());
                    }
                }
            }
            
            //Console.WriteLine("NearestEnemy() distans " + distans + " GO.count " + gameObjects.Count);

            return target; // = (Character)gameObjects[1];
        }
    }
}
