using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Scripts;
using PG2D_2020_Dzienni_FD_Projekt.States;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.npc;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using Microsoft.Xna.Framework.Content;
using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Npc;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Jhin;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.Orc;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Song song;

        int vResWidth = 1280, vResHeight = 720;
        int resWidth = 1280, resHeight = 720;

        private State currentState;
        private State nextState;

        public List<GameObject> gameObjects;

        public List<ShaderObject> shaderObjects;

        public List<Trigger> triggers;

        public TiledMap tiledMap;

        public GameHUD gameHUD = new GameHUD();

        public List<ScriptsController> scriptsList;

        public void ChangeState(State state)
        {
            nextState = state;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ResolutionManager.Init(ref graphics);
            ResolutionManager.SetVirtualResolution(vResWidth, vResHeight);
            ResolutionManager.SetResolution(resWidth, resHeight, false);
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            scriptsList = new List<ScriptsController>();
            gameObjects = new List<GameObject>();
            shaderObjects = new List<ShaderObject>();
            triggers = new List<Trigger>();

            shaderObjects.Add(new Portal(new Vector2(2080, 974)));
            shaderObjects.Add(new Sphere(new Vector2(1730, 2920)));

            Scripts scripts = new Scripts(gameObjects, triggers, gameHUD, this);
            scriptsList.Add(new ScriptsController(scripts.TeleportTo1000_1000));    //0
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationA));    //1
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationB));
            scriptsList.Add(new ScriptsController(scripts.FastTravel));             //3
            scriptsList.Add(new ScriptsController(scripts.StartDialog));
            scriptsList.Add(new ScriptsController(scripts.QuestDialog));
            scriptsList.Add(new ScriptsController(scripts.StartTradeDialogNo1));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo1));
            scriptsList.Add(new ScriptsController(scripts.EnterHomeNo1));           //8
            scriptsList.Add(new ScriptsController(scripts.ExitHomeNo1));            //9
            scriptsList.Add(new ScriptsController(scripts.EnterHomeNo2));
            scriptsList.Add(new ScriptsController(scripts.ExitHomeNo2));
            scriptsList.Add(new ScriptsController(scripts.EnterHomeNo3));
            scriptsList.Add(new ScriptsController(scripts.ExitHomeNo3));
            scriptsList.Add(new ScriptsController(scripts.EnterCave));              //14
            scriptsList.Add(new ScriptsController(scripts.ExitCave));               //15
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo2));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo3));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo4));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo5));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo6));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo7));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo8));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo9));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo10));
            scriptsList.Add(new ScriptsController(scripts.StartDialog1));           //25           
            scriptsList.Add(new ScriptsController(scripts.QuestDialog1));
            scriptsList.Add(new ScriptsController(scripts.StartDialog2));
            scriptsList.Add(new ScriptsController(scripts.QuestDialog2));
            scriptsList.Add(new ScriptsController(scripts.StartDialog3));
            scriptsList.Add(new ScriptsController(scripts.QuestDialog3));
            scriptsList.Add(new ScriptsController(scripts.StartTradeDialogNo2));


            // TODO: Add your initialization logic here
            tiledMap = new TiledMap(vResWidth, vResHeight);

            shaderObjects.Add(new Sphere(new Vector2(40 * tiledMap.tileSize, 224 * tiledMap.tileSize)));
            shaderObjects.Add(new Sphere(new Vector2(227 * tiledMap.tileSize, 186 * tiledMap.tileSize)));
            shaderObjects.Add(new Sphere(new Vector2(158 * tiledMap.tileSize, 78 * tiledMap.tileSize)));

            CharacterSettings characterSettings = new CharacterSettings
            {
                maxHp = 100,
                mode = CharcterMode.Guard,
                range = 300,
                rangeOfAttack = 30,
                weaponAttack = 20,
            };

            List<Character> specialEnemies;
            List<Quest> quests = PrepareQuests(characterSettings, out specialEnemies);

            int tileSpawnPointX = 59;
            int tielSpawnPointY = 49;

            Player player = new Player(new Vector2(tileSpawnPointX * 32, tielSpawnPointY * 32), scripts, quests, gameHUD);

            Vector2 realMapBeginning = new Vector2(tiledMap.tileSize * 31, tiledMap.tileSize * 31);

            gameObjects.Add(player);
            gameHUD.Player(player);

            gameObjects.Add(new NonplayableCharacter(new Vector2(360 * tiledMap.tileSize, 25 * tiledMap.tileSize), characterSettings, NPCType.sage));
            gameObjects.Add(new NonplayableCharacter(new Vector2(48 * tiledMap.tileSize, 221 * tiledMap.tileSize), characterSettings, NPCType.blacksmith));
            gameObjects.Add(new NonplayableCharacter(new Vector2(56 * tiledMap.tileSize, 125 * tiledMap.tileSize), characterSettings, NPCType.jeweler));
            gameObjects.Add(new NonplayableCharacter(new Vector2(357 * tiledMap.tileSize, 169 * tiledMap.tileSize), characterSettings, NPCType.warlord));
            gameObjects.Add(new Chest(new Vector2(76 * tiledMap.tileSize, 32 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(87 * tiledMap.tileSize, 36 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(167 * tiledMap.tileSize, 34 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(139 * tiledMap.tileSize, 69 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(255 * tiledMap.tileSize, 53 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(264 * tiledMap.tileSize, 92 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(213 * tiledMap.tileSize, 118 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(33 * tiledMap.tileSize, 161 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(189 * tiledMap.tileSize, 224 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Chest(new Vector2(114 * tiledMap.tileSize, 111 * tiledMap.tileSize), characterSettings));

            gameObjects.Add(new PortalFrame(new Vector2(65 * tiledMap.tileSize, 972)));
            gameObjects.Add(new SphereBackground(new Vector2(1730, 2920)));

            gameObjects.Add(new SphereBackground(new Vector2(40 * tiledMap.tileSize, 224 * tiledMap.tileSize)));
            gameObjects.Add(new SphereBackground(new Vector2(227 * tiledMap.tileSize, 186 * tiledMap.tileSize)));
            gameObjects.Add(new SphereBackground(new Vector2(158 * tiledMap.tileSize, 78 * tiledMap.tileSize)));

            triggers.Add(new Trigger(new Vector2(154 * tiledMap.tileSize, 27 * tiledMap.tileSize), new Vector2(32), 14, scriptsList));
            triggers.Add(new Trigger(new Vector2(418 * tiledMap.tileSize, 44 * tiledMap.tileSize), new Vector2(180, 30), 15, scriptsList));

            triggers.Add(new Trigger(new Vector2(65 * tiledMap.tileSize, 31 * tiledMap.tileSize), new Vector2(64, 48), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(157 * tiledMap.tileSize, 80 * tiledMap.tileSize), new Vector2(128), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(53 * tiledMap.tileSize, 92 * tiledMap.tileSize), new Vector2(128), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(39 * tiledMap.tileSize, 225 * tiledMap.tileSize), new Vector2(128), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(226 * tiledMap.tileSize, 187 * tiledMap.tileSize), new Vector2(128), 3, scriptsList));

            triggers.Add(new Trigger(new Vector2(360 * tiledMap.tileSize, 26 * tiledMap.tileSize), new Vector2(32), 4, scriptsList));
            triggers.Add(new Trigger(new Vector2(360 * tiledMap.tileSize, 26 * tiledMap.tileSize), new Vector2(32), 5, scriptsList, false));

            triggers.Add(new Trigger(new Vector2(36 * tiledMap.tileSize, 92 * tiledMap.tileSize), new Vector2(64, 48), 6, scriptsList));

            triggers.Add(new Trigger(new Vector2(76 * tiledMap.tileSize, 33 * tiledMap.tileSize), new Vector2(64, 32), 7, scriptsList));
            
            triggers.Add(new Trigger(new Vector2(239 * tiledMap.tileSize, 77 * tiledMap.tileSize), new Vector2(32, 16), 8, scriptsList));
            triggers.Add(new Trigger(new Vector2(354 * tiledMap.tileSize, 175 * tiledMap.tileSize), new Vector2(32), 9, scriptsList));

            triggers.Add(new Trigger(new Vector2(67 * tiledMap.tileSize, 108 * tiledMap.tileSize), new Vector2(64, 32), 10, scriptsList));
            triggers.Add(new Trigger(new Vector2(342 * tiledMap.tileSize, 239 * tiledMap.tileSize), new Vector2(64), 11, scriptsList));

            triggers.Add(new Trigger(new Vector2(66 * tiledMap.tileSize, 79 * tiledMap.tileSize), new Vector2(32, 16), 12, scriptsList));
            triggers.Add(new Trigger(new Vector2(359 * tiledMap.tileSize, 33 * tiledMap.tileSize), new Vector2(32), 13, scriptsList));

            triggers.Add(new Trigger(new Vector2(87 * tiledMap.tileSize, 37 * tiledMap.tileSize), new Vector2(64, 32), 16, scriptsList));
            triggers.Add(new Trigger(new Vector2(167 * tiledMap.tileSize, 35 * tiledMap.tileSize), new Vector2(64, 32), 17, scriptsList));
            triggers.Add(new Trigger(new Vector2(139 * tiledMap.tileSize, 70 * tiledMap.tileSize), new Vector2(64, 32), 18, scriptsList));
            triggers.Add(new Trigger(new Vector2(255 * tiledMap.tileSize, 54 * tiledMap.tileSize), new Vector2(64, 32), 19, scriptsList));
            triggers.Add(new Trigger(new Vector2(264 * tiledMap.tileSize, 93 * tiledMap.tileSize), new Vector2(64, 32), 20, scriptsList));
            triggers.Add(new Trigger(new Vector2(213 * tiledMap.tileSize, 119 * tiledMap.tileSize), new Vector2(64, 32), 21, scriptsList));
            triggers.Add(new Trigger(new Vector2(33 * tiledMap.tileSize, 162 * tiledMap.tileSize), new Vector2(64, 32), 22, scriptsList));
            triggers.Add(new Trigger(new Vector2(189 * tiledMap.tileSize, 225 * tiledMap.tileSize), new Vector2(64, 32), 23, scriptsList));
            triggers.Add(new Trigger(new Vector2(114 * tiledMap.tileSize, 112 * tiledMap.tileSize), new Vector2(64, 32), 24, scriptsList));

            triggers.Add(new Trigger(new Vector2(48 * tiledMap.tileSize, 222 * tiledMap.tileSize), new Vector2(32), 25, scriptsList));
            triggers.Add(new Trigger(new Vector2(48 * tiledMap.tileSize, 222 * tiledMap.tileSize), new Vector2(32), 26, scriptsList, false));
            triggers.Add(new Trigger(new Vector2(56 * tiledMap.tileSize, 126 * tiledMap.tileSize), new Vector2(32), 27, scriptsList));
            triggers.Add(new Trigger(new Vector2(56 * tiledMap.tileSize, 126 * tiledMap.tileSize), new Vector2(32), 28, scriptsList, false));
            triggers.Add(new Trigger(new Vector2(357 * tiledMap.tileSize, 169 * tiledMap.tileSize), new Vector2(32), 29, scriptsList));
            triggers.Add(new Trigger(new Vector2(357 * tiledMap.tileSize, 169 * tiledMap.tileSize), new Vector2(32), 30, scriptsList, false));

            triggers.Add(new Trigger(new Vector2(31 * tiledMap.tileSize, 97 * tiledMap.tileSize), new Vector2(64, 48), 31, scriptsList));


            characterSettings.maxHp = 60;
            characterSettings.weaponAttack = 10;
            gameObjects.Add(new Zombie(new Vector2(88 * tiledMap.tileSize, 38 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(92 * tiledMap.tileSize, 38 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(96 * tiledMap.tileSize, 38 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(100 * tiledMap.tileSize, 38 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(88 * tiledMap.tileSize, 43 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(92 * tiledMap.tileSize, 43 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(96 * tiledMap.tileSize, 43 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(100 * tiledMap.tileSize, 43 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 20;
            characterSettings.weaponAttack = 5;
            gameObjects.Add(new Crow(new Vector2(83 * tiledMap.tileSize, 68 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(90 * tiledMap.tileSize, 75 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(96 * tiledMap.tileSize, 62 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(103 * tiledMap.tileSize, 76 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(113 * tiledMap.tileSize, 80 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(128 * tiledMap.tileSize, 78 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(110 * tiledMap.tileSize, 62 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(127 * tiledMap.tileSize, 64 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(135 * tiledMap.tileSize, 48 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Crow(new Vector2(116 * tiledMap.tileSize, 52 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 80;
            characterSettings.weaponAttack = 25;
            gameObjects.Add(new Wolf(new Vector2(155 * tiledMap.tileSize, 29 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(173 * tiledMap.tileSize, 31 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(158 * tiledMap.tileSize, 39 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(177 * tiledMap.tileSize, 40 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(147 * tiledMap.tileSize, 45 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(177 * tiledMap.tileSize, 48 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(162 * tiledMap.tileSize, 56 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(154 * tiledMap.tileSize, 64 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(182 * tiledMap.tileSize, 54 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(144 * tiledMap.tileSize, 71 * tiledMap.tileSize), characterSettings));

            characterSettings.mode = CharcterMode.WaitForPlayer;
            gameObjects.Add(new Wolf(new Vector2(77 * tiledMap.tileSize, 40 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Wolf(new Vector2(64 * tiledMap.tileSize, 38 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 250;
            characterSettings.weaponAttack = 40;
            characterSettings.mode = CharcterMode.Guard;
            gameObjects.Add(new LavaGolem(new Vector2(140 * tiledMap.tileSize, 33 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new LavaGolem(new Vector2(85 * tiledMap.tileSize, 70 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new LavaGolem(new Vector2(124 * tiledMap.tileSize, 83 * tiledMap.tileSize), characterSettings));

            gameObjects.Add(new IceGolem(new Vector2(36 * tiledMap.tileSize, 165 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new IceGolem(new Vector2(151 * tiledMap.tileSize, 241 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new IceGolem(new Vector2(140 * tiledMap.tileSize, 220 * tiledMap.tileSize), characterSettings));

            gameObjects.Add(new EarthGolem(new Vector2(203 * tiledMap.tileSize, 220 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new EarthGolem(new Vector2(238 * tiledMap.tileSize, 211 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new EarthGolem(new Vector2(242 * tiledMap.tileSize, 173 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 60;
            characterSettings.rangeOfAttack = 200;
            gameObjects.Add(new Jhin(new Vector2(61 * tiledMap.tileSize, 167 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Jhin(new Vector2(89 * tiledMap.tileSize, 172 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Jhin(new Vector2(70 * tiledMap.tileSize, 170 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Jhin(new Vector2(91 * tiledMap.tileSize, 198 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Jhin(new Vector2(101 * tiledMap.tileSize, 203 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 80;
            gameObjects.Add(new Orc(new Vector2(253 * tiledMap.tileSize, 92 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Orc(new Vector2(222 * tiledMap.tileSize, 93 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Orc(new Vector2(241 * tiledMap.tileSize, 61 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Orc(new Vector2(233 * tiledMap.tileSize, 33 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 100;
            characterSettings.weaponAttack = 25;
            characterSettings.rangeOfAttack = 30;
            gameObjects.Add(new Goblin(new Vector2(211 * tiledMap.tileSize, 42 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Goblin(new Vector2(225 * tiledMap.tileSize, 45 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Goblin(new Vector2(249 * tiledMap.tileSize, 57 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Goblin(new Vector2(220 * tiledMap.tileSize, 67 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Goblin(new Vector2(252 * tiledMap.tileSize, 112 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Goblin(new Vector2(258 * tiledMap.tileSize, 115 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Ogre(new Vector2(247 * tiledMap.tileSize, 96 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Ogre(new Vector2(242 * tiledMap.tileSize, 83 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Ogre(new Vector2(222 * tiledMap.tileSize, 60 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Ogre(new Vector2(226 * tiledMap.tileSize, 29 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Ogre(new Vector2(237 * tiledMap.tileSize, 59 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 70;
            characterSettings.weaponAttack = 15;
            characterSettings.mode = CharcterMode.WaitForPlayer;
            gameObjects.Add(new Pirate(new Vector2(216 * tiledMap.tileSize, 122 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Pirate(new Vector2(216 * tiledMap.tileSize, 124 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Pirate(new Vector2(214 * tiledMap.tileSize, 120 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Pirate(new Vector2(217 * tiledMap.tileSize, 128 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 120;
            characterSettings.weaponAttack = 30;
            characterSettings.mode = CharcterMode.Guard;
            gameObjects.Add(new Lizard(new Vector2(200 * tiledMap.tileSize, 184 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(212 * tiledMap.tileSize, 164 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(200 * tiledMap.tileSize, 150 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(182 * tiledMap.tileSize, 171 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(290 * tiledMap.tileSize, 159 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(224 * tiledMap.tileSize, 197 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(246 * tiledMap.tileSize, 163 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 200;
            characterSettings.weaponAttack = 35;
            gameObjects.Add(new Gorilla(new Vector2(183 * tiledMap.tileSize, 211 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Gorilla(new Vector2(169 * tiledMap.tileSize, 196 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Gorilla(new Vector2(167 * tiledMap.tileSize, 219 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 50;
            characterSettings.weaponAttack = 10;
            gameObjects.Add(new GingerBandit(new Vector2(141 * tiledMap.tileSize, 99 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new GingerBandit(new Vector2(139 * tiledMap.tileSize, 105 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new GingerBandit(new Vector2(123 * tiledMap.tileSize, 90 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new GingerBandit(new Vector2(133 * tiledMap.tileSize, 90 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 80;
            characterSettings.weaponAttack = 20;
            gameObjects.Add(new Finn(new Vector2(146 * tiledMap.tileSize, 99 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Finn(new Vector2(139 * tiledMap.tileSize, 94 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Finn(new Vector2(127 * tiledMap.tileSize, 91 * tiledMap.tileSize), characterSettings));

            characterSettings.maxHp = 120;
            characterSettings.weaponAttack = 30;
            gameObjects.Add(new BigGuy(new Vector2(144 * tiledMap.tileSize, 105 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new BigGuy(new Vector2(135 * tiledMap.tileSize, 95 * tiledMap.tileSize), characterSettings));

            foreach (Character specEnemy in specialEnemies)
            {
                gameObjects.Add(specEnemy);
            }

            LoadInventory(player);

            Camera.Initialize(zoomLevel: 1.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadInitializeGameObjects(gameObjects);
            LoadInitializeTrigger(triggers);
            LoadInitializeShaderObjects(shaderObjects);

            // TODO: use this.Content to load your game content here
            tiledMap.Load(Content, @"Map/map.tmx");

            this.song = Content.Load<Song>("Music/JeffSpeed68_-_Jam_after_brunch");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;

            gameHUD.Load(Content);

            currentState = new MenuState(this, graphics.GraphicsDevice, Content, false);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
            }

            currentState.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            currentState.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
        public void PauseGame()
        {
            ChangeState(new PausedGameState(this, graphics.GraphicsDevice, Content));
        }

        public void ContinueGame()
        {
            ChangeState(new GameState(this, graphics.GraphicsDevice, Content));
        }

        public void StartTrade(int index)
        {
            ChangeState(new TradeState(this, graphics.GraphicsDevice, Content, (Character)this.gameObjects[index]));
        }

        public void OpenChest(int index)
        {
            ChangeState(new ChestState(this, graphics.GraphicsDevice, Content, (Character)this.gameObjects[index]));
        }

        public void LoadInitializeGameObjects(List<GameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Initialize();
                gameObject.Load(content: Content);
            }


            //Parallel.ForEach(gameObjects, gameObject =>
            //{
            //    gameObject.Initialize();
            //    gameObject.Load(content: Content);
            //});
        }

        public void LoadInitializeTrigger(List<Trigger> triggers)
        {
            foreach (var trigger in triggers)
            {
                trigger.Initialize();
                trigger.Load(content: Content);
            }
        }

        public void LoadInitializeShaderObjects(List<ShaderObject> shaderObjects)
        {
            foreach (var item in shaderObjects)
            {
                item.Initialize();
                item.Load(content: Content);
            }
        }

        public void Restart()
        {
            Initialize();
        }

        private List<Quest> PrepareQuests(CharacterSettings characterSettings, out List<Character> specialEnemies)
        {
            List<Quest> quests = new List<Quest>();
            specialEnemies = new List<Character>();

            //Quest 1
            List<Character> objectives = new List<Character>();
            Character specialEnemy = new Wolf(new Vector2(70 * tiledMap.tileSize, 39 * tiledMap.tileSize), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            specialEnemy = new Wolf(new Vector2(63 * tiledMap.tileSize, 35 * tiledMap.tileSize), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            specialEnemy = new Wolf(new Vector2(75 * tiledMap.tileSize, 34 * tiledMap.tileSize), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            string startDialog = "Can you get rid of this annoying wolfs from north?";
            string endDialog = "You killed it, thank you";
            string alternativeDialog = "Did you killed wolfs yet ?";
            quests.Add(new Quest(objectives, startDialog, endDialog, alternativeDialog, 100, 30));

            //Quest 2
            objectives = new List<Character>();

            specialEnemy = new EarthGolem(new Vector2(2500, 1500), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            specialEnemy = new IceGolem(new Vector2(1500, 2500), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            specialEnemy = new LavaGolem(new Vector2(3500, 1500), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            startDialog = "Kill 3 golems";
            endDialog = "You killed this beasts, thank you";
            alternativeDialog = "Did you killed golems yet ?";
            quests.Add(new Quest(objectives, startDialog, endDialog, alternativeDialog, 200, 70));

            return quests;
        }

        private void LoadInventory(Player player)
        {
            List<InventoryItem> inventory = player.Inventory;

            SpriteFont font = Content.Load<SpriteFont>("Fonts\\diamondfantasy");
            Texture2D health_icon = Content.Load<Texture2D>("Other/health_potion");
            Texture2D mana_icon = Content.Load<Texture2D>("Other/mana_potion");
            Texture2D default_sword = Content.Load<Texture2D>("Other/default_sword");
            Texture2D better_sword = Content.Load<Texture2D>("Other/better_sword");
            Texture2D fire_ball = Content.Load<Texture2D>("Other/fire_ball");
            Texture2D default_armour = Content.Load<Texture2D>("Other/default_armour");
            Texture2D better_armour = Content.Load<Texture2D>("Other/better_armour");
            Texture2D purse_icon = Content.Load<Texture2D>("InventoryItems/purse");
            Texture2D ruby_icon = Content.Load<Texture2D>("InventoryItems/ruby");
            Texture2D emerald_icon = Content.Load<Texture2D>("InventoryItems/emerald");
            Texture2D sapphire_icon = Content.Load<Texture2D>("InventoryItems/sapphire");
            Texture2D small_gems_icon = Content.Load<Texture2D>("InventoryItems/small_gems");
            Texture2D tiny_gems_icon = Content.Load<Texture2D>("InventoryItems/tiny_gems");
            Texture2D silver_bracelet_icon = Content.Load<Texture2D>("InventoryItems/silver_bracelet");
            Texture2D gold_bracelet_icon = Content.Load<Texture2D>("InventoryItems/gold_bracelet");
            Texture2D gems_bracelet_icon = Content.Load<Texture2D>("InventoryItems/gems_bracelet");
            Texture2D expensive_bracelet_icon = Content.Load<Texture2D>("InventoryItems/expensive_bracelet");
            Texture2D ruby_ring_icon = Content.Load<Texture2D>("InventoryItems/ruby_ring");
            Texture2D sapphire_ring_icon = Content.Load<Texture2D>("InventoryItems/sapphire_ring");
            Texture2D chalice_icon = Content.Load<Texture2D>("InventoryItems/chalice");
            Texture2D ruby_chalice_icon = Content.Load<Texture2D>("InventoryItems/ruby_chalice");
            Texture2D expensive_chalice_icon = Content.Load<Texture2D>("InventoryItems/expensive_chalice");
            Texture2D gold_dish_icon = Content.Load<Texture2D>("InventoryItems/gold_dish");
            Texture2D normal_dish_icon = Content.Load<Texture2D>("InventoryItems/normal_dish");
            Texture2D enchanted_sword_icon = Content.Load<Texture2D>("InventoryItems/enchanted_sword");
            Texture2D bloody_sword_icon = Content.Load<Texture2D>("InventoryItems/bloody_sword");
            Texture2D poisonous_sword_icon = Content.Load<Texture2D>("InventoryItems/poisonous_sword");
            Texture2D enchanted_dagger_icon = Content.Load<Texture2D>("InventoryItems/enchanted_dagger");
            Texture2D bloody_dagger_icon = Content.Load<Texture2D>("InventoryItems/bloody_dagger");
            Texture2D poisonous_dagger_icon = Content.Load<Texture2D>("InventoryItems/poisonous_dagger");

            SoundEffect drink = Content.Load<SoundEffect>(@"SoundEffects/potion");
            SoundEffect money = Content.Load<SoundEffect>(@"SoundEffects/coin");

            EventHandler trade_handler = (s, e) =>
            {
                if (currentState is TradeState)
                {
                    ((TradeState)currentState).Move((InventoryItem)s);
                }
            };

            EventHandler health_handler = (s, e) =>
            {
                if (!player.IsHpFull() && currentState is InventoryState)
                {
                    drink.Play();
                    player.Heal(30);
                    inventory.Remove((InventoryItem)s);
                }
            };

            EventHandler mana_handler = (s, e) =>
            {
                if (!player.IsMpFull() && currentState is InventoryState)
                {
                    drink.Play();
                    player.ChargeMana(3);
                    inventory.Remove((InventoryItem)s);
                }
            };

            EventHandler purse_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    InventoryItem sender = (InventoryItem)s;
                    money.Play();
                    player.EarnMoney(sender.Price);
                    inventory.Remove(sender);
                }
            };

            EventHandler change_weapon_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    InventoryItem sender = (InventoryItem)s;
                    int index = player.Inventory.IndexOf(sender);
                    player.Inventory[index] = player.Weapon;
                    player.Weapon = sender;
                }
            };

            EventHandler default_sword_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 10;
                }
            } + change_weapon_handler;

            EventHandler bloody_dagger_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 20;
                }
            }
            + change_weapon_handler;

            EventHandler poisonous_dagger_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 30;
                }
            }
            + change_weapon_handler;

            EventHandler enchanted_dagger_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 50;
                }
            }
            + change_weapon_handler;

            EventHandler better_sword_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 40;
                }
            } + change_weapon_handler;

            EventHandler bloody_sword_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 60;
                }
            }
            + change_weapon_handler;

            EventHandler poisonous_sword_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 70;
                }
            }
            + change_weapon_handler;

            EventHandler enchanted_sword_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 80;
                }
            }
            + change_weapon_handler;

            EventHandler fire_ball_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = true;
                }
            } + change_weapon_handler;

            EventHandler armour_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    InventoryItem sender = (InventoryItem)s;
                    int index = player.Inventory.IndexOf(sender);
                    player.Inventory[index] = player.Armour;
                    player.Armour = sender;
                    player.ChangeArmour();
                }
            };

            string defaultSwordDescription = "Deals 10 Damage";
            string bloodyDaggerDescription = "Deals 20 Damage";
            string poisonousDaggerDescription = "Deals 30 Damage";
            string enchantedDaggerDescription = "Deals 50 Damage";
            string betterSwordDescription = "Deals 40 Damage";
            string bloodySwordDescription = "Deals 60 Damage";
            string poisonousSwordDescription = "Deals 70 Damage";
            string enchantedSwordDescription = "Deals 80 Damage";
            string defaultArmourDescription = "Totally useless";
            string betterArmourDescription = "Blocks 40 percent of \n damage";
            string fireBallDescription = "Deals 25 Damage";
            string healthPotionDescription = "Heals 30 health points";
            string manaPotionDescription = "Restores 3 mana points";
            string purseDescription = "50 coins inside";
            string jeveleryDescription = "Useless but expensive";

            int defaultSwordPrice = 10;
            int bloodyDaggerPrice = 30;
            int poisonousDaggerPrice = 50;
            int enchantedDaggerPrice = 100;
            int betterSwordPrice = 70;
            int bloodySwordPrice = 200;
            int poisonousSwordPrice = 500;
            int enchantedSwordPrice = 1000;
            int defaultArmourPrice = 10;
            int betterArmourPrice = 600;
            int fireBallPrice = 350;
            int healthPotionPrice = 20;
            int manaPotionPrice = 10;
            int pursePrice = 50;

            //Player
            inventory.Add(new InventoryItem("Dagger", defaultSwordDescription, default_sword, font, defaultSwordPrice, default_sword_handler + trade_handler));
            inventory.Add(new InventoryItem("Leather armour", defaultArmourDescription, default_armour, font, defaultArmourPrice, armour_handler + trade_handler));
            inventory.Add(new InventoryItem("Warrior armour", betterArmourDescription, better_armour, font, betterArmourPrice, armour_handler + trade_handler));
            inventory.Add(new InventoryItem("Enchanted sword", enchantedSwordDescription, enchanted_sword_icon, font, enchantedSwordPrice, enchanted_sword_handler + trade_handler));
            inventory.Add(new InventoryItem("Fire ball", fireBallDescription, fire_ball, font, fireBallPrice, fire_ball_handler + trade_handler));

            for (int i = 0; i < 3; i++)
            {
                inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
                inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            }

            //Weapon Trader
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Dagger", defaultSwordDescription, default_sword, font, defaultSwordPrice, default_sword_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Bloody dagger", bloodyDaggerDescription, bloody_dagger_icon, font, bloodyDaggerPrice, bloody_dagger_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Poisonous dagger", poisonousDaggerDescription, poisonous_dagger_icon, font, poisonousDaggerPrice, poisonous_dagger_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Enchanted dagger", enchantedDaggerDescription, enchanted_dagger_icon, font, enchantedDaggerPrice, enchanted_dagger_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Sword", betterSwordDescription, better_sword, font, betterSwordPrice, better_sword_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Bloody sword", bloodySwordDescription, bloody_sword_icon, font, bloodySwordPrice, bloody_sword_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Poisonous sword", poisonousSwordDescription, poisonous_sword_icon, font, poisonousSwordPrice, poisonous_sword_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Enchanted sword", enchantedSwordDescription, enchanted_sword_icon, font, enchantedSwordPrice, enchanted_sword_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Leather armour", defaultArmourDescription, default_armour, font, defaultArmourPrice, armour_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Warrior armour", betterArmourDescription, better_armour, font, betterArmourPrice, armour_handler + trade_handler));
            ((Character)gameObjects[1]).Inventory.Add(new InventoryItem("Fire ball", fireBallDescription, fire_ball, font, fireBallPrice, fire_ball_handler + trade_handler));

            //Potion Trader
            for (int i = 0; i < 10; i++)
            {
                ((Character)gameObjects[2]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            }

            for (int i = 0; i < 10; i++)
            {
                ((Character)gameObjects[2]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            }

            //Chest 1
            ((Character)gameObjects[5]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[5]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[5]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[5]).Inventory.Add(new InventoryItem("Normal dish", jeveleryDescription, normal_dish_icon, font, 10, trade_handler));

            //Chest 2
            ((Character)gameObjects[6]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[6]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            ((Character)gameObjects[6]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[6]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[5]).Inventory.Add(new InventoryItem("Ruby", jeveleryDescription, ruby_icon, font, 50, trade_handler));

            //Chest 3
            ((Character)gameObjects[7]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[7]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[7]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[7]).Inventory.Add(new InventoryItem("Emerald", jeveleryDescription, emerald_icon, font, 70, trade_handler));

            //Chest 4
            ((Character)gameObjects[8]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            ((Character)gameObjects[8]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            ((Character)gameObjects[8]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[8]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));

            //Chest 5
            ((Character)gameObjects[9]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[9]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            ((Character)gameObjects[9]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[9]).Inventory.Add(new InventoryItem("Sapphire", jeveleryDescription, sapphire_icon, font, 80, trade_handler));
            ((Character)gameObjects[9]).Inventory.Add(new InventoryItem("Small gems", jeveleryDescription, small_gems_icon, font, 40, trade_handler));

            //Chest 6
            ((Character)gameObjects[10]).Inventory.Add(new InventoryItem("Tiny gems", jeveleryDescription, tiny_gems_icon, font, 30, trade_handler));
            ((Character)gameObjects[10]).Inventory.Add(new InventoryItem("Silver bracelet", jeveleryDescription, silver_bracelet_icon, font, 50, trade_handler));

            //Chest 7
            ((Character)gameObjects[11]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[11]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[11]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[11]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            ((Character)gameObjects[11]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            ((Character)gameObjects[11]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));

            //Chest 8

            ((Character)gameObjects[12]).Inventory.Add(new InventoryItem("Tiny gems", jeveleryDescription, tiny_gems_icon, font, 30, trade_handler));
            ((Character)gameObjects[12]).Inventory.Add(new InventoryItem("Silver bracelet", jeveleryDescription, silver_bracelet_icon, font, 50, trade_handler));
            ((Character)gameObjects[12]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));

            //Chest 9
            ((Character)gameObjects[13]).Inventory.Add(new InventoryItem("Expensive chalice", jeveleryDescription, expensive_chalice_icon, font, 80, trade_handler));
            ((Character)gameObjects[13]).Inventory.Add(new InventoryItem("Gold dish", jeveleryDescription, gold_dish_icon, font, 50, trade_handler));
            ((Character)gameObjects[13]).Inventory.Add(new InventoryItem("Ring with sapphire", jeveleryDescription, sapphire_ring_icon, font, 70, trade_handler));
            ((Character)gameObjects[13]).Inventory.Add(new InventoryItem("Gold bracelet", jeveleryDescription, gold_bracelet_icon, font, 40, trade_handler));
            ((Character)gameObjects[13]).Inventory.Add(new InventoryItem("Tiny gems", jeveleryDescription, tiny_gems_icon, font, 30, trade_handler));

            //Chest 10
            ((Character)gameObjects[14]).Inventory.Add(new InventoryItem("Silver bracelet", jeveleryDescription, silver_bracelet_icon, font, 50, trade_handler));
            ((Character)gameObjects[14]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[14]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[14]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            ((Character)gameObjects[14]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            ((Character)gameObjects[14]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));

            ////Chest 11
            //((Character)gameObjects[15]).Inventory.Add(new InventoryItem("Bracelet with gems", jeveleryDescription, gems_bracelet_icon, font, 60, trade_handler));
            //for (int i = 0; i < 5; i++)
            //{
            //    ((Character)gameObjects[15]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            //}

            ////Chest 12
            //((Character)gameObjects[16]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            //((Character)gameObjects[16]).Inventory.Add(new InventoryItem("Expensive bracelet", jeveleryDescription, expensive_bracelet_icon, font, 100, trade_handler));
            
            ////Chest 13
            //((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Ring with ruby", jeveleryDescription, ruby_ring_icon, font, 60, trade_handler));
            //((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            //((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            //for (int i = 0; i < 2; i++)
            //{
            //    ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            //}
            //for (int i = 0; i < 3; i++)
            //{
            //    ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            //}

            ////Chest 14
            //((Character)gameObjects[18]).Inventory.Add(new InventoryItem("Chalice", jeveleryDescription, chalice_icon, font, 30, trade_handler));
            //((Character)gameObjects[18]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            //for (int i = 0; i < 4; i++)
            //{
            //    ((Character)gameObjects[18]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    ((Character)gameObjects[18]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            //}

            ////Chest 15
            //((Character)gameObjects[19]).Inventory.Add(new InventoryItem("Ruby chalice", jeveleryDescription, ruby_chalice_icon, font, 50, trade_handler));
            //for (int i = 0; i < 6; i++)
            //{
            //    ((Character)gameObjects[19]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, pursePrice, purse_handler + trade_handler));
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    ((Character)gameObjects[19]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, healthPotionPrice, health_handler + trade_handler));
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    ((Character)gameObjects[19]).Inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, manaPotionPrice, mana_handler + trade_handler));
            //}
        }
    }
}
