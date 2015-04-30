using System;
using System.Linq;
using MetroDigger.Manager;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class LevelAccomplishedScreen : MenuScreen
    {
        #region Initialization

        private int _levelNo;

        public LevelAccomplishedScreen(bool isWon, int lvlNo, int score=0)
            : base("Level Accomplished")
        {
            _levelNo = lvlNo;
            string scoreText;
            scoreText = isWon ? String.Format("Your score: {0}", score) : "You have lost";
            string labelText = "Do you want to retry?";

            string quitText = isWon ? "Continue" : "Quit";

            MenuEntry retryEntry = new MenuEntry("Retry");
            MenuLabel scoreLabel = new MenuLabel(scoreText);
            MenuLabel questionLabel = new MenuLabel(labelText);
            MenuEntry exitEntry = new MenuEntry(quitText);

            retryEntry.Selected += RetrySelected;
            exitEntry.Selected += OnCancel;

            MenuObjects.Add(scoreLabel);
            MenuObjects.Add(questionLabel);
            MenuObjects.Add(retryEntry);
            MenuObjects.Add(exitEntry);

        }

        private void RetrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(_levelNo));
        }
        #endregion

    }
}
