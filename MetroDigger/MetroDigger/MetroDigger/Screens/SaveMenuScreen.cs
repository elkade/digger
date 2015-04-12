using System;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class SaveMenuScreen : MenuScreen
    {
        #region Initialization

        MenuTextInput _textInput = new MenuTextInput("...");


        public SaveMenuScreen()
            : base("Save Game")
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
            UserManager.Instance.SaveGame(_textInput.Text);
            OnCancel();
        }
        #endregion
    }
}
