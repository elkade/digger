using System.Collections.Generic;

namespace MetroDigger.Manager.Settings
{
    public class UserData
    {
        public UserData()
        {
            
        }
        private string _name;
        private List<UserLevel> _levels;

        public UserData(string name, List<UserLevel> levels)
        {
            Name = name;
            Levels = levels;
        }

        public List<UserLevel> Levels
        {
            get { return _levels; }
            set { _levels = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }

    public class UserLevel
    {

        public UserLevel()
        {
            BestScore = 0;
            IsUnlocked = true;
            MaxLives = 2;
            Number = 0;
        }

        public int Number { get; set; }

        public int BestScore { get; set; }

        public bool IsUnlocked { get; set; }

        public int MaxLives { get; set; }
    }
}
