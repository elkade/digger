using System;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class StartScreen : MenuScreen
    {
        #region Initialization

        public StartScreen()
            : base("MetroDigger")
        {
            MenuEntry newGameEntry = new MenuEntry("Play Game");
            MenuLabel saveGameEntry = new MenuLabel("Save Game");
            MenuEntry loadGameEntry = new MenuEntry("Load Game");
            MenuEntry optionsEntry = new MenuEntry("Options");
            MenuEntry rankingEntry = new MenuEntry("Ranking");
            MenuEntry exitEntry = new MenuEntry("Exit");

            newGameEntry.Selected += NewGameSelected;
            loadGameEntry.Selected += LoadGameSelected;
            optionsEntry.Selected += OptionsSelected;
            rankingEntry.Selected += RankingSelected;
            exitEntry.Selected += OnCancel;

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
            ScreenManager.AddScreen(new ChooseLevelScreen());//new GameplayScreen(0)
        }

        void LoadGameSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoadMenuScreen());
        }

        void OptionsSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
