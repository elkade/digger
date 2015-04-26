using System;
using MetroDigger.Gameplay;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    class GameplayScreen : GameScreen
    {
        private Level _level;

        private float _pauseAlpha;

        #region Initialization

        public GameplayScreen(Level level)//do wczytania levelu z save'a
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _level = level;
            _level.IsStarted = true;
            _level.LevelAccomplished += OnLevelAccomplished;
        }

        public GameplayScreen()//do t³a
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            int lvlNo = (new Random()).Next(GameManager.Instance.MaxLevel);
            if (!GameManager.Instance.GetLevel(lvlNo, out _level))
            {
                _level.LevelAccomplished += OnLevelAccomplished;

            }
        }

        public GameplayScreen(int lvlNo)//do wczytania levelu o konkretnym numerze
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            if (!GameManager.Instance.GetLevel(lvlNo, out _level))
            {
                _level.IsStarted = true;
                _level.LevelAccomplished += OnLevelAccomplished;

            }
            else ;//osi¹gniêto max level
        }

        private void OnLevelAccomplished(Level level, bool b)
        {
            if (!GameManager.Instance.NextLevel(ref _level))
            {
                _level.IsStarted = true;
                _level.LevelAccomplished += OnLevelAccomplished;
            }
            else
            {
                _level.IsStarted = false;
                GameManager.Instance.AddToBestScores(_level.Player.Score);
                GameManager.Instance.SaveAccomplishedLevel(_level.Number,_level.Player.Score,_level.Player.LivesCount);
                LoadingScreen.Load(ScreenManager, false, new GameplayScreen(), new StartScreen(), new RankingScreen(_level.Player.Score));
            }
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
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
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

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
                GameManager.Instance.SaveToMemory(_level);
                ScreenManager.AddScreen(new PauseMenu2());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if(_level!=null)
                _level.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }

        }


        #endregion
    }
}
