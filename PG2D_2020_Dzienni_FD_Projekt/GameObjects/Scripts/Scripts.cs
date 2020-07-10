using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
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
        private List<Trigger> triggers;
        private GameHUD hud;
        private Game1 game;

        int tileSize = 32;

        private bool startQuestDialog = false;

        public Scripts(List<GameObject> gameObjects, List<Trigger> triggers, GameHUD GameHud, Game1 game1)
        {
            this.gameObjects = gameObjects;
            this.triggers = triggers;
            this.hud = GameHud;
            this.game = game1;
        }

        /*
         * Poniżej znajdują się meotdy do budowy skryptów
         */
        private void Teleport(GameObject what, Vector2 where)
        {
            what.position = where;
        }

        private void HealingCahracter(GameObject who)
        {
            Character character = (Character)who;
            character.Respawn();
            character.Heal();
        }

        private void Trade(int i)
        {
            hud.PrintMessage("Press E to trade");

            if (Input.IsKeyDown(Keys.E))
            {
                game.StartTrade(i);
            }
        }

        private void Chest(int i)
        {
            hud.PrintMessage("Press E to open");

            if (Input.IsKeyDown(Keys.E))
            {
                game.OpenChest(i);
            }
        }

        private void toggleTriggers(int activate, int deactivate)
        {

            triggers[activate].active = true;
            triggers[deactivate].active = false;
        }

        /*
         * Poniżej znajdują się gotowe skrypty
         * WAŻNE: skrypty NIE przyjmują parametrów
         */
        public void TeleportTo1000_1000()
        {
            Teleport(gameObjects[0], new Vector2(1000, 1000));
        }

        public void PlayerRespawn()
        {
            Teleport(gameObjects[0], gameObjects[0].startPosition);
            HealingCahracter(gameObjects[0]);
        }

        public void TeleportToLocationA()
        {
            Console.WriteLine("TeleportToLocationA");
            Teleport(gameObjects[0], new Vector2(1200, 1450));
        }

        public void TeleportToLocationB()
        {
            Console.WriteLine("TeleportToLocationB");
            Teleport(gameObjects[0], new Vector2(300, 50));
        }

        public void FastTravel()
        {
            List<string> fastTravelPlaces = new List<string>();
            fastTravelPlaces.Add("Miasto poczatkowe");
            fastTravelPlaces.Add("Wodospad");
            fastTravelPlaces.Add("Labirynt");

            hud.FastTravelStart(fastTravelPlaces);

            if (Input.IsKeyDown(Keys.NumPad1) || Input.IsKeyDown(Keys.D1))
            {
                Teleport(gameObjects[0], new Vector2(65 * tileSize, 32 * tileSize));
                hud.FastTravelStop();
            }
            if (Input.IsKeyDown(Keys.NumPad2) || Input.IsKeyDown(Keys.D2))
            {
                Teleport(gameObjects[0], new Vector2(158 * tileSize, 79 * tileSize));
                hud.FastTravelStop();
            }
            if (Input.IsKeyDown(Keys.NumPad3) || Input.IsKeyDown(Keys.D3))
            {
                Teleport(gameObjects[0], new Vector2(50 * tileSize, 96 * tileSize));
                hud.FastTravelStop();
            }
        }

        public void StartDialog()
        {
            hud.PrintMessage("Press E to talk");

            if (Input.IsKeyDown(Keys.E))
            {
                toggleTriggers(6, 5);
                game.PauseGame();
            }
        }

        public void QuestDialog()
        {
            Player player = (Player)gameObjects[0];
            hud.PrintMessage(player.Interact());
            hud.PrintMessage2("Press Q to end");
            if (Input.IsKeyDown(Keys.Q))
            {
                hud.PrintMessage2(null);
                toggleTriggers(5, 6);
                game.ContinueGame();

                Quest currentQuest;
                if (player.TryGetCurrentQuest(out currentQuest))
                {
                    currentQuest.Confirm(player);
                }
                ///Console.WriteLine("QD");
            }
        }

        public void StartTradeDialogNo1()
        {
            Trade(14);
        }

        public void OpenChestNo1()
        {
            Chest(17);
        }


    }
}
