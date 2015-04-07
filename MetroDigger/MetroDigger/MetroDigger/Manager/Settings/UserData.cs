using System.Collections.Generic;

namespace MetroDigger.Manager.Settings
{
    class UserData
    {
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

    class UserLevel
    {
        private int _number;
        private int _bestScore;
        private bool _isUnblocked;
        private int _maxLives;
    }
}
