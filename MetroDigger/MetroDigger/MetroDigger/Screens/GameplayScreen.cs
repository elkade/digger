using System;
using MetroDigger.Gameplay;
using MetroDigger.Logging;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Screens
{
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

        public GameplayScreen(Level level, bool isStarted=true) //do wczytania levelu z save'a lub kontynuacji
        {
            if (level == null)
                throw new NullReferenceException();
            SetTransition();
            _level = level;
            _level.LevelAccomplished += OnLevelAccomplished;
            _level.IsStarted = isStarted;
            MediaManager.Instance.SetDimensions(_level.Width, _level.Height);
        }

        //public GameplayScreen() //do t�a
        //{
        //    SetTransition();
        //    int lvlNo = (new Random()).Next(GameManager.Instance.GetMaxLevel());
        //    if (!GameManager.Instance.GetLevel(lvlNo, out _level))
        //        _level.LevelAccomplished += OnLevelAccomplished;
        //    MediaManager.Instance.SetDimensions(_level.Width, _level.Height);
        //}

        //public GameplayScreen(int lvlNo)
        //    //do wczytania levelu o konkretnym numerze
        //{
        //    SetTransition();
        //    if (!GameManager.Instance.GetLevel(lvlNo, out _level, true))
        //    {
        //        _level.IsStarted = true;
        //        _level.LevelAccomplished += OnLevelAccomplished;
        //    }
        //    else ; //osi�gni�to max level
        //    MediaManager.Instance.SetDimensions(_level.Width, _level.Height);
        //}

        private void OnLevelAccomplished(Level level, bool isWon) //isWon-pora�ka
        {
            try
            {
                GameManager.Instance.AddToBestScores(_level.GainedScore, _level.Number);
                if (isWon)
                    GameManager.Instance.SaveAccomplishedLevel(_level.Number, _level.TotalScore, _level.TotalLives);
                //dodajemy wynik z tego levela do rankingu
                int gainedScore = _level.GainedScore;
                //pytamy,czy ponowi�
                Level levelToRetry;
                if (!GameManager.Instance.GetLevel(_level.Number, out levelToRetry))
                {
                    levelToRetry.InitLives = _level.InitLives;
                    levelToRetry.InitScore = _level.InitScore;
                }
                else ; //wyst�pi� b��d

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

        /// <summary>
        ///     Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        ///     Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
        }

        #endregion

        #region Update and Draw

        /// <summary>
        ///     Updates the state of the game. This method checks the GameScreen.IsActive
        ///     property, so the game will stop updating when the pause menu is active,
        ///     or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f/32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f/32, 0);

            if (IsActive)
            {if(_level==null)
                ScreenManager.AddScreen(new MessageBoxScreen("Unable to load level."));

                _level.Update();
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

            if (_level != null)
                _level.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha/2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #endregion
    }
}