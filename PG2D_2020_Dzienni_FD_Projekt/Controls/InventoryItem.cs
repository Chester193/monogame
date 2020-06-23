using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.Controls
{
    class InventoryItem : Button
    {
        public int Value { get; set; }

        public InventoryItem(Texture2D texture, int value, EventHandler action)
            :base(texture, null)
        {
            Value = value;
            Click += action;
        }
    }
}
