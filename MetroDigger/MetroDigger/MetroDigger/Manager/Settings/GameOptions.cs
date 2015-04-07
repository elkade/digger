namespace MetroDigger.Manager.Settings
{
    public class GameOptions
    {
        #region Singleton
        public static GameOptions Instance { get { return _instance; } }

        private static readonly GameOptions _instance = new GameOptions();
        #endregion

        private GameOptions()
        {
            
        }
        public Controls Controls { get; set; }
        public bool IsMusicEnabled { get; set; }
        public bool IsSoundEnabled { get; set; }
    }

    public enum Controls
    {
        Arrows = 0,
        Wsad = 1
    }
}
