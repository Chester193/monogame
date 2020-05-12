﻿using Microsoft.Xna.Framework;
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
            if (direction.X < 0 && AnimationIsNot(Animations.WalkingLeft))
            {
                ChangeAnimation(Animations.WalkingLeft);
            }
            else if (direction.X > 0 && AnimationIsNot(Animations.WalkingRight))
            {
                ChangeAnimation(Animations.WalkingRight);
            }
            base.UpdateAnimations();
        }

        private void FollowPlayer(List<GameObject> gameObjects)
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
    }
}
