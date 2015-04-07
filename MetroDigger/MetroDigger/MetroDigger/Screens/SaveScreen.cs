#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using XNA_GSM.Screens.MenuObjects;

#endregion

namespace MetroDigger
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class SaveScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public SaveScreen()
            : base("Save Game")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry optionsMenuEntry = new MenuEntry("GameOptions");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuObjects.Add(playGameMenuEntry);
            MenuObjects.Add(optionsMenuEntry);
            MenuObjects.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(GameManager, true, new GameplayScreen(true));
        }


        /// <summary>
        /// Event handler for when the GameOptions menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            GameManager.AddScreen(new OptionsMenuScreen());
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel()
        {
            const string message = "Are you sure you want to exit this sample?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            GameManager.AddScreen(confirmExitMessageBox);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, EventArgs e)
        {
            GameManager.Game.Exit();
        }


        #endregion
    }
}
