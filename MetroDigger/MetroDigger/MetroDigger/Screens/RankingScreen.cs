using System;
using System.Collections.Generic;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    class RankingScreen : MenuScreen
    {
        private readonly MenuLabel[] _congratsLabels;
        private readonly List<MenuLabel> _bestScoresLabels;

        public RankingScreen(int? score = null)
            : base("Ranking")
        {
            if (score != null)
            {
                string text1 = "Congratulations " + GameManager.Instance.UserName;
                string text2 = "You have finished MetroDigger!";
                string text3 = "Your score: " + score.Value;

                _congratsLabels = new[] { new MenuLabel(text1), new MenuLabel(text2), new MenuLabel(text3), };
            }
            _bestScoresLabels = new List<MenuLabel>();
            _bestScoresLabels.Add(new MenuLabel("Best scores:"));

            var bestScores = GameManager.Instance.LoadBestScores();
            int i = 1;
            foreach (var bestScore in bestScores)
            {
                _bestScoresLabels.Add(new MenuLabel(i+". "+ bestScore.Name +" "+bestScore.Score));
                i++;
            }

            MenuEntry back = new MenuEntry("Back");
            back.Selected += OnCancel;
            
            if(_congratsLabels!=null)
                foreach (var congratsLabel in _congratsLabels)
                    MenuObjects.Add(congratsLabel);

            foreach (var label in _bestScoresLabels)
                MenuObjects.Add(label);

            MenuObjects.Add(back);
        }

    }
}
