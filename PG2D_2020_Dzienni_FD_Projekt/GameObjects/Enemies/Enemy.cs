using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            //FollowPlayer(gameObjects);
            base.Update(gameObjects, map);
        }

        protected override void UpdateAnimations()
        {
            if (direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
            {
                ChangeAnimation(Animations.WalkingLeft);
            }
            else if (direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
            {
                ChangeAnimation(Animations.WalkingRight);
            }
            base.UpdateAnimations();
            
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

        public void Guard(List<GameObject> gameObjects, int range)
        {

        }
    }
}
