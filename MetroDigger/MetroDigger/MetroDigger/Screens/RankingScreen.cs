using System.Collections.Generic;
using MetroDigger.Manager;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class RankingScreen : MenuScreen
    {
        private readonly MenuLabel[] _congratsLabels;
        private readonly List<MenuLabel> _bestScoresLabels;
        private MenuCheckField _levelPicker;


        public RankingScreen(int? score = null, int? lvl=null)
            : base("Ranking")
        {
            MenuEntry back = new MenuEntry("Back");
            MenuEntry clear = new MenuEntry("Clear");

            string[] levelsLabels = new string[GameManager.Instance.GetMaxLevel()+1];
            levelsLabels[0] = "all";
            for (int j = 1; j < levelsLabels.Length; j++)
                levelsLabels[j] = j.ToString();
            _levelPicker = new MenuCheckField("Which level: ", levelsLabels, (lvl??-1)+1);

            if (score != null)
            {
                string text1 = "Congratulations " + GameManager.Instance.UserName;
                string text2 = "You have finished MetroDigger!";
                string text3 = "Your score: " + score.Value;
                _congratsLabels = new[] { new MenuLabel(text1), new MenuLabel(text2), new MenuLabel(text3), };
            }
            _bestScoresLabels = new List<MenuLabel>();
            _bestScoresLabels.Add(new MenuLabel("Best scores:"));
            try
            {
                var bestScores = GameManager.Instance.LoadBestScores(lvl);

            int i = 1;
            foreach (var bestScore in bestScores)
            {
                _bestScoresLabels.Add(new MenuLabel(i+". "+ bestScore.Name +" "+bestScore.Score));
                i++;
            }

            back.Selected += OnCancel;
            clear.Selected += (sender, args) =>
            {
                GameManager.Instance.ClearRanking(lvl);
                ScreenManager.SwitchScreen(new RankingScreen(null,lvl));
            };
            _levelPicker.Selected +=
                (sender, args) =>
                {
                    int? lvlNo = ((MenuCheckField) sender).Number - 1 == -1
                        ? (int?) null
                        : ((MenuCheckField) sender).Number - 1;
                    ScreenManager.SwitchScreen(new RankingScreen(null, lvlNo));
                };

            if(_congratsLabels!=null)
                foreach (var congratsLabel in _congratsLabels)
                    MenuObjects.Add(congratsLabel);

            foreach (var label in _bestScoresLabels)
                MenuObjects.Add(label);
            MenuObjects.Add(_levelPicker);
            MenuObjects.Add(clear);
            }
            catch
            {
                ScreenManager.AddScreen(new MessageBoxScreen("Error occured."));
            }
            MenuObjects.Add(back);
        }

    }
}
