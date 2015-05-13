using System.Collections.Generic;
using System.Globalization;
using MetroDigger.Manager;
using MetroDigger.Screens.MenuObjects;

namespace MetroDigger.Screens
{
    /// <summary>
    /// Menu renkingu. Pozwala na wybór numeru poziomu,
    /// którego ranking najlepszych wyników ma zostać pokazany oraz na wyczysczenie konkretnej listy.
    /// </summary>
    class RankingScreen : MenuScreen
    {
        private readonly MenuLabel[] _congratsLabels;

        /// <summary>
        /// Tworzy menu rankingu
        /// </summary>
        /// <param name="score">nowozdobyty wynik</param>
        /// <param name="lvl">jakiego poziomu ranking ma dotyczyć. null=całej gry</param>
        public RankingScreen(int? score = null, int? lvl=null)
            : base("Ranking")
        {
            MenuEntry back = new MenuEntry("Back");
            MenuEntry clear = new MenuEntry("Clear");

            string[] levelsLabels = new string[GameManager.Instance.GetMaxLevel()+1];
            levelsLabels[0] = "all";
            for (int j = 1; j < levelsLabels.Length; j++)
                levelsLabels[j] = j.ToString(CultureInfo.InvariantCulture);
            MenuCheckField levelPicker = new MenuCheckField("Which level: ", levelsLabels, (lvl??-1)+1);

            if (score != null)
            {
                string text1 = "Congratulations " + GameManager.Instance.UserName;
                const string text2 = "You have finished MetroDigger!";
                string text3 = "Your score: " + score.Value;
                _congratsLabels = new[] { new MenuLabel(text1), new MenuLabel(text2), new MenuLabel(text3)};
            }
            List<MenuLabel> bestScoresLabels = new List<MenuLabel> {new MenuLabel("Best scores:")};
            try
            {
                var bestScores = GameManager.Instance.LoadBestScores(lvl);

            int i = 1;
            foreach (var bestScore in bestScores)
            {
                bestScoresLabels.Add(new MenuLabel(i+". "+ bestScore.Name +" "+bestScore.Score));
                i++;
            }

            back.Selected += OnCancel;
            clear.Selected += (sender, args) =>
            {
                GameManager.Instance.ClearRanking(lvl);
                ScreenManager.SwitchScreen(new RankingScreen(null,lvl));
            };
            levelPicker.Selected +=
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

            foreach (var label in bestScoresLabels)
                MenuObjects.Add(label);
            MenuObjects.Add(levelPicker);
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
