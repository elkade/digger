using System;
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

            var unlockedLvls = GameManager.Instance.UnlockedLevels();

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

        private void RankingSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new RankingScreen());
        }

        #endregion

        #region Handle Input

        void NewGameSelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, new GameplayScreen(0));
        }

        void LoadGameSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoadMenuScreen());
        }

        void OptionsSelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}
