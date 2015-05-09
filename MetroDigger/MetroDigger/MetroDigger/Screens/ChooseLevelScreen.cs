using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class ChooseLevelScreen : MenuScreen
    {
        #region Initialization

        public ChooseLevelScreen()
            : base("Choose Level")
        {

            MenuObject[] levelsList = new MenuObject[GameManager.Instance.GetMaxLevel()];
            MenuEntry exitMenuEntry = new MenuEntry("Back");

            var unlockedLvls = new List<int>(GameManager.Instance.UnlockedLevels());
            unlockedLvls.Add(unlockedLvls.Count);
            if (levelsList.Length == 0)
            {
                MenuObjects.Add(new MenuLabel("No levels in database."));

            }
            else for (int i = 0; i < levelsList.Length; i++)
            {
                if(unlockedLvls.Contains(i))
                {
                    levelsList[i] = new MenuEntry(i.ToString());
                    int levelNo = i;
                    levelsList[i].Selected += (sender, args) => LoadingScreen.Load(ScreenManager, true, new GameplayScreen(levelNo));
                }
                else
                    levelsList[i] = new MenuLabel(i.ToString());
                MenuObjects.Add(levelsList[i]);
            }
            exitMenuEntry.Selected += OnCancel;

            MenuObjects.Add(exitMenuEntry);

        }

        #endregion
    }
}
