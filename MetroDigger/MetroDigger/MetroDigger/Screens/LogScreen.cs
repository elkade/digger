using System;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Menu logowania. Pierwsze menu gry. Pozwala na wpisanie nazwy użytkownika.
    /// </summary>
    class LogScreen : MenuScreen
    {
        #region Initialization

        private readonly MenuTextInput _nameInput;
        private const string FirstText = "Type your name...";
        /// <summary>
        /// Tworzy menu logowania.
        /// </summary>
        public LogScreen()
            : base("Type your name")
        {
            _nameInput = new MenuTextInput(FirstText);
            MenuEntry startEntry = new MenuEntry("Submit");
            MenuEntry exitEntry = new MenuEntry("Exit");

            startEntry.Selected += StartEntrySelected;
            exitEntry.Selected += OnCancel;

            MenuObjects.Add(_nameInput);
            MenuObjects.Add(startEntry);
            MenuObjects.Add(exitEntry);
        }

        #endregion

        #region Handle Input

        void StartEntrySelected(object sender, EventArgs e)
        {
            if (_nameInput.Text == FirstText)
                return;
            GameManager.Instance.SignIn(_nameInput.Text);
            ScreenManager.AddScreen(new StartScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
