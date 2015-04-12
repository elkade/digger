﻿using System;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class PauseMenu2 : MenuScreen
    {
        #region Initialization

        public PauseMenu2()
            : base("MetroDigger")
        {
            MenuEntry newGameEntry = new MenuEntry("Play Game");
            MenuEntry saveGameEntry = new MenuEntry("Save Game");
            MenuEntry loadGameEntry = new MenuEntry("Load Game");
            MenuEntry optionsEntry = new MenuEntry("Options");
            MenuEntry exitEntry = new MenuEntry("Exit");

            newGameEntry.Selected += NewGameSelected;
            saveGameEntry.Selected += SaveGameSelected;
            loadGameEntry.Selected += LoadGameSelected;
            optionsEntry.Selected += OptionsSelected;
            exitEntry.Selected += Exit;

            MenuObjects.Add(newGameEntry);
            MenuObjects.Add(saveGameEntry);
            MenuObjects.Add(loadGameEntry);
            MenuObjects.Add(optionsEntry);
            MenuObjects.Add(exitEntry);
        }

        #endregion

        #region Handle Input

        void NewGameSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(GameManager, true, new GameplayScreen(true));
        }

        void SaveGameSelected(object sender, EventArgs e)
        {
            GameManager.AddScreen(new OptionsMenuScreen());
        }

        void LoadGameSelected(object sender, EventArgs e)
        {
            GameManager.AddScreen(new OptionsMenuScreen());
        }

        void OptionsSelected(object sender, EventArgs e)
        {
            GameManager.AddScreen(new OptionsMenuScreen());
        }

        private void Exit(object sender, EventArgs e)
        {
            GameManager.Game.Exit();
        }

        #endregion
    }
}
