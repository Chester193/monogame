using Microsoft.Xna.Framework;
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
                    case CharcterMode.WhaitForPlayer:
                        WhaitForPlayer(gameObjects, range);
                        break;
                    case CharcterMode.Guard:
                        Guard(gameObjects, range);
                        break;
                    case CharcterMode.FollowPlayer:
                        FollowPlayer(gameObjects);
                        break;

                    default:
                        WhaitForPlayer(gameObjects, range);
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

        public void FollowPlayer(List<GameObject> gameObjects)
        {
            GameObject player = gameObjects[0];
            Rectangle playerBox = player.BoundingBox;

            Vector2 targetPoint = playerBox.Center.ToVector2();
            GoToPositon(targetPoint);
        }

        public void WhaitForPlayer(List<GameObject> gameObjects, int range)
        {
            Player player = (Player)gameObjects[0];
            Vector2 v = new Vector2(range, range);

            float distansToPlayer = Vector2.Distance(player.realPositon, realPositon);
            if (distansToPlayer < range)
            {
                FollowPlayer(gameObjects);
            }
            Attack(player, 20);
        }

        public void Guard(List<GameObject> gameObjects, int range)
        {
            Player player = (Player)gameObjects[0];
            Vector2 v = new Vector2(range);
            float distansToPlayer = Vector2.Distance(player.realPositon, realPositon);
            float distansToGuardPosition = Vector2.Distance(oryginalPosition, realPositon);
            if (distansToPlayer < range)
            {
                if (distansToGuardPosition <= 2 * range)
                {
                    FollowPlayer(gameObjects);
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
            float distans;
            if (points != null)
            {
                if (step > points.Count)
                    step = 0;
                else
                {
                    if (step == points.Count)
                    {
                        GoToPositon(oryginalPosition);
                        distans = Vector2.Distance(realPositon, oryginalPosition);
                        if (distans < 5) step++;
                    }
                    else
                    {
                        distans = Vector2.Distance(realPositon, points[step]);
                        if (distans < 5)
                            step++;
                        else
                            GoToPositon(points[step]);
                        if (realPositon == oryginalPosition)
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
