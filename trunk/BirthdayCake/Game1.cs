using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Diagnostics;

namespace BirthdayCake
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Game
    {
        Texture2D tutorial1tex;
        Texture2D tutorial2tex;

        bool press;

        public static Random RND = new Random();

        public static Dictionary<PlayerIndex, Color> PlayerColours;

        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        public static ContentManager ContentManager;

        public static Input InputManager;

        public static Player playerOne;
        public static Player playerTwo;
        public static Player playerThree;
        public static Player playerFour;

        ButtonCombo comboOne;
        ButtonCombo comboTwo;
        ButtonCombo comboThree;
        ButtonCombo comboFour;

        public static PeonManager manager;

        public static Temple temple1;
        public static Temple temple2;
        public static Temple temple3;
        public static Temple temple4;

        public static Terrain terrain;

        TutorialSystem tutorial;

        Hut hut;

        HUD HUD;

        MainMenu mainMenu;

        public static WinScreen winScreen;

        SplashScreen splash;

        public static Dictionary<PlayerIndex, Player> players;
        public static Dictionary<PlayerIndex, Temple> temples;
        public static Dictionary<PlayerIndex, ButtonCombo> buttonCombos;

        private Intro introScreen;

        public static State state = State.Splash;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            PlayerColours = new Dictionary<PlayerIndex, Color>(4);
            PlayerColours.Add(PlayerIndex.One, new Color(0, 240, 255));
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;

            graphics.ApplyChanges();

            InputManager = new Input();

            PlayerColours.Add(PlayerIndex.Two, Color.Fuchsia);
            PlayerColours.Add(PlayerIndex.Three, Color.Red);
            PlayerColours.Add(PlayerIndex.Four, new Color(154, 11, 142));

            mainMenu = new MainMenu(this);
            introScreen = new Intro(this);

            press = true;
            graphics.IsFullScreen = false;            
            
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
            // TODO: Add your initialization logic here


            Form.FromHandle(this.Window.Handle).SetBounds(0, -16, 1280, 1024);

            manager = new PeonManager();
            mainMenu.Initialize();
            introScreen.Initialize();
            base.Initialize();
        }

        protected override void BeginRun()
        {
            base.BeginRun();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ContentManager = Content;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            splash = new SplashScreen();

            winScreen = new WinScreen();

            terrain = new Terrain(this);
            hut = new Hut(new Vector2(600, 350), 50, 50, this, ContentManager.Load<Texture2D>(@"vilageSingle"));
            playerOne = new Player(PlayerIndex.One, this);
            playerOne.position = new Vector2(330, 150);
            playerTwo = new Player(PlayerIndex.Two, this);
            playerTwo.position = new Vector2(935, 844);
            playerThree = new Player(PlayerIndex.Three, this);
            playerThree.position = new Vector2(330, 844);
            playerFour = new Player(PlayerIndex.Four, this);
            playerFour.position = new Vector2(950, 150);

            players = new Dictionary<PlayerIndex, Player>();
            temples = new Dictionary<PlayerIndex, Temple>();

            players.Add(PlayerIndex.One, playerOne);
            players.Add(PlayerIndex.Two, playerTwo);
            players.Add(PlayerIndex.Three, playerThree);
            players.Add(PlayerIndex.Four, playerFour);

            temples.Add(PlayerIndex.One, new Temple(PlayerIndex.One, new Vector2(44, 33), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne));
            temples.Add(PlayerIndex.Two, new Temple(PlayerIndex.Two, new Vector2(992, 731), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerTwo));
            temples.Add(PlayerIndex.Three, new Temple(PlayerIndex.Three, new Vector2(75, 672), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne));
            temples.Add(PlayerIndex.Four, new Temple(PlayerIndex.Four, new Vector2(973, 58), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne));

            comboOne = new ButtonCombo(PlayerIndex.One);
            comboTwo = new ButtonCombo(PlayerIndex.Two);
            comboThree = new ButtonCombo(PlayerIndex.Three);
            comboFour = new ButtonCombo(PlayerIndex.Four);


            //buttonCombos.Add(PlayerIndex.One, comboOne);
            //buttonCombos.Add(PlayerIndex.Two, comboTwo);
            //buttonCombos.Add(PlayerIndex.Three, comboThree);
            //buttonCombos.Add(PlayerIndex.Four, comboFour);

            tutorial = new TutorialSystem();
            tutorial1tex = Content.Load<Texture2D>("tutOne");
            tutorial2tex = Content.Load<Texture2D>("tutTwo");

            temple1 = new Temple(PlayerIndex.One, new Vector2(44, 33), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne);
            temple2 = new Temple(PlayerIndex.Two, new Vector2(992, 731), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne);
            temple3 = new Temple(PlayerIndex.Three, new Vector2(75, 672), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne);
            temple4 = new Temple(PlayerIndex.Four, new Vector2(973, 58), ContentManager.Load<Texture2D>(@"templeGold"), ContentManager.Load<Texture2D>(@"templeGray"), playerOne);

            HUD = new HUD();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public enum State { Splash, Intro, MainMenu, Game, Tutorial1, Tutorial2 }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case State.Splash:
                    splash.Update(gameTime);
                    if (splash.finished)
                        state = State.Intro;
                    break;
                case State.Intro:
                    introScreen.Update(gameTime);
                    if (introScreen.IsDone)
                    {
                        state = State.MainMenu;
                        mainMenu.IsShowing = true;
                    }
                    break;
                case State.MainMenu:

                    // main menu switches to the game state itself
                    mainMenu.Update(gameTime);
                    if (!mainMenu.IsShowing)
                        state = State.Game;
                    break;

                case State.Tutorial1:
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !press)
                    {
                        state = State.Tutorial2;
                    }

                    press = GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A);
                    break;

                case State.Tutorial2:
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) && !press)
                    {
                        state = State.MainMenu;
                    }

                    press = GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A);
                    break;

                case State.Game:
                    winScreen.Update(gameTime);
                    if (!winScreen.IsShowing)
                    {
                        //tutorial.Update(gameTime);
                        if (!tutorial.IsShowing)
                        {
                            InputManager.Update(gameTime);

                            manager.Update(gameTime);
                            hut.Update(gameTime);

                            temples[PlayerIndex.One].Update(gameTime);
                            temples[PlayerIndex.Two].Update(gameTime);
                            temples[PlayerIndex.Three].Update(gameTime);
                            temples[PlayerIndex.Four].Update(gameTime);


                            players[PlayerIndex.One].Update(gameTime);
                            players[PlayerIndex.Two].Update(gameTime);
                            players[PlayerIndex.Three].Update(gameTime);
                            players[PlayerIndex.Four].Update(gameTime);

                            comboOne.Update(gameTime);
                            comboOne.Position = new Vector2((int)playerOne.position.X, (int)playerOne.position.Y);
                            comboTwo.Update(gameTime);
                            comboTwo.Position = new Vector2((int)playerTwo.position.X, (int)playerTwo.position.Y);
                            comboThree.Update(gameTime);
                            comboThree.Position = new Vector2((int)playerThree.position.X, (int)playerThree.position.Y);
                            comboFour.Update(gameTime);
                            comboFour.Position = new Vector2((int)playerFour.position.X, (int)playerFour.position.Y);

                            HUD.Update(gameTime);
                            terrain.Update(gameTime);

                            //tutorial.IsShowing = !tutorial.demigodDone;
                        }
                    }
                    break;
            }

            /*
            else
                mainMenu.IsShowing = true;

            if (splash.finished && mainMenu.IsShowing)
                mainMenu.Update(gameTime);
            winScreen.Update(gameTime);

            if (!winScreen.IsShowing)
            {
                if (mainMenu.IsShowing)
                {
                    //mainMenu.Update(gameTime);
                }
                else
                {
                    
                }

                
            }
            */

            //Window.Title = Mouse.GetState().X.ToString() + " " + Mouse.GetState().Y.ToString();
            Window.Title = "FPS: " + Math.Round((1000 / gameTime.ElapsedGameTime.TotalMilliseconds)).ToString() + "     peon count: " + manager.Peons.Count.ToString();

            Window.Title = Mouse.GetState().X.ToString() + " " + Mouse.GetState().Y.ToString();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (state)
            {
                        case State.Splash:
                            GraphicsDevice.Clear(Color.White);
                            splash.Draw(gameTime);
                            break;
                        case State.Intro:
                            introScreen.Draw(gameTime);
                            break;
                        case State.MainMenu:
                            mainMenu.Draw(gameTime);
                            break;
                        case State.Tutorial1:
                            spriteBatch.Draw(tutorial1tex, Vector2.Zero, Color.White);
                            break;
                        case State.Tutorial2:
                            spriteBatch.Draw(tutorial2tex, Vector2.Zero, Color.White);
                            break;
                        case State.Game:
                            terrain.Draw(gameTime);

                            temples[PlayerIndex.One].Draw();
                            temples[PlayerIndex.Two].Draw();
                            temples[PlayerIndex.Three].Draw();
                            temples[PlayerIndex.Four].Draw();

                            //tutorial.Draw();
                            mainMenu.Draw(gameTime);
                            hut.Draw(gameTime);
                            manager.Draw(gameTime);
                            HUD.Draw();

                            players[PlayerIndex.One].Draw(gameTime);
                            players[PlayerIndex.Two].Draw(gameTime);
                            players[PlayerIndex.Three].Draw(gameTime);
                            players[PlayerIndex.Four].Draw(gameTime);

                            comboOne.Draw(gameTime);
                            comboTwo.Draw(gameTime);
                            comboThree.Draw(gameTime);
                            comboFour.Draw(gameTime);

                            splash.Draw(gameTime);
                            winScreen.Draw(gameTime);
                            break;

                    
                //Window.Title = "FPS: " + Math.Round((1000 / gameTime.ElapsedGameTime.TotalMilliseconds)).ToString() + "     peon count: " + manager.Peons.Count.ToString() + "     FPS: " + Math.Round((1000 / gameTime.ElapsedGameTime.TotalMilliseconds)).ToString();
            }
            spriteBatch.End();
        }
    }
}

