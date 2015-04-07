#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements

using System;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using XNA_GSM.Screens.MenuObjects;

#endregion

namespace MetroDigger
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry ungulateMenuEntry;
        MenuEntry languageMenuEntry;
        MenuEntry frobnicateMenuEntry;
        MenuEntry elfMenuEntry;
        MenuCheckField musicMenuCheckField;
        MenuCheckField soundEffectsMenuCheckField;

        enum Ungulate
        {
            BactrianCamel,
            Dromedary,
            Llama,
        }

        static Ungulate currentUngulate = Ungulate.Dromedary;

        static string[] languages = { "C#", "French", "Deoxyribonucleic acid" };
        static int currentLanguage = 0;

        static bool frobnicate = true;

        static int elf = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("GameOptions")
        {
            // Create our menu entries.
            ungulateMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            frobnicateMenuEntry = new MenuEntry(string.Empty);
            elfMenuEntry = new MenuEntry(string.Empty);
            musicMenuCheckField = new MenuCheckField("Music:", "On", "Off", GameOptions.Instance.IsMusicEnabled);
            soundEffectsMenuCheckField = new MenuCheckField("Sound effects:", "On", "Off", GameOptions.Instance.IsSoundEnabled);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            ungulateMenuEntry.Selected += UngulateMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            frobnicateMenuEntry.Selected += FrobnicateMenuEntrySelected;
            elfMenuEntry.Selected += ElfMenuEntrySelected;
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
            
            // Add entries to the menu.
            MenuObjects.Add(ungulateMenuEntry);
            MenuObjects.Add(languageMenuEntry);
            MenuObjects.Add(frobnicateMenuEntry);
            MenuObjects.Add(elfMenuEntry);
            MenuObjects.Add(musicMenuCheckField);
            MenuObjects.Add(soundEffectsMenuCheckField);
            MenuObjects.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu _text.
        /// </summary>
        void SetMenuEntryText()
        {
            ungulateMenuEntry.Text = "Preferred ungulate: " + currentUngulate;
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
            frobnicateMenuEntry.Text = "Frobnicate: " + (frobnicate ? "on" : "off");
            elfMenuEntry.Text = "elf: " + elf;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void UngulateMenuEntrySelected(object sender, EventArgs e)
        {
            currentUngulate++;

            if (currentUngulate > Ungulate.Llama)
                currentUngulate = 0;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, EventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void FrobnicateMenuEntrySelected(object sender, EventArgs e)
        {
            frobnicate = !frobnicate;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, EventArgs e)
        {
            elf++;

            SetMenuEntryText();
        }


        #endregion
    }
}
