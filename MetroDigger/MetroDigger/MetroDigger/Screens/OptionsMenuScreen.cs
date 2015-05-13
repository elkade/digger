using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    ///Menu opcji gry. Pozwala na w³¹czenie/wy³¹czenie dŸwiêków i muzyki oraz zmiane sterowania na strza³ki/wsad.
    /// </summary>
    internal class OptionsMenuScreen : MenuScreen
    {
        public OptionsMenuScreen()
            : base("GameOptions")
        {
            MenuCheckField controlsCheckField = new MenuCheckField("Control:", new[] {"Arrow", "Wsad"},
                GameOptions.Instance.Controls == Controls.Arrows ? 0 : 1);
            MenuCheckField musicMenuCheckField = new MenuCheckField("Music:", new[] {"On", "Off"},
                GameOptions.Instance.IsMusicEnabled ? 0 : 1);
            MenuCheckField soundEffectsMenuCheckField = new MenuCheckField("Sound effects:", new[] {"On", "Off"},
                GameOptions.Instance.IsSoundEnabled ? 0 : 1);

            MenuLabel shootLabel = new MenuLabel("Space - shoot");

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