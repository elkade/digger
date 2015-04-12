using System;
using MetroDigger.Gameplay;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        private readonly GameOptions _gameGameOptions;

        private Level _level;

        private float pauseAlpha;

        #region Initialization


        public GameplayScreen(bool isStarted)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _level = new Level(8, 8, isStarted);
            _level.LevelAccomplished += OnLevelAccomplished;
        }

        private void OnLevelAccomplished(bool b)
        {
            if (b)
            {
                LoadingScreen.Load(GameManager, false, null, new GameplayScreen(false), new StartScreen());
            }
            _level = new Level(8,8, true);
            _level.LevelAccomplished += OnLevelAccomplished;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            GameManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                _level.Update(gameTime);
            }
        }

        public override void HandleInput(InputHandler input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.IsPauseGame())
            {
                GameManager.AddScreen(new PauseMenu2());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = GameManager.SpriteBatch;

            spriteBatch.Begin();

            _level.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                GameManager.FadeBackBufferToBlack(alpha);
            }

        }


        #endregion
    }
}
