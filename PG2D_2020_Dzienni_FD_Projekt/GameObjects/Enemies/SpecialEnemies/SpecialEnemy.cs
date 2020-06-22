using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.SpecialEnemies
{
    public class SpecialEnemy : Enemy
    {
        public Quest Quest { get; set; } = null;

        public SpecialEnemy(Vector2 startingPosition, CharacterSettings settings)
        {
            this.position = startingPosition;
            applyGravity = false;
            active = false;

            base.SetCharacterSettings(settings);
        }

        public override void Die()
        {
            base.Die();
            if (Quest != null)
            {
                Quest.Action();
            }
        }
    }
}
