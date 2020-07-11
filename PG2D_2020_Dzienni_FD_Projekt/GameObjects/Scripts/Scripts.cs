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
            fastTravelPlaces.Add("Town");
            fastTravelPlaces.Add("Forest");
            fastTravelPlaces.Add("Market");
            fastTravelPlaces.Add("Piramid");
            fastTravelPlaces.Add("Ice vilage");

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
                Teleport(gameObjects[0], new Vector2(56 * tileSize, 93 * tileSize));
                hud.FastTravelStop();
            }
            if (Input.IsKeyDown(Keys.NumPad4) || Input.IsKeyDown(Keys.D4))
            {
                Teleport(gameObjects[0], new Vector2(227 * tileSize, 183 * tileSize));
                hud.FastTravelStop();
            }
            if (Input.IsKeyDown(Keys.NumPad5) || Input.IsKeyDown(Keys.D5))
            {
                Teleport(gameObjects[0], new Vector2(43 * tileSize, 222 * tileSize));
                hud.FastTravelStop();
            }
        }

        public void StartDialog()
        {
            hud.PrintMessage("Press E to talk");

            if (Input.IsKeyDown(Keys.E))
            {
                toggleTriggers(8, 7);
                game.PauseGame();
            }
        }

        public void StartDialog1()
        {
            hud.PrintMessage("Press E to talk");

            if (Input.IsKeyDown(Keys.E))
            {
                toggleTriggers(28, 27);
                game.PauseGame();
            }
        }

        public void StartDialog2()
        {
            hud.PrintMessage("Press E to talk");

            if (Input.IsKeyDown(Keys.E))
            {
                toggleTriggers(30, 29);
                game.PauseGame();
            }
        }

        public void StartDialog3()
        {
            hud.PrintMessage("Press E to talk");

            if (Input.IsKeyDown(Keys.E))
            {
                toggleTriggers(32, 31);
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
                toggleTriggers(7, 8);
                game.ContinueGame();

                Quest currentQuest;
                if (player.TryGetCurrentQuest(out currentQuest))
                {
                    currentQuest.Confirm(player);
                }
                ///Console.WriteLine("QD");
            }
        }

        public void QuestDialog1()
        {
            Player player = (Player)gameObjects[0];
            hud.PrintMessage(player.Interact());
            hud.PrintMessage2("Press Q to end");
            if (Input.IsKeyDown(Keys.Q))
            {
                hud.PrintMessage2(null);
                toggleTriggers(27, 28);
                game.ContinueGame();

                Quest currentQuest;
                if (player.TryGetCurrentQuest(out currentQuest))
                {
                    currentQuest.Confirm(player);
                }
                ///Console.WriteLine("QD");
            }
        }

        public void QuestDialog2()
        {
            Player player = (Player)gameObjects[0];
            hud.PrintMessage(player.Interact());
            hud.PrintMessage2("Press Q to end");
            if (Input.IsKeyDown(Keys.Q))
            {
                hud.PrintMessage2(null);
                toggleTriggers(29, 30);
                game.ContinueGame();

                Quest currentQuest;
                if (player.TryGetCurrentQuest(out currentQuest))
                {
                    currentQuest.Confirm(player);
                }
                ///Console.WriteLine("QD");
            }
        }

        public void QuestDialog3()
        {
            Player player = (Player)gameObjects[0];
            hud.PrintMessage(player.Interact());
            hud.PrintMessage2("Press Q to end");
            if (Input.IsKeyDown(Keys.Q))
            {
                hud.PrintMessage2(null);
                toggleTriggers(31, 32);
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
            Trade(1);
        }

        public void StartTradeDialogNo2()
        {
            Trade(2);
        }

        public void OpenChestNo1()
        {
            Chest(5);
        }

        public void OpenChestNo2()
        {
            Chest(6);
        }
        public void OpenChestNo3()
        {
            Chest(7);
        }
        public void OpenChestNo4()
        {
            Chest(8);
        }
        public void OpenChestNo5()
        {
            Chest(9);
        }
        public void OpenChestNo6()
        {
            Chest(10);
        }
        public void OpenChestNo7()
        {
            Chest(11);
        }
        public void OpenChestNo8()
        {
            Chest(12);
        }
        public void OpenChestNo9()
        {
            Chest(13);
        }
        public void OpenChestNo10()
        {
            Chest(14);
        }

        public void EnterHomeNo1()
        {
            Console.WriteLine("enter H1");
            Teleport(gameObjects[0], new Vector2(tileSize * 353, tileSize * 172));
        }

        public void ExitHomeNo1()
        {
            Console.WriteLine("exit H1");
            Teleport(gameObjects[0], new Vector2(tileSize * 239, tileSize * 78));
        }

        public void EnterHomeNo2()
        {
            Console.WriteLine("enter H2");
            Teleport(gameObjects[0], new Vector2(tileSize * 344, tileSize * 239));
        }
        public void ExitHomeNo2()
        {
            Console.WriteLine("exit H2");
            Teleport(gameObjects[0], new Vector2(tileSize * 67, tileSize * 109));
        }

        public void EnterHomeNo3()
        {
            Console.WriteLine("enter H3");
            Teleport(gameObjects[0], new Vector2(tileSize * 359, tileSize * 31));
        }
        public void ExitHomeNo3()
        {
            Console.WriteLine("exit H3");
            Teleport(gameObjects[0], new Vector2(tileSize * 66, tileSize * 79));
        }
        public void EnterCave()
        {
            Console.WriteLine("enter");
            Teleport(gameObjects[0], new Vector2(tileSize * 419, tileSize * 42));
        }
        public void ExitCave()
        {
            Console.WriteLine("exit");
            Teleport(gameObjects[0], new Vector2(tileSize * 154, tileSize * 28));
        }
    }
}
