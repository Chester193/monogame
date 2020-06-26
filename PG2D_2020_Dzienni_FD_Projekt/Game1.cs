using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects;
using PG2D_2020_Dzienni_FD_Projekt.Utilities;
using System.Collections.Generic;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Scripts;
using PG2D_2020_Dzienni_FD_Projekt.States;
using PG2D_2020_Dzienni_FD_Projekt.GameObjects.Enemies.SpecialEnemies;
using PG2D_2020_Dzienni_FD_Projekt.Controls;
using Microsoft.Xna.Framework.Content;
using System;
using System.Runtime.Remoting.Messaging;

namespace PG2D_2020_Dzienni_FD_Projekt
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int vResWidth = 1280, vResHeight = 720;
        int resWidth = 1280, resHeight = 720;

        private State currentState;
        private State nextState;

        public List<GameObject> gameObjects;

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
            triggers = new List<Trigger>();

            Scripts scripts = new Scripts(gameObjects, triggers, gameHUD, this);
            scriptsList.Add(new ScriptsController(scripts.TeleportTo1000_1000));
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationA));
            scriptsList.Add(new ScriptsController(scripts.TeleportToLocationB));
            scriptsList.Add(new ScriptsController(scripts.FastTravel));
            scriptsList.Add(new ScriptsController(scripts.StartDialog));
            scriptsList.Add(new ScriptsController(scripts.QuestDialog));


            // TODO: Add your initialization logic here
            tiledMap = new TiledMap(vResWidth, vResHeight);

            CharacterSettings characterSettings = new CharacterSettings
            {
                maxHp = 100,
                mode = CharcterMode.Guard,
                range = 300,
                rangeOfAttack = 30,
                weaponAttack = 20,
            };

            List<SpecialEnemy> specialEnemies;
            List<Quest> quests = PrepareQuests(characterSettings, out specialEnemies);

            int tileSpawnPointX = 59;
            int tielSpawnPointY = 52;
            Player player = new Player(new Vector2(tileSpawnPointX * 32, tielSpawnPointY * 32), scripts, quests);
            LoadInventory(player);

            Vector2 realMapBeginning = new Vector2(tiledMap.tileSize * 31, tiledMap.tileSize * 31);
            
            gameObjects.Add(player);
            gameHUD.Player(player);

            List<Vector2> points = new List<Vector2>();
            //points.Add(new Vector2(650, 970));
            //points.Add(new Vector2(650, 1070));
            //points.Add(new Vector2(850, 1070));

            characterSettings.points = points;
            gameObjects.Add(new Zombie(new Vector2(1000, 1000), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(720, 1000), characterSettings));
            gameObjects.Add(new Lizard(new Vector2(600, 800), characterSettings));

            
            characterSettings.mode = 0;
            gameObjects.Add(new Lizard(new Vector2(400, 600), characterSettings));
            characterSettings.rangeOfAttack = 30;
            gameObjects.Add(new Zombie(new Vector2(300, 400), characterSettings));
            gameObjects.Add(new Viking1(new Vector2(300, 300), characterSettings));
            gameObjects.Add(new Viking2(new Vector2(300, 200), characterSettings));
            gameObjects.Add(new Viking3(new Vector2(300, 100), characterSettings));

            foreach(SpecialEnemy specEnemy in specialEnemies)
            {
                gameObjects.Add(specEnemy);
            }

            characterSettings.mode = CharcterMode.FollowPlayer;

            gameObjects.Add(new Demon(new Vector2(290, 000), characterSettings));

            triggers.Add(new Trigger(new Vector2(250, 0), new Vector2(200, 30), 1, scriptsList));
            triggers.Add(new Trigger(new Vector2(1100, 1570), new Vector2(200, 30), 2, scriptsList));
            triggers.Add(new Trigger(new Vector2(345, 665), new Vector2(75), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(890, 1300), new Vector2(75), 3, scriptsList));
            triggers.Add(new Trigger(new Vector2(1465, 25), new Vector2(75), 3, scriptsList));

            triggers.Add(new Trigger(new Vector2(tileSpawnPointX * 30, tileSpawnPointX * 30), new Vector2(75), 4, scriptsList));
            triggers.Add(new Trigger(new Vector2(tileSpawnPointX * 30, tileSpawnPointX * 30), new Vector2(75), 5, scriptsList, false));


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

            // TODO: use this.Content to load your game content here
            tiledMap.Load(Content, @"Map/map.tmx");

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
            if(nextState != null)
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

            Texture2D health_icon = Content.Load<Texture2D>("Other/health_potion");
            Texture2D mana_icon = Content.Load<Texture2D>("Other/mana_potion");
            EventHandler health_handler = (s, e) => 
            { 
                if(!player.IsHpFull())
                {
                    player.Heal(10);
                    inventory.Remove((InventoryItem)s);
                }
            };

            EventHandler mana_handler = (s, e) =>
            {
                if (!player.IsMpFull())
                {
                    player.ChargeMana(2);
                    inventory.Remove((InventoryItem)s);
                }
            };

                for (int i = 0; i < 3; i++)
            {
                inventory.Add(new InventoryItem(health_icon, 50, health_handler));
                inventory.Add(new InventoryItem(mana_icon, 30, mana_handler));
            }
        }
    }
}
