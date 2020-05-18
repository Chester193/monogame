using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    class Enemy : Character
    {

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            if(path.Count == 0 || GoToPoint(path[0]))
            {
                path = PathFinder.FindPath(map, new Vector2(BoundingBox.X, BoundingBox.Y), new Vector2(gameObjects[0].BoundingBox.X, gameObjects[0].BoundingBox.Y));
            }
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
    }
}
