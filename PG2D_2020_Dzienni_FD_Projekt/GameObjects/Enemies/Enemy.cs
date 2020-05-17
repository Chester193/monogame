using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            int mode = GetMode();
            int range = GetRange();

            if (!isDead)
            {

                switch (mode)
                {
                    case 0:
                        WhaitForPlayer(gameObjects, range);
                        break;
                    case 1:
                        Guard(gameObjects, range);
                        break;
                    case 2:
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
                if (distansToGuardPosition <= 2* range)
                {
                    FollowPlayer(gameObjects);
                }
            }
            else
            {
                    GoToPositon(oryginalPosition);
            }
            Attack(player, 20);
        }

        public string DirectionToString()
        {
            string s = direction.ToString();
            return s;
        }
    }
}
