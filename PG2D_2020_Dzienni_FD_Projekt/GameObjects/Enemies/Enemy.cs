using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using StateMachine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class Enemy : Character
    {
        private int step = 0;
        private float distanceToPlayer;
        Vector2 nextPoint = new Vector2(0);
        int patrolTimer = 50;
        Player player;

        private enum EState { IDLE, FOLLOW, ATTACK, PATROL };
        private enum ETrigger { STOP, FOLLOW_PLAYER, ATTACK, GO_PATROL };

        private Fsm<EState, ETrigger> enemyAiMachine;

        List<GameObject> gameObjects;
        TiledMap map;

        public List<SoundEffect> golemsEffects;
        public List<SoundEffect> goblinEffects;

        public Enemy()
        {
            enemyAiMachine = Fsm<EState, ETrigger>.Builder(EState.IDLE)
                .State(EState.FOLLOW)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .TransitionTo(EState.ATTACK).On(ETrigger.ATTACK)
                    .TransitionTo(EState.PATROL).On(ETrigger.GO_PATROL)
                    .Update(args =>
                    {
                        //if (this is Wolf) Console.WriteLine("FOLLOW " + distanceToPlayer);
                        Follow(map, gameObjects);
                    })
                .State(EState.IDLE)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .TransitionTo(EState.ATTACK).On(ETrigger.ATTACK)
                    .TransitionTo(EState.PATROL).On(ETrigger.GO_PATROL)
                    .Update(args =>
                    {
                        //if (this is Wolf) Console.WriteLine("IDLE " + distanceToPlayer);
                    })
                .State(EState.ATTACK)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .TransitionTo(EState.PATROL).On(ETrigger.GO_PATROL)
                    .Update(args =>
                    {
                        //if (this is Wolf) Console.WriteLine("ATTACK " + distanceToPlayer);
                        AttackPlayer();
                    })
                .State(EState.PATROL)
                    .TransitionTo(EState.IDLE).On(ETrigger.STOP)
                    .TransitionTo(EState.ATTACK).On(ETrigger.ATTACK)
                    .TransitionTo(EState.FOLLOW).On(ETrigger.FOLLOW_PLAYER)
                    .Update(args =>
                    {
                        //if (this is Wolf) Console.WriteLine("PATROL " + distanceToPlayer);
                        Patrol();
                    })
            .Build();
        }
        public override void Initialize()
        {
            golemsEffects = new List<SoundEffect>();
            goblinEffects = new List<SoundEffect>();
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            golemsEffects.Add(content.Load<SoundEffect>(@"SoundEffects/giant1"));
            golemsEffects.Add(content.Load<SoundEffect>(@"SoundEffects/giant2"));
            golemsEffects.Add(content.Load<SoundEffect>(@"SoundEffects/giant3"));
            golemsEffects.Add(content.Load<SoundEffect>(@"SoundEffects/giant4"));
            golemsEffects.Add(content.Load<SoundEffect>(@"SoundEffects/giant5"));
            goblinEffects.Add(content.Load<SoundEffect>(@"SoundEffects/goblinHurt"));
            goblinEffects.Add(content.Load<SoundEffect>(@"SoundEffects/goblinSlash"));
            goblinEffects.Add(content.Load<SoundEffect>(@"SoundEffects/goblinDie"));
            base.Load(content);
        }

        public override void Update(List<GameObject> gameObjectsG, TiledMap mapG, GameTime gameTime)
        {
            gameObjects = gameObjectsG;
            map = mapG;
            player = (Player)gameObjects[0];

            CharcterMode mode = GetMode();
            int range = GetRange();
            distanceToPlayer = countDistanceToPlayer(player);

            if (nextPoint.Equals(new Vector2(0)))
                nextPoint = originalPosition;


            if (hit) Attack(this.target, characterSettings.weaponAttack);

            if (!isDead && active)
            {
                switch (mode)
                {
                    case CharcterMode.WaitForPlayer:
                        //if (this is Viking1) Console.WriteLine("WaitForPlayer ");
                        WaitForPlayer();
                        break;
                    case CharcterMode.Guard:
                        //if (this is Viking1) Console.WriteLine("Guard ");
                        Guard(range);
                        break;
                    case CharcterMode.FollowPlayer:
                        //if (this is Viking1) Console.WriteLine("FollowPlayer ");
                        FollowPlayer();
                        break;
                    case CharcterMode.NPC:
                        Npc();
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
                    if (IsAnimationComplete && distanceToPlayer <= characterSettings.rangeOfAttack)
                    {
                        hit = true;
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
            if (player.IsDead()) enemyAiMachine.Trigger(ETrigger.STOP);
            else if (distanceToPlayer < characterSettings.rangeOfAttack)
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            else enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
        }

        public void WaitForPlayer()
        {
            //Console.WriteLine(player.IsDead());
            if (player.IsDead()) enemyAiMachine.Trigger(ETrigger.STOP);
            else if (distanceToPlayer < characterSettings.rangeOfAttack)
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            else if (distanceToPlayer < characterSettings.spotRange)
                enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
            else if (distanceToPlayer > characterSettings.range * 2)
                enemyAiMachine.Trigger(ETrigger.GO_PATROL);
        }

        public void AttackPlayer()
        {
            isAttacking = true;
            target = player;
        }

        public void Guard(int range)
        {
            float distanceToGuardPosition = Vector2.Distance(originalPosition, realPositon);
            float distancePlayerToGuardPosition = Vector2.Distance(player.position, originalPosition);

            //if (this is Wolf) Console.WriteLine(distanceToPlayer + " / " + characterSettings.spotRange);
            if (player.IsDead()) enemyAiMachine.Trigger(ETrigger.GO_PATROL);
            else if (distanceToPlayer < characterSettings.rangeOfAttack)
            {
                //if (this is Wolf) Console.WriteLine("G  attack");
                enemyAiMachine.Trigger(ETrigger.ATTACK);
            }
            else if (distanceToPlayer < characterSettings.spotRange && distanceToGuardPosition <= 2 * range && distancePlayerToGuardPosition <= 2 * range)
            {
                //if (this is Wolf) Console.WriteLine("G  follow  ");
                enemyAiMachine.Trigger(ETrigger.FOLLOW_PLAYER);
            }
            else
            {
                //if (this is Wolf) Console.WriteLine("G  patrol");
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

        public void Npc()
        {
            enemyAiMachine.Trigger(ETrigger.GO_PATROL);
        }
    }
}
