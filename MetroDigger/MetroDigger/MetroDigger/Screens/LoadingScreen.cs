using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Ekran ³adowania. S³u¿y jako przerywnik w przechodzeniu pomiêdzy kluczowymi ekranami w grze.
    /// </summary>
    class LoadingScreen : GameScreen
    {
        readonly bool _loadingIsSlow;
        bool _otherScreensAreGone;

        readonly GameScreen[] _screensToLoad;

        /// <summary>
        /// Tworzy nowy ekran ³adowania. Konstruktor wywo³ywany ze statycznej metody Load
        /// </summary>
        private LoadingScreen(bool loadingIsSlow, GameScreen[] screensToLoad)
        {
            _loadingIsSlow = loadingIsSlow;
            _screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Aktywuje ekran ³adowania.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, params GameScreen[] screensToLoad)
        {
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            LoadingScreen loadingScreen = new LoadingScreen(loadingIsSlow,
                                                            screensToLoad);

            screenManager.AddScreen(loadingScreen);
        }




        /// <summary>
        /// Aktualizuje ekran ³adowania
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (_otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in _screensToLoad)
                    if (screen != null)
                        ScreenManager.AddScreen(screen);
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Odrysowuje ekran ³adowania
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                _otherScreensAreGone = true;
            }

            if (_loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = MediaManager.Instance.Font;

                const string message = "Loading...";

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }
    }
}
