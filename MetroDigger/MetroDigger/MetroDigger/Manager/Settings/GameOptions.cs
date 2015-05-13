namespace MetroDigger.Manager.Settings
{
    /// <summary>
    /// Reprezentuje zestaw opcji gry
    /// </summary>
    public class GameOptions
    {
        #region Singleton
        public static GameOptions Instance { get { return _instance; } }

        private static readonly GameOptions _instance = new GameOptions();
        #endregion

        private GameOptions()
        {
            
        }
        /// <summary>
        /// Typ kontroli klawiatury
        /// </summary>
        public Controls Controls { get; set; }
        /// <summary>
        /// Określa, czy muzyka jest włączona
        /// </summary>
        public bool IsMusicEnabled { get; set; }
        /// <summary>
        /// Określa, czy efekty dźwiąkowe są włączone
        /// </summary>
        public bool IsSoundEnabled { get; set; }
    }
    /// <summary>
    /// Typy kontroli klawiatury
    /// </summary>
    public enum Controls
    {
        Arrows = 0,
        Wsad = 1
    }
}
