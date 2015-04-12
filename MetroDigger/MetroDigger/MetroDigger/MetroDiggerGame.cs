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
    /// stuff happens in the GameManager component.
    /// </summary>
    public class MetroDiggerGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly GameManager _gameManager;
        private readonly SoundManager _soundManager;
        private readonly GameOptions _gameOptions;
        private readonly GraphicResourceContainer _graphicResourceContainer;

        public MetroDiggerGame()
        {
            Content.RootDirectory = "Content";

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 700,
                PreferredBackBufferHeight = 700
            };

            _gameManager = new GameManager(this);
            Components.Add(_gameManager);

            _gameOptions = GameOptions.Instance;
            //_gameOptions.IsMusicEnabled = true;
            _gameOptions.Controls = Controls.Arrows;

            _graphicResourceContainer = GraphicResourceContainer.Instance;

            _soundManager = SoundManager.Instance;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _gameManager.AddScreen(new GameplayScreen(false));
            _gameManager.AddScreen(new LogScreen());

        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var grc = _graphicResourceContainer;

            _soundManager.LoadSound("menuSong", Content.Load<SoundEffect>("DST-PrinterFriendlyVersion"), SoundType.Music);
            _soundManager.LoadSound("laser", Content.Load<SoundEffect>("Laser Sound Effect"), SoundType.SoundEffect);
            _soundManager.PlaySound("menuSong");

            grc.Free = Content.Load<Texture2D>("background");
            grc.Soil = Content.Load<Texture2D>("ziemia");
            grc.Rock = Content.Load<Texture2D>("skala");

            grc.PlayerIdle = Content.Load<Texture2D>("haniaIdle");
            grc.PlayerWithDrill = Content.Load<Texture2D>("haniaswider1");

            grc.DrillingPracticles.Add(Content.Load<Texture2D>("circle"));
            grc.DrillingPracticles.Add(Content.Load<Texture2D>("star"));
            grc.DrillingPracticles.Add(Content.Load<Texture2D>("diamond"));

            grc.RedBullet[0] = Content.Load<Texture2D>("redHor");
            grc.RedBullet[1] = Content.Load<Texture2D>("redVer");

            grc.MetroTunnel = Content.Load<Texture2D>("znacznik");
            grc.MetroStation = Content.Load<Texture2D>("stacja");

            grc.Font = Content.Load<SpriteFont>("menufont");

            grc.PowerCell = Content.Load<Texture2D>("bateria");
            grc.Drill = Content.Load<Texture2D>("swider");

            grc.Miner = Content.Load<Texture2D>("kopacz1");
            grc.Ranger = Content.Load<Texture2D>("zwiadowca1");
        }
    }

}
