using System;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class LoadMenuScreen : MenuScreen
    {
        #region Initialization

        MenuTextInput _textInput = new MenuTextInput("...");


        public LoadMenuScreen()
            : base("Load Game")
        {
            MenuLabel label = new MenuLabel("Enter name of your save:");
            MenuEntry okMenuEntry = new MenuEntry("OK");
            MenuEntry exitMenuEntry = new MenuEntry("Cancel");

            okMenuEntry.Selected += OkSelected;
            exitMenuEntry.Selected += OnCancel;

            MenuObjects.Add(label);
            MenuObjects.Add(_textInput);
            MenuObjects.Add(okMenuEntry);
            MenuObjects.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input

        void OkSelected(object sender, EventArgs e)
        {
            GameManager.Instance.LoadLevelFromFile(_textInput.Text);
            LoadingScreen.Load(ScreenManager,false,new GameplayScreen(GameManager.Instance.LoadSavedLevelFromMemory()));
        }
        #endregion
    }
}
