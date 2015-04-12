namespace MetroDigger.Manager
{
    class UserManager
    {
        #region Singleton
        public static UserManager Instance { get { return _instance; } }

        private static readonly UserManager _instance = new UserManager();
        #endregion

        private UserManager()
        {

        }


        public void SignIn(string text)
        {

        }

        public void SaveGame(string text)
        {
            throw new System.NotImplementedException();
        }

        public void LoadGame(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
