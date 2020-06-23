using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using StateMachine;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class Enemy : Character
    {
        private int step = 0;
        private float distanceToPlayer;
        Vector2 nextPoint = new Vector2(0);
        int patrolTimer = 50;

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
                        Follow(map, gameObjects);
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

            if (nextPoint.Equals(new Vector2(0)))
                nextPoint = originalPosition;

            if (!isDead && active)
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
                    isCollidable = false;
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
                    if (IsAnimationComplete || distanceToPlayer >= characterSettings.rangeOfAttack)
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
            if (distanceToPlayer < characterSettings.rangeOfAttack)
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            else
                enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
        }


        public void WaitForPlayer()
        {
            if (distanceToPlayer < characterSettings.rangeOfAttack)
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
            if (distanceToPlayer < characterSettings.rangeOfAttack)
            {
                //Console.WriteLine("G  attack");
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            }
            else if (distanceToPlayer < range)
            {
                if (distanceToGuardPosition <= 2 * range)
                {
                    //Console.WriteLine("G  follow");
                    enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
                }
            }
            else
            {
                //Console.WriteLine("G  patrol");
                enemyAiMachine.Trigger(ETrigger.GO_PATROL);
            }
        }

        private float countDistanceToPlayer(Player player)
        {
            return Vector2.Distance(player.realPositon, realPositon);
        }

        public void Patrol()
        {
            if (step == 0)
            {
                nextPoint = realPositon;
                step++;
            }

            if (step > 4)
            {
                nextPoint = originalPosition;
                step = 0;
            }

            float distance = Vector2.Distance(realPositon, nextPoint);
            if (patrolTimer <= 0 || patrolTimer > 150)
            {
                nextPoint = RandomPoint(100);
                step++;
                patrolTimer = 50;
            }
            if (distance < 15)
            {
                patrolTimer--;
            }
            else
            {
                GoToPositon(map, gameObjects, nextPoint);
                patrolTimer++;
            }
        }

        public string DirectionToString()
        {
            string s = direction.ToString();
            return s;
        }

        private Vector2 RandomPoint(int range)
        {
            Vector2 rPoint = new Vector2(originalPosition.X, originalPosition.Y);
            //Console.WriteLine("RP: " + rPoint + " org: " + originalPosition);
            var rand = new Random();
            if (rand.NextDouble() < 0.5)
                rPoint.X += rand.Next(range / 2, range);
            else
                rPoint.X -= rand.Next(range / 2, range);

            if (rand.NextDouble() < 0.5)
                rPoint.Y += rand.Next(range / 2, range);
            else
                rPoint.Y -= rand.Next(range / 2, range);
            //Console.WriteLine("RP: " + rPoint);

            return rPoint;
        }
    }
}
