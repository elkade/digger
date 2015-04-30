using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    ///     The options screen is brought up over the top of the main menu
    ///     screen, and gives the user a chance to configure the game
    ///     in various hopefully useful ways.
    /// </summary>
    internal class OptionsMenuScreen : MenuScreen
    {
        private MenuCheckField controlsCheckField;
        private MenuCheckField musicMenuCheckField;
        private MenuLabel shootLabel;
        private MenuCheckField soundEffectsMenuCheckField;

        public OptionsMenuScreen()
            : base("GameOptions")
        {
            controlsCheckField = new MenuCheckField("Control:", new[] {"Arrow", "Wsad"},
                GameOptions.Instance.Controls == Controls.Arrows ? 0 : 1);
            musicMenuCheckField = new MenuCheckField("Music:", new[] {"On", "Off"},
                GameOptions.Instance.IsMusicEnabled ? 0 : 1);
            soundEffectsMenuCheckField = new MenuCheckField("Sound effects:", new[] {"On", "Off"},
                GameOptions.Instance.IsSoundEnabled ? 0 : 1);

            shootLabel = new MenuLabel("Space - shoot");

            var back = new MenuEntry("Back");

            controlsCheckField.Selected +=
                (sender, args) =>
                {
                    GameOptions.Instance.Controls = ((MenuCheckField) sender).Number==0 ? Controls.Arrows : Controls.Wsad;
                };

            musicMenuCheckField.Selected += (sender, args) =>
            {
                GameOptions.Instance.IsMusicEnabled = ((MenuCheckField)sender).Number == 0;
                MediaManager.Instance.Switch(SoundType.Music);
            };
            soundEffectsMenuCheckField.Selected += (sender, args) =>
            {
                GameOptions.Instance.IsSoundEnabled = ((MenuCheckField)sender).Number == 0;
                MediaManager.Instance.Switch(SoundType.SoundEffect);
            };
            back.Selected += OnCancel;

            MenuObjects.Add(controlsCheckField);
            MenuObjects.Add(shootLabel);
            MenuObjects.Add(musicMenuCheckField);
            MenuObjects.Add(soundEffectsMenuCheckField);
            MenuObjects.Add(back);
        }
    }
}