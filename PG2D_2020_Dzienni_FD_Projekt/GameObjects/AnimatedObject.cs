using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Animations;
using PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects
{
    public class AnimatedObject : GameObject
    {
        protected int currentAnimationFrame;
        protected int animationTimer;
        protected int currentAnimationX = -1, currentAnimationY = -1;
        protected AnimationSet animationSet = new AnimationSet();
        protected Animation currentAnimation;

        protected bool flipForLeftFrames = false;
        protected bool flipForRightFrames = false;

        protected SpriteEffects spriteEffect = SpriteEffects.None;

        protected class Animations
        {
            public static string WalkingRight = "Right - Walking";
            public static string WalkingLeft = "Left - Walking";
            public static string WalkingBack = "Back - Walking";
            public static string WalkingFront = "Front - Walking";
            public static string IdleRight = "Right - Idle";
            public static string IdleLeft = "Left - Idle";
            public static string IdleBack = "Back - Idle";
            public static string IdleFront = "Front - Idle";
            public static string SlashLeft = "Left - Slashing";
            public static string SlashRight = "Right - Slashing";
            public static string SlashBack = "Back - Slashing";
            public static string SlashFront = "Front - Slashing";
            public static string DieFront = "Front - Died";
            public static string DieBack = "Back - Died";
            public static string DieLeft = "Left - Died";
            public static string DieRight = "Right - Died";
            public static string HurtBack = "Back - Hurt";
            public static string HurtFront = "Front - Hurt";
            public static string HurtLeft = "Left - Hurt";
            public static string HurtRight = "Right - Hurt";
            public static string IceCone = "iceCone";
            public static string OrcAxe = "orcAxe";
            //public static string Dying = "Dying";
        }
        //protected enum Animations


        public bool IsAnimationComplete
        {
            get
            {
                return currentAnimationFrame >= currentAnimation.frames.Count - 1;
            }
        }

        protected void LoadAnimations(SpriteAtlasData atlasData)
        {

            int frameWidth = 0;
            int frameHeight = 0;
            List<Animation> animations = new List<Animation>();

            foreach (var animationName in atlasData.AnimationNames)
            {
                var rects = atlasData.SourceRects[animationName];

                //TODO: Change to framerate
                var anim = new Animation(name: animationName, frames: rects, animationSpeed: 5);
                animations.Add(anim);

                frameWidth = rects[0].Width;
                frameHeight = rects[0].Height;

            }

            animationSet = new AnimationSet(frameWidth, frameHeight, animations);

            center.X = animationSet.frameWidth / 2;
            center.Y = animationSet.frameHeight / 2;

            //Default animation is first on the list
            if (animationSet.animationList.Count > 0)
            {
                currentAnimation = animationSet.animationList[0];
                currentAnimationFrame = 0;
                animationTimer = 0;
            }

        }

        protected void CalculateFramePosition()
        {
            currentAnimationX = currentAnimation.frames[currentAnimationFrame].X;
            currentAnimationY = currentAnimation.frames[currentAnimationFrame].Y;
        }

        public override void Update(List<GameObject> gameObjects, TiledMap map)
        {
            base.Update(gameObjects, map);
            if (currentAnimation != null)
            {
                UpdateAnimations();
            }
        }

        protected virtual void UpdateAnimations()
        {
            if (currentAnimation?.frames.Count == 0)
                return;
            animationTimer -= 1;

            if (animationTimer < 0)
            {
                animationTimer = currentAnimation.animationSpeed;

                if (IsAnimationComplete)
                {
                    currentAnimationFrame = 0;
                }
                else
                {
                    currentAnimationFrame++;
                }

                CalculateFramePosition();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (active == false)
                return;

            if (currentAnimationX == -1 || currentAnimationY == -1)
                base.Draw(spriteBatch);
            else
            {
                spriteBatch.Draw(texture, position, new Rectangle(currentAnimationX, currentAnimationY, animationSet.frameWidth, animationSet.frameHeight), tintColor, rotation, Vector2.Zero, scale, spriteEffect, layerDepth);
                DrawBoundingBox(spriteBatch);
            }
        }

        protected virtual void ChangeAnimation(string newAnimation)
        {
            currentAnimation = GetAnimation(newAnimation);
            if (currentAnimation == null)
                return;

            currentAnimationFrame = 0;
            animationTimer = currentAnimation.animationSpeed;

            CalculateFramePosition();

            if (flipForLeftFrames == true && currentAnimation.name.Contains("Left") ||
                flipForRightFrames == true && currentAnimation.name.Contains("Right"))
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }

        }

        private Animation GetAnimation(string name)
        {
            foreach (Animation anim in animationSet.animationList)
            {
                if (anim.name == name)
                    return anim;
            }

            return null;
        }


        protected bool AnimationIsNot(string input)
        {
            //Used to check if our currentAnimation isn't set to the one passed in:
            return currentAnimation != null && input != currentAnimation.name;
        }
    }
}
