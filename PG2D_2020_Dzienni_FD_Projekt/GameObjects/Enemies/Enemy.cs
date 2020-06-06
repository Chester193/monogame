using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using StateMachine;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        private int step = 0;
        private float distanceToPlayer;

        private enum EState { IDLE, FOLLOW };
        private enum ETrigger { STOP, FOLLOW_PLAYER };

        private Fsm<EState, ETrigger> enemyAiMachine;

        List<GameObject> gameObjects;
        TiledMap map;

        public Enemy()
        {
            enemyAiMachine = Fsm<EState, ETrigger>.Builder(EState.IDLE)
                .State(EState.FOLLOW)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .Update(args => {
                        Console.WriteLine("FOLLOW");
                        FollowPlayer(gameObjects, map); 
                    })
                .State(EState.IDLE)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .Update(args => {
                        Console.WriteLine("IDLE");
                    })
            .Build();
        }

        public override void Update(List<GameObject> gameObjectsG, TiledMap mapG, GameTime gameTime)
        {
            gameObjects = gameObjectsG;
            map = mapG;

            CharcterMode mode = GetMode();
            int range = GetRange();
            distanceToPlayer = countDistanceToPlayer((Player)gameObjects[0]);

            if (!isDead)
            {
                switch (mode)
                {
                    case CharcterMode.WaitForPlayer:
                        WaitForPlayer();//gameObjects, range, map);
                        break;
                    case CharcterMode.Guard:
                        Guard(gameObjects, range, map);
                        break;
                    case CharcterMode.FollowPlayer:
                        FollowPlayer(gameObjects, map);
                        break;

                    default:
                        WaitForPlayer();// gameObjects, range, map);
                        break;
                }
            }
            enemyAiMachine.Update(TimeSpan.FromMilliseconds(gameTime.ElapsedGameTime.TotalMilliseconds));

            base.Update(gameObjects, map, gameTime);
        }

        protected override void UpdateAnimations()
        {
            if (isDead)
            {
                if (direction.X <= 0 && AnimationIsNot(Animations.DieLeft))
                {
                    ChangeAnimation(Animations.DieLeft);
                }
                else if (direction.X > 0 && AnimationIsNot(Animations.DieRight))
                {
                    ChangeAnimation(Animations.DieRight);
                }
                if (IsAnimationComplete)
                {
                    ChangeAnimation(null);
                }
            }
            else
            {
                if (velocity != Vector2.Zero && direction.X <= 0 && AnimationIsNot(Animations.WalkingLeft))
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
                    if (IsAnimationComplete || distanceToPlayer >= rangeOfAttack)
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
            Follow(gameObjects[0], map, gameObjects);
            Attack((Character)player, 20);
        }

        public void WaitForPlayer() //List<GameObject> gameObjects, int range, TiledMap map)
        {
            Player player = (Player)gameObjects[0];
            //Vector2 v = new Vector2(range, range);

            if (distanceToPlayer < 400)
            {
                Console.WriteLine("dsitance<<<<  :" + distanceToPlayer);
                //Follow(gameObjects[0], map, gameObjects);
                enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
            }
            else if(distanceToPlayer > 800)
            {
                Console.WriteLine("dsitance>>>>  :" + distanceToPlayer);
                enemyAiMachine.Trigger(ETrigger.STOP);
            }

            //Attack(player, 20);
        }

        public void Guard(List<GameObject> gameObjects, int range, TiledMap map)
        {
            Player player = (Player)gameObjects[0];
            float distanceToGuardPosition = Vector2.Distance(originalPosition, realPositon);
            if (distanceToPlayer < range)
            {
                if (distanceToGuardPosition <= 2 * range)
                {
                    Follow(gameObjects[0], map, gameObjects);
                }
            }
            else
            {
                Patrol();
            }
            Attack(player, 20);
        }

        private float countDistanceToPlayer(Player player)
        {
            return Vector2.Distance(player.realPositon, realPositon);
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
                        GoToPoint(originalPosition);
                        distance = Vector2.Distance(realPositon, originalPosition);
                        if (distance < 5) step++;
                    }
                    else
                    {
                        distance = Vector2.Distance(realPositon, points[step]);
                        if (distance < 5)
                            step++;
                        else
                            GoToPoint(points[step]);
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
