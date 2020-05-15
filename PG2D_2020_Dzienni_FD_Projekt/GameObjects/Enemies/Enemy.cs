using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            //FollowPlayer(gameObjects);
            Guard(gameObjects, 200);
            //WhaitForPlayer(gameObjects, 200);
            base.Update(gameObjects, map);
        }

        protected override void UpdateAnimations()
        {
            if (velocity != Vector2.Zero && direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
             {
                 ChangeAnimation(Animations.WalkingLeft);
             }
             else if (velocity != Vector2.Zero && direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
             {
                 ChangeAnimation(Animations.WalkingRight);
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
             }

             if (isAttacking)
             {
                 velocity = Vector2.Zero;
                 if (direction.X <= 0 && AnimationIsNot(Animations.SlashLeft))
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

             if (velocity == Vector2.Zero && isJumping == false && isAttacking == false)
             {               
                if ((direction.X <= 0) && AnimationIsNot(Animations.IdleLeft))
                {
                    ChangeAnimation(Animations.IdleLeft);
                }
                else if ((direction.X > 0) && AnimationIsNot(Animations.IdleRight))
                {
                    ChangeAnimation(Animations.IdleRight);
                }
                
            }

            base.UpdateAnimations();
        }

        public void FollowPlayer(List<GameObject> gameObjects)
        {
            GameObject player = gameObjects[0];
            Rectangle playerBox = player.BoundingBox;
            float directionX = playerBox.X - BoundingBox.X;
            float directionY = playerBox.Y - BoundingBox.Y;

            if (directionY > maxSpeed)
                MoveDown();
            else if (directionY < -maxSpeed)
                MoveUp();

            if (directionX > maxSpeed)
                MoveRight();
            else if (directionX < -maxSpeed)
                MoveLeft();
        }

        public void WhaitForPlayer(List<GameObject> gameObjects, int range)
        {
            GameObject player = gameObjects[0];
            Vector2 v = new Vector2(range, range);
            if((player.position.X < center.X + range) && (player.position.X > center.X - range) && (player.position.Y < center.Y + range) && (player.position.Y > center.Y - range))
            {
                FollowPlayer(gameObjects);
            }
                        
        }

        public void Guard(List<GameObject> gameObjects, int range)
        {
            GameObject player = gameObjects[0];
            Vector2 v = new Vector2(range, range);
            if ((player.position.X < oryginalPosition.X + range + 100) && (player.position.X > oryginalPosition.X - range) && (player.position.Y < oryginalPosition.Y + range + 100) && (player.position.Y > oryginalPosition.Y - range)) //+100 aby centru obszary znajdowalo sie na przeciwniu (+/-)
            {
                FollowPlayer(gameObjects);
                
            }
            else
            {
                if (position != oryginalPosition) GoToPositon(oryginalPosition);
            }

        }

        public string DirectionToString()
        {
            string s = direction.ToString();
            return s;
        }
    }
}
