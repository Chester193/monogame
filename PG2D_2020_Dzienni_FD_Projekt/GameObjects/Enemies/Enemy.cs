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

        private enum EState { IDLE, FOLLOW, ATTACK, PATROL };
        private enum ETrigger { STOP, FOLLOW_PLAYER, ATTACK, GO_PATROL };

        private Fsm<EState, ETrigger> enemyAiMachine;

        List<GameObject> gameObjects;
        TiledMap map;

        public Enemy()
        {
            enemyAiMachine = Fsm<EState, ETrigger>.Builder(EState.IDLE)
                .State(EState.FOLLOW)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .TransitionTo(EState.ATTACK).On(ETrigger.ATTACK)
                    .TransitionTo(EState.PATROL).On(ETrigger.GO_PATROL)
                    .Update(args =>
                    {
                        //Console.WriteLine("FOLLOW");
                        Follow(gameObjects[0], map, gameObjects);
                    })
                .State(EState.IDLE)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .TransitionTo(EState.ATTACK).On(ETrigger.ATTACK)
                    .TransitionTo(EState.PATROL).On(ETrigger.GO_PATROL)
                    .Update(args =>
                    {
                        //Console.WriteLine("IDLE");
                    })
                .State(EState.ATTACK)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .Update(args =>
                    {
                        //Console.WriteLine("ATTACK");
                        AttackPlayer();
                    })
                .State(EState.PATROL)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .TransitionTo(EState.ATTACK).On(ETrigger.ATTACK)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .Update(args =>
                    {
                        //Console.WriteLine("PATROL");
                        Patrol();
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
                        WaitForPlayer();
                        break;
                    case CharcterMode.Guard:
                        Guard(range);
                        break;
                    case CharcterMode.FollowPlayer:
                        FollowPlayer();
                        break;

                    default:
                        WaitForPlayer();
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

        public void FollowPlayer()
        {
            if (distanceToPlayer < rangeOfAttack)
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            else
                enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
        }

        public void WaitForPlayer()
        {
            if (distanceToPlayer < rangeOfAttack)
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            else if (distanceToPlayer < 400)
                enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
            else if (distanceToPlayer > 800)
                enemyAiMachine.Trigger(ETrigger.STOP);
        }

        public void AttackPlayer()
        {
            Player player = (Player)gameObjects[0];
            Attack(player, 20);
        }

        public void Guard(int range)
        {
            float distanceToGuardPosition = Vector2.Distance(originalPosition, realPositon);
            enemyAiMachine.Trigger(ETrigger.GO_PATROL);
            if (distanceToPlayer < rangeOfAttack)
            {
                Console.WriteLine("G  attack");
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            }
            else if (distanceToPlayer < range)
            {
                if (distanceToGuardPosition <= 2 * range)
                {
                    Console.WriteLine("G  follow");
                    enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
                }
                else
                {
                    Console.WriteLine("G  patrol");
                    enemyAiMachine.Trigger(ETrigger.GO_PATROL);
                }
            }

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
                        //GoToPoint(originalPosition);
                        GoToPositon(map, gameObjects, originalPosition);
                        distance = Vector2.Distance(realPositon, originalPosition);
                        if (distance < 5) step++;
                    }
                    else
                    {
                        distance = Vector2.Distance(realPositon, points[step]);
                        if (distance < 5)
                            step++;
                        else
                            GoToPositon(map, gameObjects, points[step]);
                        if (realPositon == originalPosition)
                            step++;
                    }

                }
            }
        }

        public string DirectionToString()
        {
            string s = direction.ToString();
            return s;
        }

    }
}
