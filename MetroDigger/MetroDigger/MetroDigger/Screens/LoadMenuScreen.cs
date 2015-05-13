using System;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Menu ładowania gry zapisanej w pliku. Pozwala wpisać nazwę pliku, z którego ma zostać wczytana gra.
    /// </summary>
    class LoadMenuScreen : MenuScreen
    {
        #region Initialization

        readonly MenuTextInput _textInput = new MenuTextInput("...");

        /// <summary>
        /// Tworzy noew menu ładowania gry z pliku.
        /// </summary>
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
            try
            {
                GameManager.Instance.LoadLevelFromFile(_textInput.Text,true);
            }
            catch
            {
                ScreenManager.AddScreen(new MessageBoxScreen("Unable to load file of specific name."));
                return;
            }
            LoadingScreen.Load(ScreenManager,false,new GameplayScreen(GameManager.Instance.LoadSavedLevelFromMemory()));
        }
        #endregion
    }
}
