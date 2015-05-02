using System;
using System.Collections.Generic;
using System.Linq;
using MetroDigger.Manager;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class ChooseLevelScreen : MenuScreen
    {
        #region Initialization

        public ChooseLevelScreen()
            : base("Choose Level")
        {
            
            MenuObject[] levelsList = new MenuObject[GameManager.Instance.MaxLevel];

            var unlockedLvls = new List<int>(GameManager.Instance.UnlockedLevels());
            unlockedLvls.Add(unlockedLvls.Count);

            for (int i = 0; i < levelsList.Length; i++)
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

        }

        #endregion
    }
}
