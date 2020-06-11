﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class GameHUD
    {
        SpriteFont fontArial, fontDiamond, fontCocoonian;
        Player player;
        Enemy enemy;

        bool pause = false;
        bool gameStart = false;

        public void Load(ContentManager content)
        {
            fontArial = content.Load<SpriteFont>("Fonts\\Arial");
            fontDiamond = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            fontCocoonian = content.Load<SpriteFont>("Fonts\\cocoonian");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (gameStart)
            {
                if (!pause)
                {
                    Vector2 v0 = new Vector2(10, 0);
                    spriteBatch.DrawString(fontCocoonian, "To bedzie sumer gra!!!", v0, Color.Gold);
                    Vector2 v1 = new Vector2(10, 30);
                    spriteBatch.DrawString(fontDiamond, "HP: " + player.HpToString() + "/" + player.MaxHpToString(), v1, Color.Red);
                    Vector2 v2 = new Vector2(10, 70);
                    spriteBatch.DrawString(fontDiamond, "MP: " + player.MpToString() + "/" + player.MaxMpToString(), v2, Color.Blue);
                }
                else
                {
                    Vector2 v1 = new Vector2(500, 270);
                    spriteBatch.DrawString(fontDiamond, "GAME PAUSED", v1, Color.White);
                    v1.Y += 40;
                    spriteBatch.DrawString(fontDiamond, "GAME PAUSED", v1, Color.Black);
                    v1.Y += 40;
                    spriteBatch.DrawString(fontDiamond, "GAME PAUSED", v1, Color.Red);
                }
            }
            else
            {
                Vector2 v1 = new Vector2(300, 300);
                spriteBatch.DrawString(fontDiamond, "PRESS ENTER TO START THE GAME", v1, Color.White);
            }


            spriteBatch.End();
        }

        internal void Player(Player player)
        {
            this.player = player;
        }

        internal void Enemy(Enemy enemy)
        {
            this.enemy = enemy;
        }

        internal void TogglePause()
        {
            pause = !pause;
        }

        internal void StartGame()
        {
            gameStart = true;
        }
    }
}
