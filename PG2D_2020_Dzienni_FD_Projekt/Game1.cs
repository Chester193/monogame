﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Scripts;
using PG2D_2020_Dzienni_FD_Projekt.States;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.SpecialEnemies;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.npc;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using Microsoft.Xna.Framework.Content;
using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Npc;

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
            scriptsList.Add(new ScriptsController(scripts.TeleportTo1000_1000));
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationA));
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationB));
            scriptsList.Add(new ScriptsController(scripts.FastTravel));
            scriptsList.Add(new ScriptsController(scripts.StartDialog));
            scriptsList.Add(new ScriptsController(scripts.QuestDialog));
            scriptsList.Add(new ScriptsController(scripts.StartTradeDialogNo1));
            scriptsList.Add(new ScriptsController(scripts.OpenChestNo1));


            // TODO: Add your initialization logic here
            tiledMap = new TiledMap(vResWidth, vResHeight);

            CharacterSettings characterSettings = new CharacterSettings
            {
                maxHp = 100,
                mode = CharcterMode.WaitForPlayer,
                range = 300,
                rangeOfAttack = 30,
                weaponAttack = 20,
            };

            List<SpecialEnemy> specialEnemies;
            List<Quest> quests = PrepareQuests(characterSettings, out specialEnemies);

            int tileSpawnPointX = 59;
            int tielSpawnPointY = 52;

            Player player = new Player(new Vector2(tileSpawnPointX * 32, tielSpawnPointY * 32), scripts, quests, gameHUD);

            Vector2 realMapBeginning = new Vector2(tiledMap.tileSize * 31, tiledMap.tileSize * 31);

            gameObjects.Add(player);
            gameHUD.Player(player);

            characterSettings.mode = CharcterMode.Guard;
            characterSettings.rangeOfAttack = 30;
            gameObjects.Add(new Zombie(new Vector2(104 * tiledMap.tileSize, 38 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(163 * tiledMap.tileSize, 56 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(172 * tiledMap.tileSize, 59 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(165 * tiledMap.tileSize, 77 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Zombie(new Vector2(101 * tiledMap.tileSize, 43 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Viking1(new Vector2(59 * tiledMap.tileSize, 92 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Viking2(new Vector2(59 * tiledMap.tileSize, 93 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new Viking3(new Vector2(61 * tiledMap.tileSize, 91 * tiledMap.tileSize), characterSettings));
            gameObjects.Add(new PortalFrame(new Vector2(65 * tiledMap.tileSize, 972)));
            gameObjects.Add(new SphereBackground(new Vector2(1730, 2920)));

            foreach (SpecialEnemy specEnemy in specialEnemies)
            {
                gameObjects.Add(specEnemy);
            }

            characterSettings.mode = CharcterMode.FollowPlayer;
            gameObjects.Add(new Demon(new Vector2(110 * tiledMap.tileSize, 58 * tiledMap.tileSize), characterSettings));

            triggers.Add(new Trigger(new Vector2(240 * tiledMap.tileSize, 30 * tiledMap.tileSize), new Vector2(200, 30), 1, scriptsList));
            triggers.Add(new Trigger(new Vector2(246 * tiledMap.tileSize, 30 * tiledMap.tileSize), new Vector2(200, 30), 2, scriptsList));
            triggers.Add(new Trigger(new Vector2(65 * tiledMap.tileSize, 31 * tiledMap.tileSize), new Vector2(75), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(158 * tiledMap.tileSize, 78 * tiledMap.tileSize), new Vector2(75), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(50 * tiledMap.tileSize, 95 * tiledMap.tileSize), new Vector2(75), 3, scriptsList));

            triggers.Add(new Trigger(new Vector2(54 * tiledMap.tileSize, 56 * tiledMap.tileSize), new Vector2(75), 4, scriptsList));
            triggers.Add(new Trigger(new Vector2(54 * tiledMap.tileSize, 56 * tiledMap.tileSize), new Vector2(75), 5, scriptsList, false));

            triggers.Add(new Trigger(new Vector2(52 * tiledMap.tileSize), new Vector2(75), 6, scriptsList));

            triggers.Add(new Trigger(new Vector2(60 * tiledMap.tileSize, 51 * tiledMap.tileSize), new Vector2(75), 7, scriptsList));

            characterSettings.mode = CharcterMode.WaitForPlayer;

            gameObjects.Add(new NonplayableCharacter(new Vector2(55 * tiledMap.tileSize, 54 * tiledMap.tileSize), characterSettings, NPCType.sage));
            gameObjects.Add(new Chest(new Vector2(60 * tiledMap.tileSize, 51 * tiledMap.tileSize), characterSettings));
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

        private List<Quest> PrepareQuests(CharacterSettings characterSettings, out List<SpecialEnemy> specialEnemies)
        {
            List<Quest> quests = new List<Quest>();
            specialEnemies = new List<SpecialEnemy>();

            //Quest 1
            List<SpecialEnemy> objectives = new List<SpecialEnemy>();
            SpecialEnemy specialEnemy = new Wolf(new Vector2(1500, 1500), characterSettings);
            objectives.Add(specialEnemy);
            specialEnemies.Add(specialEnemy);

            string startDialog = "Hi, can you kill one wolf for me ? \n It always came from North";
            string endDialog = "You killed this beast, thank you";
            string alternativeDialog = "Did you killed wolf yet ?";
            quests.Add(new Quest(objectives, startDialog, endDialog, alternativeDialog, 100));

            //Quest 2
            objectives = new List<SpecialEnemy>();

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
            quests.Add(new Quest(objectives, startDialog, endDialog, alternativeDialog, 200));

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
                    player.Heal(10);
                    inventory.Remove((InventoryItem)s);
                }
            };

            EventHandler mana_handler = (s, e) =>
            {
                if (!player.IsMpFull() && currentState is InventoryState)
                {
                    drink.Play();
                    player.ChargeMana(2);
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

            EventHandler better_sword_handler = (s, e) =>
            {
                if (currentState is InventoryState)
                {
                    player.isRanged = false;
                    player.characterSettings.weaponAttack = 50;
                }
            } + change_weapon_handler;

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
            string betterSwordDescription = "Deals 50 Damage";
            string defaultArmourDescription = "Totally useless";
            string betterArmourDescription = "Blocks 40 percent of \n damage";
            string fireBallDescription = "Deals ?? Damage";
            string healthPotionDescription = "Heals 10 health points";
            string manaPotionDescription = "Restores 2 mana points";
            string purseDescription = "20 coins inside";
            string jeveleryDescription = "Useless but expensive";

            inventory.Add(new InventoryItem("Short sword", defaultSwordDescription, default_sword, font, 10, default_sword_handler + trade_handler));
            inventory.Add(new InventoryItem("Leather armour", defaultArmourDescription, default_armour, font, 10, armour_handler + trade_handler));
            inventory.Add(new InventoryItem("Warrior armour", betterArmourDescription, better_armour, font, 120, armour_handler + trade_handler));
            inventory.Add(new InventoryItem("Ninja sword", betterSwordDescription, better_sword, font, 100, better_sword_handler + trade_handler));
            inventory.Add(new InventoryItem("Fire ball", fireBallDescription, fire_ball, font, 80, fire_ball_handler + trade_handler));

            for (int i = 0; i < 3; i++)
            {
                inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, 10, health_handler + trade_handler));
                inventory.Add(new InventoryItem("Mana potion", manaPotionDescription, mana_icon, font, 5, mana_handler + trade_handler));
            }
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Health potion", healthPotionDescription, health_icon, font, 10, health_handler + trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Purse", purseDescription, purse_icon, font, 20, purse_handler + trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Ruby", jeveleryDescription, ruby_icon, font, 50, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Emerald", jeveleryDescription, emerald_icon, font, 70, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Sapphire", jeveleryDescription, sapphire_icon, font, 80, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Small gems", jeveleryDescription, small_gems_icon, font, 40, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Tiny gems", jeveleryDescription, tiny_gems_icon, font, 30, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Silver bracelet", jeveleryDescription, silver_bracelet_icon, font, 20, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Gold bracelet", jeveleryDescription, gold_bracelet_icon, font, 40, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Bracelet with gems", jeveleryDescription, gems_bracelet_icon, font, 60, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Expensive bracelet", jeveleryDescription, expensive_bracelet_icon, font, 100, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Ring with ruby", jeveleryDescription, ruby_ring_icon, font, 60, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Ring with sapphire", jeveleryDescription, sapphire_ring_icon, font, 70, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Chalice", jeveleryDescription, chalice_icon, font, 30, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Ruby chalice", jeveleryDescription, ruby_chalice_icon, font, 50, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Expensive chalice", jeveleryDescription, expensive_chalice_icon, font, 80, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Gold dish", jeveleryDescription, gold_dish_icon, font, 20, trade_handler));
            ((Character)gameObjects[17]).Inventory.Add(new InventoryItem("Normal dish", jeveleryDescription, normal_dish_icon, font, 5, trade_handler));
        }
    }
}
