using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using System.Collections.Generic;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    public class GameHUD
    {
        SpriteFont fontArial, fontDiamond, fontCocoonian;
        Player player;
        Enemy enemy;

        bool fastTravel = false;
        List<string> fastTravelPlaces;
        int timer;

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


            if (fastTravel && timer>0)
            {
                v1 = new Vector2(500, 100);
                spriteBatch.DrawString(fontDiamond, "[Click the number]", v1, Color.White);
                v2 = new Vector2(500, 140);
                spriteBatch.DrawString(fontDiamond, "Fast travel to:", v2, Color.White);

                int i = 1;
                v2.X += 20;
                foreach (var place in fastTravelPlaces)
                {
                    v2.Y += 38;
                    spriteBatch.DrawString(fontDiamond, i + ". " +  place, v2, Color.White);
                    i++;
                }

                timer -= 1;
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

        public void FastTravelStart(List<string> fastTravelPlaces)
        {
            this.fastTravelPlaces = fastTravelPlaces;
            fastTravel = true;
            timer = 30;
        }

        public void FastTravelStop()
        {
            fastTravel = false;

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
