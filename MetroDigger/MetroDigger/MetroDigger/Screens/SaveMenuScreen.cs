using System;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Menu zapisywania gry. Pozwala na wpisanie nazwy pliku, do którego nastąpi zapis gry.
    /// </summary>
    class SaveMenuScreen : MenuScreen
    {
        #region Initialization

        readonly MenuTextInput _textInput = new MenuTextInput("...");

        /// <summary>
        /// Tworzy menu zapisywania gry
        /// </summary>
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
            try
            {
                GameManager.Instance.SaveGameToFile(_textInput.Text);
            }
            catch (Exception)
            {
                ScreenManager.AddScreen(new MessageBoxScreen("File with specified name already exists."));
                return;
            }
            OnCancel();
        }
        #endregion
    }
}
