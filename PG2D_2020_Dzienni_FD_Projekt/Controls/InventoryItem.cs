using Microsoft.Xna.Framework;
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

        public InventoryItem(Texture2D texture, SpriteFont font, int price, EventHandler action)
            : base(texture, font)
        {
            Price = price;
            Click += action;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(_font, Price.ToString(), new Vector2(Rectangle.X - 10, Rectangle.Y - 10), Color.Gold, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0.1f);
        }
    }
}
