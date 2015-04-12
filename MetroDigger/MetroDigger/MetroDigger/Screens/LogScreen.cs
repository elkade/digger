using System;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class LogScreen : MenuScreen
    {
        #region Initialization

        private readonly MenuTextInput _nameInput;
        private readonly MenuEntry _startEntry;
        private readonly MenuEntry _exitEntry;

        public LogScreen()
            : base("Type your name")
        {
            _nameInput = new MenuTextInput("Type your name...");
            _startEntry = new MenuEntry("Submit");
            _exitEntry = new MenuEntry("Exit");

            _startEntry.Selected += StartEntrySelected;
            _exitEntry.Selected += OnCancel;

            MenuObjects.Add(_nameInput);
            MenuObjects.Add(_startEntry);
            MenuObjects.Add(_exitEntry);
        }

        #endregion

        #region Handle Input

        void StartEntrySelected(object sender, EventArgs e)
        {
            UserManager.Instance.SignIn(_nameInput.Text);
            GameManager.AddScreen(new StartScreen());
        }

        protected override void OnCancel()
        {
            GameManager.Game.Exit();
        }

        #endregion
    }
}
