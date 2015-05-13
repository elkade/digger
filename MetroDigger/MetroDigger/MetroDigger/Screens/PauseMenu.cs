using System;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Menu pauzy gry. Daje takie same mozliwości jak menu startowe.
    /// Przez naciśnięcie klawisza Esc pozwala wrócić do aktualnie rozgrywanej gry.
    /// </summary>
    class PauseMenu : MenuScreen
    {
        #region Initialization
        /// <summary>
        /// Tworzy menu pauzy gry
        /// </summary>
        public PauseMenu()
            : base("MetroDigger")
        {
            MenuEntry newGameEntry = new MenuEntry("Play Game");
            MenuEntry saveGameEntry = new MenuEntry("Save Game");
            MenuEntry loadGameEntry = new MenuEntry("Load Game");
            MenuEntry optionsEntry = new MenuEntry("Options");
            MenuEntry rankingEntry = new MenuEntry("Ranking");
            MenuEntry exitEntry = new MenuEntry("Exit");

            newGameEntry.Selected += NewGameSelected;
            saveGameEntry.Selected += SaveGameSelected;
            loadGameEntry.Selected += LoadGameSelected;
            optionsEntry.Selected += OptionsSelected;
            rankingEntry.Selected += RankingSelected;
            exitEntry.Selected += Exit;

            MenuObjects.Add(newGameEntry);
            MenuObjects.Add(saveGameEntry);
            MenuObjects.Add(loadGameEntry);
            MenuObjects.Add(optionsEntry);
            MenuObjects.Add(rankingEntry);
            MenuObjects.Add(exitEntry);
        }

        private void RankingSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new RankingScreen());
        }

        #endregion

        #region Handle Input

        void NewGameSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen( new ChooseLevelScreen());//GameplayScreen(0)
        }

        void SaveGameSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new SaveMenuScreen());
        }

        void LoadGameSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoadMenuScreen());
        }

        void OptionsSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        private void Exit(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
