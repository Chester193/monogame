using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class Character : AnimatedObject
    {
        public PathFinder pathFinder = new PathFinder();
        public Vector2 velocity;
        protected float acceleration = 0.4f;
        protected float deceleration = 0.78f;
        protected float maxSpeed = 4.0f;

        const float gravity = 1.0f;
        const float jumpVelocity = 16.0f;
        const float terminalVelocity = 32.0f;

        protected bool isAttacking = false;
        protected bool isJumping = false;
        public static bool applyGravity = false;
        const bool drawPath = true;

        public int maxHp;
        public int hp;

        public int maxMp;
        public int mp;

        Texture2D pathTexture;
        Color pathColor;
        protected int pathWidth, pathHeight;

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

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            UpdateMovement(gameObjects, map);
            base.Update(gameObjects, map);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (drawPath)
            {
                foreach (Point item in pathFinder.visited_test)
                    spriteBatch.Draw(pathTexture, new Vector2(item.X, item.Y), new Rectangle(item.X, item.Y, pathWidth, pathHeight), new Color(255, 0, 0, 128), rotation, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);

                foreach (Node item in pathFinder.available_test)
                    spriteBatch.Draw(pathTexture, new Vector2(item.Position.X, item.Position.Y), new Rectangle((int)item.Position.X, (int)item.Position.Y, pathWidth, pathHeight), new Color(0, 255, 0, 128), rotation, Vector2.Zero, 1f, SpriteEffects.None, 0.15f);

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

            if (applyGravity == true)
            {
                ApplyGravity(map);
            }

            velocity.X = ApplyDrag(velocity.X, deceleration);
            if (applyGravity == false)
            {
                velocity.Y = ApplyDrag(velocity.Y, deceleration);
            }

        }

        public void Follow(GameObject player, TiledMap map)
        {
            Vector2 nextStep;
            if (!pathFinder.TryGetFirstStep(out nextStep) || GoToPoint(nextStep))
            {
                pathFinder.FindPath(map, new Vector2(BoundingBox.X, BoundingBox.Y), new Vector2(player.BoundingBox.X, player.BoundingBox.Y));
            }
        }

        public bool GoToPoint(Vector2 point)
        {
            bool arriveX = false, arriveY = false;
            float directionX = point.X - BoundingBox.X;
            float directionY = point.Y - BoundingBox.Y;

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

            return arriveX && arriveY;
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
            hp -= dmg;
            if (hp <= 0) hp = 0;
        }

        public void ManaUse(int mpUsed)
        {
            if (mpUsed > mp) throw new NotEnoughMpException();
            mp -= mpUsed;
            if (mp <= 0) mp = 0;
        }

        public void Heal(int points)
        {
            hp += points;
            if (hp >= maxHp) hp = maxHp;
        }
        
        public void Heal()
        {
            hp = maxHp;
        }

        public String HpToString()
        {
            String hpS = hp.ToString();
            return hpS;
        }

        public String MaxHpToString()
        {
            String hpS = maxHp.ToString();
            return hpS;
        }

        public String MpToString()
        {
            String mpS = mp.ToString();
            return mpS;
        }

        public String MaxMpToString()
        {
            String mpS = maxMp.ToString();
            return mpS;
        }

        public void MaxHpAdd(int addHp)
        {
            maxHp += addHp;
        }

        public void SetMaxHp(int newMaxHp)
        {
            maxHp = newMaxHp;
        }

    }
}

public class NotEnoughMpException : Exception
{
    public NotEnoughMpException() : base() { }
}