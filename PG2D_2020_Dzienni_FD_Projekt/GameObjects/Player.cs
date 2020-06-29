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
        private List<Quest> quests;
        private int currentQuestIndex = 0;
        private Character target = null;

        public Player()
        {
            applyGravity = false;
        }

        public Player(Vector2 startingPosition, Scripts.Scripts scripts, List<Quest> quests)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.scripts = scripts;
            this.quests = quests;
        }

        public bool TryGetCurrentQuest(out Quest currentQuest)
        {
            if (currentQuestIndex < quests.Count)
            {
                currentQuest = quests[currentQuestIndex];
                return true;
            }

            currentQuest = null;
            return false;

        }

        public override void Initialize()
        {
            characterSettings.maxHp = 100;
            characterSettings.hp = 100;
            characterSettings.maxMp = 10;
            characterSettings.mp = 10;

            characterSettings.rangeOfAttack = 30;
            characterSettings.weaponAttack = 30;

            base.Initialize();
        }


        public override void Load(ContentManager content)
        {

            texture = TextureLoader.Load(@"characters/warrior", content);
            SpriteAtlasData atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/warrior.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(Animations.IdleRight);

            base.Load(content);

            boundingBoxOffset = new Vector2(40, 57);
            boundingBoxWidth = 32;
            boundingBoxHeight = 32;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            Quest currentQuest;
            if (TryGetCurrentQuest(out currentQuest))
            {
                currentQuest.Update();
                if (currentQuest.State == QuestState.Done)
                    currentQuestIndex++;
            }

            if (hit) Attack(target, characterSettings.weaponAttack);

            if (!isAttacking && !isHurting && !isDead)
                CheckInput(gameObjects, map);
            base.Update(gameObjects, map, gameTime);
        }

        protected override void UpdateAnimations()
        {
            if (currentAnimation == null)
                return;

            if (isDead)
            {
                if (direction.Y < 0 && AnimationIsNot(Animations.DieBack))
                {
                    ChangeAnimation(Animations.DieBack);
                }
                if (direction.Y > 0 && AnimationIsNot(Animations.DieFront))
                {
                    ChangeAnimation(Animations.DieFront);
                }
                if (direction.X < 0 && AnimationIsNot(Animations.DieLeft))
                {
                    ChangeAnimation(Animations.DieLeft);
                }
                if (direction.X > 0 && AnimationIsNot(Animations.DieRight))
                {
                    ChangeAnimation(Animations.DieRight);
                }

                if (IsAnimationComplete)
                {
                    scripts.PlayerRespawn();
                }
            }

            else if (isHurting)
            {
                currentAnimation.animationSpeed = 1;
                velocity = Vector2.Zero;
                if (direction.Y < 0 && AnimationIsNot(Animations.HurtBack))
                {
                    ChangeAnimation(Animations.HurtBack);
                }
                if (direction.Y > 0 && AnimationIsNot(Animations.HurtFront))
                {
                    ChangeAnimation(Animations.HurtFront);
                }
                if (direction.X < 0 && AnimationIsNot(Animations.HurtLeft))
                {
                    ChangeAnimation(Animations.HurtLeft);
                }
                if (direction.X > 0 && AnimationIsNot(Animations.HurtRight))
                {
                    ChangeAnimation(Animations.HurtRight);
                }
                if (IsAnimationComplete)
                {
                    isHurting = false;
                }
            }

            else if (isAttacking)
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
                    hit = true;
                    isAttacking = false;
                }
            }

            else if (velocity != Vector2.Zero)
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

            else
            //(velocity == Vector2.Zero && isJumping == false && isAttacking == false && isDead == false && isHurting == false)
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
            base.UpdateAnimations();
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
                isAttacking = true;
                MeleAttack(gameObjects);
            }

            //HUD tests:
            if (Input.KeyPressed(Keys.H) == true)
                Heal();
            if (Input.KeyPressed(Keys.J) == true)
                Heal(15);
            if (Input.KeyPressed(Keys.K) == true)
                MaxHpAdd(50);
        }

        private void MeleAttack(List<GameObject> gameObjects)
        {
            target = NearestEnemy(gameObjects);
            //if (enemyInRange != null) Attack(enemyInRange, characterSettings.weaponAttack);
        }

        private Character NearestEnemy(List<GameObject> gameObjects)
        {
            float distance = 0, distancePrev = 0, weaponDistance = 0;
            Character character;
            Character target = null;
            Vector2 weaponPositon = new Vector2(realPositon.X + (direction.X * characterSettings.rangeOfAttack), realPositon.Y + (direction.Y * characterSettings.rangeOfAttack));

            for (int i = 1; i < gameObjects.Count; i++)
            {
                try
                {
                    character = (Character)gameObjects[i];
                    //Console.WriteLine("i: " + i + " hp " + character.HpToString());

                    if (!character.IsDead())
                    {
                        distance = Vector2.Distance(character.realPositon, realPositon);
                        weaponDistance = Vector2.Distance(character.realPositon, weaponPositon);
                        Console.WriteLine("dist: " + distance + " W-dist: " + weaponDistance);
                        if (distancePrev == 0) distancePrev = weaponDistance;
                        if (weaponDistance <= distancePrev)
                        {
                            distancePrev = weaponDistance;
                            if (distance < characterSettings.rangeOfAttack && weaponDistance < characterSettings.rangeOfAttack) target = character;
                        }
                    }
                }

                catch (InvalidCastException e)
                {

                }


            }

            Console.WriteLine(target);

            return target;
        }

        public string Interact()
        {
            Quest currentQuest;

            if (TryGetCurrentQuest(out currentQuest))
            {
                return currentQuest.getDialog();
            }
            else
            {
                return Quest.defaultDialog;
            }
        }
    }
}
