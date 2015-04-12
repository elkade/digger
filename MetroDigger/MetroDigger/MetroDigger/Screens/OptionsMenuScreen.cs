using System;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        private MenuCheckField controlsCheckField;
        private MenuCheckField musicMenuCheckField;
        private MenuCheckField soundEffectsMenuCheckField;
        private MenuLabel shootLabel;

        public OptionsMenuScreen()
            : base("GameOptions")
        {
            controlsCheckField = new MenuCheckField("Control:", "Arrow", "Wsad", GameOptions.Instance.Controls==Controls.Arrows);
            musicMenuCheckField = new MenuCheckField("Music:", "On", "Off", GameOptions.Instance.IsMusicEnabled);
            soundEffectsMenuCheckField = new MenuCheckField("Sound effects:", "On", "Off", GameOptions.Instance.IsSoundEnabled);

            shootLabel = new MenuLabel("Space - shoot");

            MenuEntry back = new MenuEntry("Back");

            controlsCheckField.Selected += (sender, args) =>
            {
                GameOptions.Instance.Controls = ((MenuCheckField)sender).IsOn?Controls.Arrows:Controls.Wsad;
            };

            musicMenuCheckField.Selected += (sender, args) =>
            {
                GameOptions.Instance.IsMusicEnabled = ((MenuCheckField)sender).IsOn;
                SoundManager.Instance.Switch(((MenuCheckField)sender).IsOn, SoundType.Music);
            };
            soundEffectsMenuCheckField.Selected += (sender, args) =>
            {
                GameOptions.Instance.IsSoundEnabled = ((MenuCheckField)sender).IsOn;
                SoundManager.Instance.Switch(((MenuCheckField)sender).IsOn, SoundType.SoundEffect);
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
