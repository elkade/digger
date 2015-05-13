using System;
using MetroDigger.Gameplay;
using MetroDigger.Logging;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Ekran rozgrywki. Wyœwietla poziom z ca³¹ zawartoœci¹
    /// </summary>
    internal class GameplayScreen : GameScreen
    {
        private readonly Level _level;

        private float _pauseAlpha;

        private void SetTransition()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            Logger.Log("GameplayScreen loaded");
        }

        #region Initialization
        /// <summary>
        /// Tworzy nowy ekran rozgrywki
        /// </summary>
        /// <param name="level">Poziom do wczytania</param>
        /// <param name="isStarted">Czy gra rozpoczyna sie natychmiast</param>
        public GameplayScreen(Level level, bool isStarted = true)
        {
            if (level == null)
                throw new NullReferenceException();
            SetTransition();
            _level = level;
            _level.LevelAccomplished += OnLevelAccomplished;
            _level.IsStarted = isStarted;
            MediaManager.Instance.SetDimensions(_level.Width, _level.Height);
        }
        /// <summary>
        /// Metoda wywo³ywana przez zdarzenie zakoñczenia gry
        /// </summary>
        /// <param name="level">ukoñczony poziom</param>
        /// <param name="isWon">czy gra zakoñczy³a siê sukcesem</param>
        private void OnLevelAccomplished(Level level, bool isWon)
        {
            try
            {
                GameManager.Instance.AddToBestScores(_level.GainedScore, _level.Number);
                if (isWon)
                    GameManager.Instance.SaveAccomplishedLevel(_level.Number, _level.TotalScore, _level.TotalLives);
                //dodajemy wynik z tego levela do rankingu
                int gainedScore = _level.GainedScore;
                //pytamy,czy ponowiæ
                Level levelToRetry;
                if (!GameManager.Instance.GetLevel(_level.Number, out levelToRetry))
                {
                    levelToRetry.InitLives = _level.InitLives;
                    levelToRetry.InitScore = _level.InitScore;
                }
                else ; //wyst¹pi³ b³¹d

                Level levelToContinue;
                if (!GameManager.Instance.GetLevel(_level.Number + 1, out levelToContinue))
                {
                    levelToContinue.InitLives = _level.TotalLives;
                    levelToContinue.InitScore = _level.TotalScore;
                }
                else
                    GameManager.Instance.AddToBestScores(_level.TotalScore);
                ScreenManager.AddScreen(new LevelAccomplishedScreen(isWon, levelToRetry, levelToContinue, gainedScore));
            }
            catch (Exception ex)
            {
                var msg = new MessageBoxScreen("Unable to load level.");
                msg.Accept += () => ScreenManager.Start(new StartScreen());
                ScreenManager.AddScreen(msg);
            }
        }

        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Aktualizuje stan poziomu
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
                if (_level == null)
                    ScreenManager.AddScreen(new MessageBoxScreen("Unable to load level."));

                if (_level != null) _level.Update();
            }
        }
        /// <summary>
        /// Obs³uguje wciœniête klawisze niedotycz¹ce logiki gry
        /// </summary>
        /// <param name="input"></param>
        public override void HandleInput(InputHandler input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (input.IsPauseGame())
            {
                GameManager.Instance.SaveToMemory(_level);
                ScreenManager.AddScreen(new PauseMenu());
            }
        }
        /// <summary>
        /// Rysuje poziom i efekty przejœcia ekranów na ekranie
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if (_level != null)
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