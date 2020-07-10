using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public enum CharcterMode
    {
        WaitForPlayer,
        Guard,
        FollowPlayer
    }

    public struct CharacterSettings
    {
        public int maxHp;
        public int hp;

        public CharcterMode mode;
        public int range;
        public int rangeOfAttack;
        public int weaponAttack;

        public int maxMp;
        public int mp;
    }

    public class Character : AnimatedObject
    {
        public List<InventoryItem> Inventory { get; private set; }
        public PathFinder pathFinder = new PathFinder();
        public SoundEffect soundEffecct;
        public Timer timer = new Timer();
        public Vector2 velocity;
        protected float acceleration = 0.4f;
        protected float deceleration = 0.78f;
        protected float maxSpeed = 4.0f;
        protected float armour = 1.0f;

        const float gravity = 1.0f;
        const float jumpVelocity = 16.0f;
        const float terminalVelocity = 32.0f;

        protected bool isDead = false;
        protected bool isAttacking = false;
        protected bool hit = false;
        protected Character target = null;
        protected bool isJumping = false;
        protected bool isHurting = false;

        public static bool applyGravity = false;
        const bool drawPath = false;

        Texture2D pathTexture;
        Color pathColor;
        bool positionChanged = true;
        protected int pathWidth, pathHeight;

        public Vector2 realPositon;

        public Scripts.Scripts scripts;

        public CharacterSettings characterSettings;

        public Character()
        {
            Inventory = new List<InventoryItem>();
        }

        public override void Initialize()
        {
            velocity = Vector2.Zero;
            isJumping = false;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            pathTexture = TextureLoader.Load(@"other/pixel", content);

            base.Load(content);

            pathColor = new Color(0, 0, 255, 128);
            pathWidth = 32;
            pathHeight = 32;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map, GameTime gameTime)
        {
            UpdateMovement(gameObjects, map);
            base.Update(gameObjects, map, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (active && !isDead && characterSettings.hp < characterSettings.maxHp && this is Enemy)
            {
                int maxLength = 100;
                Vector2 pos = new Vector2(BoundingBox.Center.X - maxLength / 2, position.Y);
                Rectangle currentLevel = new Rectangle((int)pos.X, (int)pos.Y, characterSettings.hp * maxLength / characterSettings.maxHp, 10);
                Rectangle background = new Rectangle((int)pos.X, (int)pos.Y, maxLength, 10);
                spriteBatch.Draw(pathTexture, pos, currentLevel, Color.DarkGreen, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.09f);
                spriteBatch.Draw(pathTexture, pos, background, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            }

            if (drawPath)
            {
                //foreach (Point item in pathFinder.visited_test)
                //    spriteBatch.Draw(pathTexture, new Vector2(item.X, item.Y), new Rectangle(item.X, item.Y, pathWidth, pathHeight), new Color(255, 0, 0, 128), rotation, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);

                //foreach (Node item in pathFinder.available_test)
                //    spriteBatch.Draw(pathTexture, new Vector2(item.Position.X, item.Position.Y), new Rectangle((int)item.Position.X, (int)item.Position.Y, pathWidth, pathHeight), new Color(0, 255, 0, 128), rotation, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);

                foreach (Vector2 item in pathFinder.Path)
                    spriteBatch.Draw(pathTexture, new Vector2(item.X, item.Y), new Rectangle((int)item.X, (int)item.Y, pathWidth, pathHeight), pathColor, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0.05f);
            }
        }

        private void UpdateMovement(List<GameObject> gameObjects, TiledMap map)
        {
            if (velocity.X != 0 && CheckCollisions(gameObjects, map, true) == true)
            {
                velocity.X = 0;
            }

            position.X += velocity.X;

            if (velocity.Y != 0 && CheckCollisions(gameObjects, map, false) == true)
            {
                velocity.Y = 0;
            }

            position.Y += velocity.Y;

            positionChanged = velocity.X != 0 || velocity.Y != 0;

            if (applyGravity == true)
            {
                ApplyGravity(map);
            }

            velocity.X = ApplyDrag(velocity.X, deceleration);
            if (applyGravity == false)
            {
                velocity.Y = ApplyDrag(velocity.Y, deceleration);
            }

            realPositon = BoundingBox.Center.ToVector2();
            if (originalPosition == new Vector2(-1, -1)) originalPosition = new Vector2(realPositon.X, realPositon.Y);
        }

        public void Follow(TiledMap map, List<GameObject> gameObjects, int targetIndex = 0)
        {
            GameObject targetCharacter = gameObjects[targetIndex];
            List<GameObject> gameObjectsWithoutPlayer = new List<GameObject>(gameObjects);
            gameObjectsWithoutPlayer.Remove(targetCharacter);
            Vector2 target = new Vector2(targetCharacter.BoundingBox.Center.X, targetCharacter.BoundingBox.Center.Y);

            GoToPositon(map, gameObjectsWithoutPlayer, target);
        }

        public void GoToPositon(TiledMap map, List<GameObject> gameObjects, Vector2 target)
        {
            if (timer.Count())
            {
                return;
            }

            Vector2 nextStep;
            if (!pathFinder.TryGetFirstStep(out nextStep) || GoToPoint(nextStep))
            {
                List<GameObject> gameObjectsWithoutPlayer = new List<GameObject>(gameObjects);
                gameObjectsWithoutPlayer.Remove(this);
                bool pathFound = pathFinder.FindPath(map, gameObjectsWithoutPlayer, new Vector2(BoundingBox.Center.X, BoundingBox.Center.Y), target);
                if (!pathFound)
                {
                    timer.Time = 60;
                }
            }
        }

        public bool GoToPoint(Vector2 point)
        {
            bool arriveX = false, arriveY = false;
            float directionX = point.X - BoundingBox.Center.X;
            float directionY = point.Y - BoundingBox.Center.Y;

            if (isAttacking || isDead)
            {
                return false;
            }

            if (directionY > maxSpeed)
                MoveDown();
            else if (directionY < -maxSpeed)
                MoveUp();
            else
                arriveY = true;


            if (directionX > maxSpeed)
                MoveRight();
            else if (directionX < -maxSpeed)
                MoveLeft();
            else
                arriveX = true;

            return arriveX && arriveY || !positionChanged;
        }

        private void ApplyGravity(TiledMap map)
        {
            if (isJumping == true || OnGround(map) == Rectangle.Empty)
                velocity.Y += gravity;
            velocity.Y = Math.Min(velocity.Y, terminalVelocity);
        }


        //TODO: CONCATENATE MOVEMENT FUNCTIONS TO ONE 
        protected void MoveRight()
        {
            velocity.X += acceleration + deceleration;
            velocity.X = Math.Min(velocity.X, maxSpeed);
            direction.X = 1;
            direction.Y = 0;
        }

        protected void MoveLeft()
        {
            velocity.X -= acceleration + deceleration;
            velocity.X = Math.Max(velocity.X, -maxSpeed);
            direction.X = -1;
            direction.Y = 0;
        }

        public override void BulletResponse(int damageTaken)
        {
            isHurting = true;
            this.Damage(damageTaken);
            base.BulletResponse(damageTaken);
        }
        protected void MoveDown()
        {
            velocity.Y += acceleration + deceleration;
            velocity.Y = Math.Min(velocity.Y, maxSpeed);
            direction.Y = 1;
            direction.X = 0;
        }


        protected void MoveUp()
        {
            velocity.Y -= acceleration + deceleration;
            velocity.Y = Math.Max(velocity.Y, -maxSpeed);
            direction.Y = -1;
            direction.X = 0;
        }

        protected bool Jump(TiledMap map)
        {
            if (isJumping == true)
                return false;

            if (velocity.Y == 0 && OnGround(map) != Rectangle.Empty)
            {
                velocity.Y -= jumpVelocity;
                isJumping = true;
                return true;
            }

            return false;
        }

        protected virtual bool CheckCollisions(List<GameObject> gameObjects, TiledMap map, bool xAxis)
        {
            Rectangle futureBoundingBox = BoundingBox;
            int maxX = (int)maxSpeed, maxY = (int)maxSpeed;

            if (applyGravity == true)
                maxY = (int)jumpVelocity;

            if (xAxis == true && velocity.X != 0)
            {
                //TODO: make it better
                futureBoundingBox.X += Math.Sign(velocity.X) * maxX;
                //if (velocity.X > 0)
                //    futureBoundingBox.X += maxX;
                //else
                //    futureBoundingBox.X -= maxX;
            }
            else if (applyGravity == false && xAxis == false && velocity.Y != 0)
            {
                futureBoundingBox.Y += Math.Sign(velocity.Y) * maxY;
                //if (velocity.Y > 0)
                //    futureBoundingBox.Y += maxY;
                //else
                //    futureBoundingBox.Y -= maxY;
            }
            else if (applyGravity == true && xAxis == false && velocity.Y != gravity)
            {
                futureBoundingBox.Y += Math.Sign(velocity.Y) * maxY;
            }


            Rectangle wallCollision = map.CheckCollision(futureBoundingBox);

            if (wallCollision != Rectangle.Empty)
            {
                if (applyGravity == true && velocity.Y >= gravity && (futureBoundingBox.Bottom > wallCollision.Top - maxSpeed) && (futureBoundingBox.Bottom <= wallCollision.Top + velocity.Y))
                {
                    LandResponse(wallCollision);
                    return true;
                }
                else
                    return true;
            }

            foreach (var gameObject in gameObjects)
            {
                if (gameObject != this && gameObject.active == true && gameObject.isCollidable == true && gameObject.CheckCollision(futureBoundingBox) == true)
                {
                    return true;
                }
            }

            return false;

        }

        public void LandResponse(Rectangle wallCollision)
        {
            position.Y = wallCollision.Top - (boundingBoxHeight + boundingBoxOffset.Y);
            velocity.Y = 0;
            isJumping = false;
        }


        protected Rectangle OnGround(TiledMap map)
        {
            Rectangle futureBoundingBox = new Rectangle((int)(position.X + boundingBoxOffset.X), (int)(position.Y + boundingBoxOffset.Y + (velocity.Y + gravity)), boundingBoxWidth, boundingBoxHeight);

            return map.CheckCollision(futureBoundingBox);
        }

        protected float ApplyDrag(float val, float amount)
        {
            if (val > 0.0f && (val -= amount) < 0.0f) return 0.0f;
            if (val < 0.0f && (val += amount) > 0.0f) return 0.0f;
            return val;
        }


        public void Damage(int dmg)
        {
            characterSettings.hp -= (int)(dmg * armour);
            if (characterSettings.hp <= 0)
            {
                characterSettings.hp = 0;
                Die();
            }

            //Console.WriteLine("Character.Damage() " + dmg);
        }

        public virtual void Die()
        {
            isDead = true;
        }

        public void ManaUse(int mpUsed)
        {
            if (mpUsed > characterSettings.mp) throw new NotEnoughMpException();
            characterSettings.mp -= mpUsed;
            if (characterSettings.mp <= 0) characterSettings.mp = 0;
        }

        public bool IsHpFull()
        {
            return characterSettings.hp >= characterSettings.maxHp;
        }

        public bool IsMpFull()
        {
            return characterSettings.mp >= characterSettings.maxMp;
        }

        public void Heal(int points)
        {
            characterSettings.hp += points;
            if (IsHpFull()) characterSettings.hp = characterSettings.maxHp;
        }

        public void ChargeMana(int points)
        {
            characterSettings.mp += points;
            if (IsMpFull()) characterSettings.mp = characterSettings.maxMp;
        }

        public void Heal()
        {
            characterSettings.hp = characterSettings.maxHp;
        }

        public String HpToString()
        {
            String hpS = characterSettings.hp.ToString();
            return hpS;
        }

        public String MaxHpToString()
        {
            String hpS = characterSettings.maxHp.ToString();
            return hpS;
        }

        public String MpToString()
        {
            String mpS = characterSettings.mp.ToString();
            return mpS;
        }

        public String MaxMpToString()
        {
            String mpS = characterSettings.maxMp.ToString();
            return mpS;
        }

        public void MaxHpAdd(int addHp)
        {
            characterSettings.maxHp += addHp;
        }

        public void SetMaxHp(int newMaxHp)
        {
            characterSettings.maxHp = newMaxHp;
        }

        public virtual void Attack(Character target, int dmg)
        {
            hit = false;
            if (target == null) return;
            float distanceToTarget = Vector2.Distance(target.realPositon, realPositon);
            //Console.WriteLine("Character.Attack() " + distansToTarget + " / " + rangeOfAttack + " t.rPositon " + target.realPositon + " player.rPosioton" + realPositon);
            /*
            if (distanceToTarget < characterSettings.rangeOfAttack && !isAttacking)
            {
                isAttacking = true;
                target.hurt();
                target.Damage(dmg);
                //Console.WriteLine("Character.Attack()[EndIF]()");
            }
            */

            if (distanceToTarget < characterSettings.rangeOfAttack)
            {
                target.hurt();
                target.Damage(dmg);
            }
        }


        public void SetMode(CharcterMode mode)
        {
            this.characterSettings.mode = mode;
        }

        public CharcterMode GetMode()
        {
            return characterSettings.mode;
        }

        public int GetRange()
        {
            return characterSettings.range;
        }

        public void SetRange(int range)
        {
            this.characterSettings.range = range;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void Respawn()
        {
            isDead = false;
        }

        public void SetCharacterSettings(CharacterSettings settings)
        {
            this.characterSettings.maxHp = settings.maxHp;
            this.characterSettings.hp = settings.maxHp;

            SetMode(settings.mode);
            SetRange(settings.range);
            this.characterSettings.rangeOfAttack = settings.rangeOfAttack;
            this.characterSettings.weaponAttack = settings.weaponAttack;

            this.characterSettings.maxMp = settings.maxMp;
            this.characterSettings.mp = settings.mp;
        }

        public virtual void hurt()
        {
            isHurting = true;
        }
    }

}

public class NotEnoughMpException : Exception
{
    public NotEnoughMpException() : base() { }
}