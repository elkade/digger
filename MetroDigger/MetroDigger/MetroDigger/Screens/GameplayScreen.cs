using System;
using MetroDigger.Gameplay;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    internal class GameplayScreen : GameScreen
    {
        private Level _level;

        private float _pauseAlpha;

        private void SetTransition()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        #region Initialization

        public GameplayScreen(Level level) //do wczytania levelu z save'a lub kontynuacji
        {
            SetTransition();
            if (level == null)
            {
                //coœ siê popsu³o
                return;
            }
            _level = level;
            _level.IsStarted = true;
            _level.LevelAccomplished += OnLevelAccomplished;
        }

        public GameplayScreen() //do t³a
        {
            SetTransition();
            int lvlNo = (new Random()).Next(GameManager.Instance.MaxLevel);
            if (!GameManager.Instance.GetLevel(lvlNo, out _level))
            {
                if (_level == null)
                {
                    //coœ siê popsu³o
                    return;
                }
                _level.LevelAccomplished += OnLevelAccomplished;

            }
        }

        public GameplayScreen(int lvlNo)
            //do wczytania levelu o konkretnym numerze
        {
            SetTransition();
            if (!GameManager.Instance.GetLevel(lvlNo, out _level, true))
            {
                if (_level == null)
                {
                    //coœ siê popsu³o
                    return;
                }
                _level.IsStarted = true;
                _level.LevelAccomplished += OnLevelAccomplished;

            }
            else ; //osi¹gniêto max level
        }

        private void OnLevelAccomplished(Level level, bool isWon) //isWon-pora¿ka
        {
            if (isWon)
            {
                GameManager.Instance.AddToBestScores(_level.GainedScore, _level.Number);
                GameManager.Instance.SaveAccomplishedLevel(_level.Number, _level.TotalScore,_level.TotalLives);
            }
            //dodajemy wynik z tego levela do rankingu
            int gainedScore = _level.GainedScore;
            //pytamy,czy ponowiæ
            Level levelToRetry;
            if (!GameManager.Instance.GetLevel(_level.Number, out levelToRetry))
            {
                levelToRetry.InitLives = _level.InitLives;
                levelToRetry.InitScore = _level.InitScore;
            }
            else ;//wyst¹pi³ b³¹d

            Level levelToContinue;
            if (!GameManager.Instance.GetLevel(_level.Number + 1, out levelToContinue))
            {
                levelToContinue.InitLives = _level.TotalLives;
                levelToContinue.InitScore = _level.TotalScore;
            }
            else
            {
                levelToContinue = null;
                if (isWon)
                    GameManager.Instance.AddToBestScores(_level.TotalScore);
            }
            ScreenManager.AddScreen(new LevelAccomplishedScreen(isWon, levelToRetry, levelToContinue, gainedScore));
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
