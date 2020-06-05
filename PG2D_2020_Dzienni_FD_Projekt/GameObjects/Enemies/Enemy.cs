using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        private int step = 0;

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            CharcterMode mode = GetMode();
            int range = GetRange();

            if (!isDead)
            {
                switch (mode)
                {
                    case CharcterMode.WaitForPlayer:
                        WaitForPlayer(gameObjects, range, map);
                        break;
                    case CharcterMode.Guard:
                        Guard(gameObjects, range, map);
                        break;
                    case CharcterMode.FollowPlayer:
                        FollowPlayer(gameObjects, map);
                        break;

                    default:
                        WaitForPlayer(gameObjects, range, map);
                        break;
                }
            }

            base.Update(gameObjects, map);
        }

        protected override void UpdateAnimations()
        {
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
            else
            {
                if (velocity != Vector2.Zero && direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
                {
                    ChangeAnimation(Animations.WalkingLeft);
                }
                else if (velocity != Vector2.Zero && direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
                {
                    ChangeAnimation(Animations.WalkingRight);
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
            }


            base.UpdateAnimations();
        }
        
        public void FollowPlayer(List<GameObject> gameObjects, TiledMap map)
        {
            GameObject player = gameObjects[0];
            Rectangle playerBox = player.BoundingBox;

            Vector2 targetPoint = playerBox.Center.ToVector2();
            Follow(player, map);
            Attack((Character)player, 20);
        }

        public void WaitForPlayer(List<GameObject> gameObjects, int range, TiledMap map)
        {
            Player player = (Player)gameObjects[0];
            Vector2 v = new Vector2(range, range);

            float distanceToPlayer = Vector2.Distance(player.realPositon, realPositon);
            if (distanceToPlayer < range)
            {
                Follow(gameObjects[0], map);
            }
            Attack(player, 20);
        }

        public void Guard(List<GameObject> gameObjects, int range, TiledMap map)
        {
            Player player = (Player)gameObjects[0];
            Vector2 v = new Vector2(range);
            float distanceToPlayer = Vector2.Distance(player.realPositon, realPositon);
            float distanceToGuardPosition = Vector2.Distance(originalPosition, realPositon);
            if (distanceToPlayer < range)
            {
                if (distanceToGuardPosition <= 2 * range)
                {
                    Follow(gameObjects[0], map);
                }
            }
            else
            {
                Patrol();
            }
            Attack(player, 20);
        }

        public void Patrol()
        {
            float distance;
            if (points != null)
            {
                if (step > points.Count)
                    step = 0;
                else
                {
                    if (step == points.Count)
                    {
                        GoToPositon(originalPosition);
                        distance = Vector2.Distance(realPositon, originalPosition);
                        if (distance < 5) step++;
                    }
                    else
                    {
                        distance = Vector2.Distance(realPositon, points[step]);
                        if (distance < 5)
                            step++;
                        else
                            GoToPositon(points[step]);
                        if (realPositon == originalPosition)
                            step++;
                    }

                }
                //Console.WriteLine("step: " + step);
            }
        }

        public string DirectionToString()
        {
            string s = direction.ToString();
            return s;
        }

    }
}
