using System;
using MetroDigger.Gameplay;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Menu ukończonego poziomu.
    /// Pozwala wybrać opcję powtórzenia poziomu lub przejścia do następnego poziomu w przypadku wygranej.
    /// Pozwala na powtórzenie poziomu lub wyjście do menu głównego w przypadku porażki.
    /// </summary>
    class LevelAccomplishedScreen : MenuScreen
    {
        private readonly Level _levelToRetry;
        private readonly Level _levelToContinue;
        private readonly int _gainedScore;

        #region Initialization
        /// <summary>
        /// Tworzy nowe menu ukończonego poziomu
        /// </summary>
        /// <param name="isWon">czy poziom zakończył się sukcesem</param>
        /// <param name="levelToRetry">poziom, który ma zostać wczytany w przypadku ponowienia gry</param>
        /// <param name="levelToContinue">poziom, który ma zostać wczytany w przypadku kontynuacji</param>
        /// <param name="gainedScore">liczba ponktów zdobyta w ukończonym poziomie</param>
        public LevelAccomplishedScreen(bool isWon, Level levelToRetry, Level levelToContinue, int gainedScore=0)
            : base("Level Accomplished")
        {
            _levelToRetry = levelToRetry;
            _levelToContinue = levelToContinue;
            _gainedScore = gainedScore;
            string scoreText = isWon ? String.Format("Your score: {0}", gainedScore) : "You have lost";
            const string labelText = "Do you want to retry?";

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
            ScreenManager.Start(new StartScreen(), new RankingScreen(_levelToRetry.TotalScore + _gainedScore));
        }

        private void ContinueSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(_levelToContinue));
        }

        protected override void OnCancel()
        {
            ScreenManager.Start(new StartScreen());
        }

        #endregion

    }
}
