using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.enums;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            FollowPlayer(gameObjects);
            base.Update(gameObjects, map);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        protected override void UpdateAnimations()
        {
            if (characterXDirection == Direction.Left && AnimationIsNot(Animations.WalkingLeft))
            {
                ChangeAnimation(Animations.WalkingLeft);
            }
            if (characterXDirection == Direction.Right && AnimationIsNot(Animations.WalkingRight))
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

            if (directionX > maxSpeed)
                MoveRight();
            else if (directionX < -maxSpeed)
                MoveLeft();

            if (directionY > maxSpeed)
                MoveDown();
            else if (directionY < -maxSpeed)
                MoveUp();
        }
    }
}
