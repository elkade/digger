using System;
using MetroDigger.Gameplay;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class LevelAccomplishedScreen : MenuScreen
    {
        private readonly Level _levelToRetry;
        private readonly Level _levelToContinue;
        private readonly int _gainedScore;

        #region Initialization

        public LevelAccomplishedScreen(bool isWon, Level levelToRetry, Level levelToContinue, int gainedScore=0)
            : base("Level Accomplished")
        {
            _levelToRetry = levelToRetry;
            _levelToContinue = levelToContinue;
            _gainedScore = gainedScore;
            string scoreText;
            scoreText = isWon ? String.Format("Your score: {0}", levelToRetry.TotalScore) : "You have lost";
            string labelText = "Do you want to retry?";

            string quitText = isWon ? "Continue" : "Quit";

            MenuEntry retryEntry = new MenuEntry("Retry");
            MenuLabel scoreLabel = new MenuLabel(scoreText);
            MenuLabel questionLabel = new MenuLabel(labelText);
            MenuEntry exitEntry = new MenuEntry(quitText);

            retryEntry.Selected += RetrySelected;
            if (!isWon)
                exitEntry.Selected += OnCancel;
            else
            {
                if(levelToContinue==null)
                    exitEntry.Selected += RankingSelected;
                else
                    exitEntry.Selected += ContinueSelected;
            }

            MenuObjects.Add(scoreLabel);
            MenuObjects.Add(questionLabel);
            MenuObjects.Add(retryEntry);
            MenuObjects.Add(exitEntry);

        }

        private void RetrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true ,new GameplayScreen(_levelToRetry));
        }

        private void RankingSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(), new StartScreen(), new RankingScreen(_levelToRetry.TotalScore + _gainedScore));
        }

        private void ContinueSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(_levelToContinue));
        }

        protected override void OnCancel()
        {
            LoadingScreen.Load(ScreenManager, false, new GameplayScreen(),new StartScreen());
        }

        #endregion

    }
}
