﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG2D_2020_Dzienni_FD_Projekt.GameObjects.Scripts
{
    /*
     * Klasa zawiera metody do budowy skryptów i gotowe skrypty
     */
    public class Scripts
    {
        private List<GameObject> gameObjects;

        public Scripts(List<GameObject> gameObjects)
        {
            this.gameObjects = gameObjects;
        }

        /*
         * Poniżej znajdują się meotdy do budowy skryptów
         */
         private void Teleport(GameObject what, Vector2 where)
        {
            what.position = where;
        }

        /*
         * Poniżej znajdują się gotowe skrypty
         */
        public void TeleportTo1000_1000()
        {
            Teleport(gameObjects[0], new Vector2(1000, 1000));
        }

    }
}
