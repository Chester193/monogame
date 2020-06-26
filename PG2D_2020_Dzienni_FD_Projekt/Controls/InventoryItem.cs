using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.Controls
{
    public class InventoryItem : Button
    {
        public int Price { get; set; }

        public InventoryItem(Texture2D texture, int price, EventHandler action)
            :base(texture, null)
        {
            Price = price;
            Click += action;
        }
    }
}
