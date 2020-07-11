using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.States
{
    class ChestState : TradeState
    {
        public ChestState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Character npc)
            :base(game, graphicsDevice, content, npc)
        {
            characterName = "Chest";
            sound = content.Load<SoundEffect>(@"SoundEffects/door");
            sound.Play();
        }
        public override void Move(InventoryItem item)
        {
            if (player.Inventory.IndexOf(item) >= 0)
            {
                player.Inventory.Remove(item);
                npc.Inventory.Add(item);
            }
            else if (npc.Inventory.IndexOf(item) >= 0)
            {
                npc.Inventory.Remove(item);
                player.Inventory.Add(item);
            }
        }
    }
}
