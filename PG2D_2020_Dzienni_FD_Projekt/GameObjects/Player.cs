using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
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
        private Texture2D secondTexture;
        private SpriteAtlasData atlas, secondAtlas;
        private List<Quest> quests;
        private int currentQuestIndex = 0;
        private GameHUD hud;

        public bool isRanged = false;
        private int fireDelay;
        Fireball fireBall;

        public int Money { get; private set; } = 0;
        public int Exp { get; private set; } = 0;
        public InventoryItem Weapon { get; set; }
        public InventoryItem Armour { get; set; }

        SoundEffect slash;
        SoundEffect inventoryOpen, coin;
        SoundEffect dyingEffect;
        SoundEffectInstance step;
        List<SoundEffect> hurtingEffects;
        List<SoundEffect> fireBallSound;

        public Player()
        {
            applyGravity = false;
        }

        public Player(Vector2 startingPosition, Scripts.Scripts scripts, List<Quest> quests, GameHUD hud)
        {
            this.position = startingPosition;
            applyGravity = false;

            this.scripts = scripts;
            this.quests = quests;

            this.hud = hud;

            target = null;

            characterSettings.armour = 1;
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
            characterSettings.maxHp = 80;
            characterSettings.hp = 80;
            characterSettings.maxMp = 10;
            characterSettings.mp = 10;

            characterSettings.rangeOfAttack = 30;
            characterSettings.weaponAttack = 10;

            fireDelay = 0;
            fireBall = new Fireball();

            hurtingEffects = new List<SoundEffect>();
            fireBallSound = new List<SoundEffect>();
            Weapon = Inventory[0];
            Inventory.RemoveAt(0);

            Armour = Inventory[0];
            Inventory.RemoveAt(0);

            base.Initialize();
        }


        public override void Load(ContentManager content)
        {
            texture = TextureLoader.Load(@"characters/warrior", content);
            secondTexture = TextureLoader.Load(@"characters/secondWarrior", content);
            atlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/warrior.atlas", texture, content);
            secondAtlas = SpriteAtlasLoader.ParseSpriteAtlas(@"characters/secondWarrior.atlas", texture, content);

            LoadAnimations(atlas);
            ChangeAnimation(Animations.IdleRight);

            fireBall.Load(content);

            slash = content.Load<SoundEffect>(@"SoundEffects/swing");
            inventoryOpen = content.Load<SoundEffect>(@"SoundEffects/cloth");
            coin = content.Load<SoundEffect>(@"SoundEffects/coin");
            hurtingEffects.Add(content.Load<SoundEffect>(@"SoundEffects/damage1"));
            hurtingEffects.Add(content.Load<SoundEffect>(@"SoundEffects/damage2"));
            hurtingEffects.Add(content.Load<SoundEffect>(@"SoundEffects/damage3"));
            fireBallSound.Add(content.Load<SoundEffect>(@"SoundEffects/Fireball1"));
            fireBallSound.Add(content.Load<SoundEffect>(@"SoundEffects/Fireball2"));
            fireBallSound.Add(content.Load<SoundEffect>(@"SoundEffects/Fireball3"));
            dyingEffect = content.Load<SoundEffect>(@"SoundEffects/death");
            step = content.Load<SoundEffect>(@"SoundEffects/footstep06").CreateInstance();
            step.IsLooped = true;
            step.Volume = 0.4f;

            base.Load(content);

            boundingBoxOffset = new Vector2(40, 57);
            boundingBoxWidth = 32;
            boundingBoxHeight = 32;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            Quest currentQuest;
            fireDelay--;
            fireBall.Update(gameObjects, map, gameTime);

            if (TryGetCurrentQuest(out currentQuest))
            {
                currentQuest.Update();
                if (currentQuest.State == QuestState.Done)
                    currentQuestIndex++;
            }

            if (hit) Attack(this.target, characterSettings.weaponAttack);

            if (!isAttacking && !isHurting && !isDead)
            {
                if (!velocity.Equals(Vector2.Zero))
                    step.Play();
                else
                    step.Stop();
                    
                CheckInput(gameObjects, map);
            }
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
                isAttacking = false;
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
                currentAnimation.animationSpeed = 1;
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
            fireBall.Draw(spriteBatch);
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
                if (!isRanged)
                {
                    isAttacking = true;
                    slash.Play();
                    MeleAttack(gameObjects);
                }
                else if(!fireBall.active){
                    try
                    {
                        ManaUse(1);
                        isHurting = true;
                        Fire();
                        fireBallSound[new Random().Next(0, 3)].Play();
                    }
                    catch(NotEnoughMpException e)
                    {
                        hud.PrintMessage("Not enough Mana", 100);
                    }
                }
            }

            if (Input.KeyPressed(Keys.Tab))
                inventoryOpen.Play();

            //HUD tests:
            if (Input.KeyPressed(Keys.H) == true)
                Heal();
            if (Input.KeyPressed(Keys.J) == true)
                Heal(15);
            if (Input.KeyPressed(Keys.K) == true)
                MaxHpAdd(50);
        }

        public void ChangeArmour()
        {
            Texture2D tmpTexture = texture;
            texture = secondTexture;
            secondTexture = tmpTexture;
            SpriteAtlasData tmpAtlas = atlas;
            atlas = secondAtlas;
            secondAtlas = tmpAtlas;
            LoadAnimations(atlas);
            ChangeAnimation(Animations.IdleFront);
            if (characterSettings.armour == 1)
            {
                characterSettings.armour = 0.6f;
            }
            else
            {
                characterSettings.armour = 1f;
            }
        }

        public void Fire()
        {
            if (fireBall.active == false && !isDead)
            {
                fireBall.Fire(this, new Vector2(this.BoundingBox.X, this.BoundingBox.Y));
            }
        }

        private void MeleAttack(List<GameObject> gameObjects)
        {
            target = NearestEnemy(gameObjects);
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

                    if (!character.IsDead())
                    {
                        distance = Vector2.Distance(character.realPositon, realPositon);
                        weaponDistance = Vector2.Distance(character.realPositon, weaponPositon);
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

        public void EarnMoney(int amount)
        {
            Money += amount;
            coin.Play();
        }

        public void SpendMoney(int amount)
        {
            if (Money - amount < 0)
                throw new NotEnoughMoneyException();

            Money -= amount;
        }

        public void GainExperience(int amount)
        {
            Exp += amount;
            characterSettings.maxHp = 80 + Exp / 50;
        }

        public override void hurt()
        {
            if (!isDead)
            {
                isHurting = true;
                hurtingEffects[new Random().Next(0, 3)].Play();         
            }
        }

        public override void Die()
        {
            if(!isDead)
                dyingEffect.Play();
            base.Die();
        }

        public void resetFireDelay()
        {
            this.fireDelay = 80;
        }
    }
}
public class NotEnoughMoneyException : Exception
{
    public NotEnoughMoneyException() : base() { }
}