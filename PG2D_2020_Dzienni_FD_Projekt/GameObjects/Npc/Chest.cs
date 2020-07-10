using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.npc;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Npc
{
    class Chest : Character
    {
        public Chest(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            base.SetCharacterSettings(settings);
        }

        public override void Initialize()
        {
            scale = 0.3f;
            base.Initialize();
        }

        public override void Load(ContentManager content)
        {
            texture = TextureLoader.Load(@"characters/chest", content);

            base.Load(content);
        }
    }
}
