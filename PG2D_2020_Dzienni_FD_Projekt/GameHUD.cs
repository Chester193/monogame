using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class GameHUD
    {
        SpriteFont fontArial, fontDiamond, fontCocoonian;
        
        public void Load(ContentManager content)
        {
            fontArial = content.Load<SpriteFont>("Fonts\\Arial");
            fontDiamond = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            fontCocoonian = content.Load<SpriteFont>("Fonts\\cocoonian");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(fontCocoonian , "Zaraz dostaniesz wpierdol!!!", Vector2.Zero, Color.Gold);
            Vector2 v = new Vector2(10, 30);
            spriteBatch.DrawString(fontDiamond, "HP: " + GameObjects.Player.hp.ToString() + "/" + GameObjects.Player.maxHp.ToString() , v, Color.Red);
            Vector2 v2 = new Vector2(10, 70);
            spriteBatch.DrawString(fontDiamond, "MP: " + GameObjects.Player.mp.ToString() + "/" + GameObjects.Player.maxMp.ToString(), v2, Color.Blue);
            spriteBatch.End();
        }
    }
}
