using System.Collections.Generic;
using MetroDigger.Logging;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using MetroDigger.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class MetroDiggerGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly ScreenManager screenManager;
        private readonly GameOptions _gameOptions;
        private readonly MediaManager _mediaManager;

        public MetroDiggerGame()
        {
            Logger.Config("log");
            Logger.Log("start");
            Logger.Log("setting resolution");
            MediaManager.Instance.Width = 800;
            MediaManager.Instance.Height = 700;
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = MediaManager.Instance.Width,
                PreferredBackBufferHeight = MediaManager.Instance.Height
            };

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            _gameOptions = GameOptions.Instance;
            //_gameOptions.IsMusicEnabled = true;
            _gameOptions.Controls = Controls.Arrows;

            _mediaManager = MediaManager.Instance;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Logger.Log("initializing screens");
            screenManager.AddScreen(new GameplayScreen());
            screenManager.AddScreen(new LogScreen());

        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Logger.Log("loading media");
            var mm = _mediaManager;

            mm.LoadSound("menuSong", Content.Load<SoundEffect>("DST-PrinterFriendlyVersion"), SoundType.Music);
            mm.LoadSound("laser", Content.Load<SoundEffect>("Laser Sound Effect"), SoundType.SoundEffect);
            mm.PlaySound("menuSong");

            mm.LoadGraphics("Free", Content.Load<Texture2D>("background"));
            mm.LoadGraphics("Soil", Content.Load<Texture2D>("ziemia"));
            mm.LoadGraphics("Rock", Content.Load<Texture2D>("skala"));
            mm.LoadGraphics("PlayerIdle", Content.Load<Texture2D>("haniaIdle"));
            mm.LoadGraphics("PlayerWithDrill", Content.Load<Texture2D>("haniaswider1"));
            mm.DrillingPracticles = new List<Texture2D>
            {
                Content.Load<Texture2D>("circle"),
                Content.Load<Texture2D>("star"),
                Content.Load<Texture2D>("diamond")
            };
            //mm.LoadGraphics("Bullet", Content.Load<Texture2D>("redHor"));
            mm.LoadGraphics("Bullet", Content.Load<Texture2D>("redVer"));
            mm.LoadGraphics("Tunnel", Content.Load<Texture2D>("znacznik"));
            mm.LoadGraphics("Station", Content.Load<Texture2D>("stacja"));
            mm.LoadGraphics("PowerCell", Content.Load<Texture2D>("bateria"));
            mm.LoadGraphics("Drill", Content.Load<Texture2D>("swider"));
            mm.LoadGraphics("Miner", Content.Load<Texture2D>("kopacz1"));
            mm.LoadGraphics("Ranger", Content.Load<Texture2D>("zwiadowca1"));
            mm.LoadGraphics("Stone", Content.Load<Texture2D>("kamien"));
            mm.LoadGraphics("Water", Content.Load<Texture2D>("wodaEx"));

            mm.Font = Content.Load<SpriteFont>("menufont");
            mm.TopBarFont = Content.Load<SpriteFont>("gamefont");
        }
    }

}
