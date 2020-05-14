﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class GameHUD
    {
        SpriteFont fontArial, fontDiamond, fontCocoonian;
        Player player;
        
        public void Load(ContentManager content)
        {
            fontArial = content.Load<SpriteFont>("Fonts\\Arial");
            fontDiamond = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            fontCocoonian = content.Load<SpriteFont>("Fonts\\cocoonian");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            Vector2 v0 = new Vector2(10, 0);
            spriteBatch.DrawString(fontCocoonian , "To bedzie sumer gra!!!", v0, Color.Gold);
            Vector2 v1 = new Vector2(10, 30);
            spriteBatch.DrawString(fontDiamond, "HP: " + player.HpToString() + "/" + player.MaxHpToString() , v1, Color.Red);
            Vector2 v2 = new Vector2(10, 70);
            spriteBatch.DrawString(fontDiamond, "MP: " + player.MpToString() + "/" + player.MaxMpToString(), v2, Color.Blue);
            spriteBatch.End();
        }

        internal void Player(Player player)
        {
            this.player = player;
        }
    }
}