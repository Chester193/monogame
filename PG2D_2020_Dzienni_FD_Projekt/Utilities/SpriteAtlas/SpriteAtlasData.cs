using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.Utilities.SpriteAtlas
{
    public class SpriteAtlasData
    {
        public List<string> PageNames = new List<string>();
        public List<string> AnimationNames = new List<string>();
        public Dictionary<string, List<Rectangle>> SourceRects = new Dictionary<string, List<Rectangle>>();

    }
}
