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
        Texture2D moneyIcon, expIcon;
        Player player;
        Enemy enemy;

        bool fastTravel = false;
        List<string> fastTravelPlaces;
        int fastTraveTimer, messageTimer;
        string message = null;
        string message2 = null;

        public void Load(ContentManager content)
        {
            fontArial = content.Load<SpriteFont>("Fonts\\Arial");
            fontDiamond = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            fontCocoonian = content.Load<SpriteFont>("Fonts\\cocoonian");
            moneyIcon = content.Load<Texture2D>("Other/money");
            expIcon = content.Load<Texture2D>("Other/exp");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            //spriteBatch.DrawString(fontCocoonian, "To bedzie sumer gra!!!", new Vector2(10, 0), Color.Gold);
            spriteBatch.Draw(moneyIcon, new Rectangle(10, 0, moneyIcon.Width, moneyIcon.Height), Color.Gold);
            spriteBatch.DrawString(fontDiamond, player.Money.ToString(), new Vector2(35, 0), Color.Gold, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(expIcon, new Rectangle(100, 0, expIcon.Width, expIcon.Height), Color.Silver);
            spriteBatch.DrawString(fontDiamond, player.Exp.ToString(), new Vector2(125, 0), Color.Silver, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(fontDiamond, "HP: " + player.HpToString() + "/" + player.MaxHpToString(), new Vector2(10, 25), Color.Red);
            spriteBatch.DrawString(fontDiamond, "MP: " + player.MpToString() + "/" + player.MaxMpToString(), new Vector2(10, 65), Color.Blue);

            if (fastTravel && fastTraveTimer > 0)
            {
                Vector2 v1 = new Vector2(500, 100);
                spriteBatch.DrawString(fontDiamond, "[Click the number]", v1, Color.White);
                Vector2 v2 = new Vector2(500, 140);
                spriteBatch.DrawString(fontDiamond, "Fast travel to:", v2, Color.White);

                int i = 1;
                v2.X += 20;
                foreach (var place in fastTravelPlaces)
                {
                    v2.Y += 38;
                    spriteBatch.DrawString(fontDiamond, i + ". " + place, v2, Color.White);
                    i++;
                }

                fastTraveTimer -= 1;
            }

            if (message != null && messageTimer > 0)
            {
                Vector2 v1 = new Vector2(500, 100);
                spriteBatch.DrawString(fontDiamond, message, v1, Color.White);

                if (message2 != null)
                {
                    Vector2 v2 = new Vector2(500, 300);
                    spriteBatch.DrawString(fontDiamond, message2, v2, Color.White);
                }

                messageTimer--;
            }
            if (messageTimer <= 0)
            {
                message = null;
                message2 = null;
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
            fastTraveTimer = 30;
        }

        public void FastTravelStop()
        {
            fastTravel = false;
        }

        public void PrintMessage(string msg)
        {
            message = msg;
            messageTimer = 10;
        }
        public void PrintMessage2(string msg)
        {
            message2 = msg;
            messageTimer = 10;
        }
    }
}
