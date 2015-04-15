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
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 700,
                PreferredBackBufferHeight = 700
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

            screenManager.AddScreen(new GameplayScreen());
            screenManager.AddScreen(new LogScreen());

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var mm = _mediaManager;

            mm.LoadSound("menuSong", Content.Load<SoundEffect>("DST-PrinterFriendlyVersion"), SoundType.Music);
            mm.LoadSound("laser", Content.Load<SoundEffect>("Laser Sound Effect"), SoundType.SoundEffect);
            mm.PlaySound("menuSong");

            mm.Free = Content.Load<Texture2D>("background");
            mm.Soil = Content.Load<Texture2D>("ziemia");
            mm.Rock = Content.Load<Texture2D>("skala");

            mm.PlayerIdle = Content.Load<Texture2D>("haniaIdle");
            mm.PlayerWithDrill = Content.Load<Texture2D>("haniaswider1");

            mm.DrillingPracticles.Add(Content.Load<Texture2D>("circle"));
            mm.DrillingPracticles.Add(Content.Load<Texture2D>("star"));
            mm.DrillingPracticles.Add(Content.Load<Texture2D>("diamond"));

            mm.RedBullet[0] = Content.Load<Texture2D>("redHor");
            mm.RedBullet[1] = Content.Load<Texture2D>("redVer");

            mm.MetroTunnel = Content.Load<Texture2D>("znacznik");
            mm.MetroStation = Content.Load<Texture2D>("stacja");

            mm.Font = Content.Load<SpriteFont>("menufont");

            mm.PowerCell = Content.Load<Texture2D>("bateria");
            mm.Drill = Content.Load<Texture2D>("swider");

            mm.Miner = Content.Load<Texture2D>("kopacz1");
            mm.Ranger = Content.Load<Texture2D>("zwiadowca1");
        }
    }

}
