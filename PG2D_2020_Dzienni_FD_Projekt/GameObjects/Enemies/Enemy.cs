using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            FollowPlayer(gameObjects);
            base.Update(gameObjects, map);
        }

        protected override void UpdateAnimations()
        {
            if (velocity != Vector2.Zero && direction.X < 0 && AnimationIsNot(Animations.WalkingLeft) && isAttacking == false)
            {
                ChangeAnimation(Animations.WalkingLeft);
            }
            else if (velocity != Vector2.Zero && direction.X > 0 && AnimationIsNot(Animations.WalkingRight) && isAttacking == false)
            {
                ChangeAnimation(Animations.WalkingRight);
            }
            base.UpdateAnimations();
        }

        private void FollowPlayer(List<GameObject> gameObjects)
        {
            GameObject player = gameObjects[0];
            Vector2 playerPosition = player.position;
            float directionX = playerPosition.X - position.X;
            float directionY = playerPosition.Y - position.Y;

            if (directionY > maxSpeed)
                MoveDown();
            else if (directionY < -maxSpeed)
                MoveUp();

            if (directionX > maxSpeed)
                MoveRight();
            else if (directionX < -maxSpeed)
                MoveLeft();


        }
    }
}
