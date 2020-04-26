using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.Animations
{
    public class Animation
    {
        public string name;
        public List<Rectangle> frames = new List<Rectangle>();
        public int animationSpeed;

        //TODO:
        public int framerate = 0;

        public Animation()
        {

        }

        public Animation(string name, List<Rectangle> frames, int animationSpeed)
        {
            this.name = name;
            this.frames = frames;
            this.animationSpeed = animationSpeed;
        }

    }

    public class AnimationSet
    {
        public int frameWidth;
        public int frameHeight;

        public List<Animation> animationList = new List<Animation>();

        public AnimationSet()
        {

        }

        public AnimationSet(int frameWidth, int frameHeight, List<Animation> animations)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.animationList = animations;
        }

    }
}
