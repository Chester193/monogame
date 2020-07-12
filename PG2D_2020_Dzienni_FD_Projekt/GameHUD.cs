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
        Texture2D moneyIcon, expIcon, background, backgroundText;
        Player player;
        Enemy enemy;

        bool fastTravel = false;
        List<string> fastTravelPlaces;
        int fastTraveTimer, messageTimer;
        string message = null;
        string message2 = null;

        private Texture2D potionEffect;
        private Texture2D Meffect_1, Meffect_2, Meffect_3;
        Effect hPotion;
        int potionEffectTimer;
        bool hudPotionEffect = false;
        bool manaPotion = false;

        public void Load(ContentManager content)
        {
            fontArial = content.Load<SpriteFont>("Fonts\\Arial");
            fontDiamond = content.Load<SpriteFont>("Fonts\\diamondfantasy");
            fontCocoonian = content.Load<SpriteFont>("Fonts\\cocoonian");
            moneyIcon = content.Load<Texture2D>("Other/money");
            expIcon = content.Load<Texture2D>("Other/exp");
            background = content.Load<Texture2D>("Other/HUD_bg");
            backgroundText = content.Load<Texture2D>("Other/HUD_text_bg");

            potionEffect = content.Load<Texture2D>("Other/mPotionEff");
            hPotion = content.Load<Effect>("VisualEffects/HPotion");
            Meffect_1 = content.Load<Texture2D>("VisualEffects/Meffect_1");
            Meffect_2 = content.Load<Texture2D>("VisualEffects/Meffect_2");
            Meffect_3 = content.Load<Texture2D>("VisualEffects/Meffect_3");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, 230, 130), Color.White);
            //spriteBatch.DrawString(fontCocoonian, "To bedzie sumer gra!!!", new Vector2(10, 0), Color.Gold);
            spriteBatch.Draw(moneyIcon, new Rectangle(20, 15, moneyIcon.Width, moneyIcon.Height), Color.Gold);
            spriteBatch.DrawString(fontDiamond, player.Money.ToString(), new Vector2(45, 15), Color.Gold, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
            spriteBatch.Draw(expIcon, new Rectangle(110, 15, expIcon.Width, expIcon.Height), Color.Azure);
            spriteBatch.DrawString(fontDiamond, player.Exp.ToString(), new Vector2(135, 15), Color.Azure, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(fontDiamond, "HP: " + player.HpToString() + "/" + player.MaxHpToString(), new Vector2(20, 40), new Color(249, 22, 29));
            spriteBatch.DrawString(fontDiamond, "MP: " + player.MpToString() + "/" + player.MaxMpToString(), new Vector2(20, 80), Color.Blue);

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

            if (message != null || message2 != null)
            {
                spriteBatch.Draw(backgroundText, new Rectangle(230, 0, backgroundText.Width, 130), Color.White);
            }

            if (message != null && messageTimer > 0)
            {
                Vector2 v1 = new Vector2(400, 5);
                spriteBatch.DrawString(fontDiamond, message, v1, Color.White);

                if (message2 != null)
                {
                    Vector2 v2 = new Vector2(800, 85);
                    spriteBatch.DrawString(fontDiamond, message2, v2, new Color(249, 22, 29));
                }

                messageTimer--;
            }
            if (messageTimer <= 0)
            {
                message = null;
                message2 = null;
            }

            spriteBatch.End();

            potionEffectTimer--;
            if (potionEffectTimer <= 0)
            {
                hudPotionEffect = false;
                manaPotion = false;
            }
            hPotion.Parameters["on"].SetValue(hudPotionEffect);
            hPotion.Parameters["mPotion"].SetValue(manaPotion);
            hPotion.Parameters["Meffect_1"].SetValue(Meffect_1);
            hPotion.Parameters["Meffect_2"].SetValue(Meffect_2);
            hPotion.Parameters["Meffect_3"].SetValue(Meffect_3);
            hPotion.Parameters["Timer"].SetValue(potionEffectTimer);

            spriteBatch.Begin(effect: hPotion);
            spriteBatch.Draw(potionEffect, new Rectangle(35, 177, 165, 164), Color.White);
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

        public void PrintMessage(string msg, int timer = 10)
        {
            message = msg;
            messageTimer = timer;
        }
        public void PrintMessage2(string msg, int timer = 10)
        {
            message2 = msg;
            messageTimer = timer;
        }

        public void ManaPotionEffect()
        {
            potionEffectTimer = 100;
            hudPotionEffect = true;
            manaPotion = true;
        }

        public void HelthPotionEffect()
        {
            potionEffectTimer = 100;
            hudPotionEffect = true;
        }
    }
}
