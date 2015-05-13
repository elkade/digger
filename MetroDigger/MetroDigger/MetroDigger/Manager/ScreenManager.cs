using System;
using System.Collections.Generic;
using MetroDigger.Gameplay;
using MetroDigger.Logging;
using MetroDigger.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Manager
{
    /// <summary>
    /// Odpowiada za obs�ug� ekran�w gry i przej�cia pomi�dzy nimi.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        readonly List<GameScreen> _screens = new List<GameScreen>();
        readonly List<GameScreen> _screensToUpdate = new List<GameScreen>();

        Texture2D _blankTexture;

        bool _isInitialized;
        /// <summary>
        /// Obiekt XNA s�u��cy do rysowania
        /// </summary>
        public SpriteBatch SpriteBatch { get; private set; }
        /// <summary>
        /// Tworzy now� instancj� managera przypisan� do bie��cego obiektu gry
        /// </summary>
        /// <param name="game">Bie��cy boiekt gry</param>
        public ScreenManager(Game game)
            : base(game)
        {
        }
        /// <summary>
        /// Inicjalizuje obiekt
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _isInitialized = true;
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            _blankTexture = content.Load<Texture2D>("blank");
        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            InputHandler.Instance.Update();

            _screensToUpdate.Clear();

            foreach (GameScreen screen in _screens)
                _screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            while (_screensToUpdate.Count > 0)
            {
                GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];

                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(InputHandler.Instance);

                        otherScreenHasFocus = true;
                    }
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

        }
        /// <summary>
        /// Rysuje aktualnie wy�wietlane ekrany
        /// </summary>
        /// <param name="gameTime">Aktualny czas gry</param>
        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (GameScreen screen in _screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }
        /// <summary>
        /// Dodaje nowy ekran na pocz�tek listy wy�wietlanych ekran�w
        /// </summary>
        /// <param name="screen"></param>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;


            _screens.Add(screen);
            Logger.Log("Screen loaded");

        }
        /// <summary>
        /// Usuwa konkretny ekran z listy wy�wietlanych ekran�w
        /// </summary>
        /// <param name="screen"></param>
        public void RemoveScreen(GameScreen screen)
        {

            _screens.Remove(screen);
            _screensToUpdate.Remove(screen);
        }
        /// <summary>
        /// Pobiera tablic� aktualnie wy�wietlanych ekran�w
        /// </summary>
        /// <returns>Tablica aktualnie wy�wietlanych ekran�w</returns>
        public GameScreen[] GetScreens()
        {
            return _screens.ToArray();
        }
        /// <summary>
        /// Powoduje zaciemnienie ekranu
        /// </summary>
        /// <param name="alpha">Wsp�czynnik zaciemnienia</param>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            SpriteBatch.Begin();

            SpriteBatch.Draw(_blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            SpriteBatch.End();
        }

        /// <summary>
        /// Usuwa ekran z pocz�tku listy i zamienia go na podany.
        /// </summary>
        /// <param name="screen">Ekran, kt�ry ma by� pierwszy na li�cie</param>
        public void SwitchScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.IsExiting = false;
            (_screens[_screens.Count - 1]).ExitScreen();
            _screens.Add(screen);
        }
        /// <summary>
        /// Dodaje do listy startowy zestaw ekran�w i inicjalizuje je.
        /// </summary>
        /// <param name="screens"></param>
        public void Start(params GameScreen[] screens)
        {
            foreach (GameScreen loadedScreen in GetScreens())
                loadedScreen.ExitScreen();
            try
            {
                Level lvl;
                int lvlNo = (new Random()).Next(GameManager.Instance.GetMaxLevel());
                GameManager.Instance.GetLevel(lvlNo, out lvl);
                AddScreen(new GameplayScreen(lvl));
            }catch{}
            foreach(var screen in screens)
                AddScreen(screen);
        }
    }
}
