using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    /// <summary>
    /// Teren z wodą.
    /// </summary>
    class Water : Terrain
    {
        internal enum WaterLevel
        {
            FullClosed=3,
            FullOpen=2,
            Half=1,
            Quarter=0
        }
        /// <summary>
        /// Tworzy nowy tern z wodą
        /// </summary>
        /// <param name="lvl">Określa poziom wody w terenie</param>
        /// <param name="isFull">Określa, czy teren przechowuje informacjęo tym, że przechowuje wodę.
        /// Jest źródłem wody.</param>
        /// <param name="isClosed">Określa, czy teren graniczy ze stropem</param>
        public Water(WaterLevel lvl, bool isFull = true, bool isClosed = true)
        {
            if (lvl == WaterLevel.FullClosed && !isClosed)
                lvl = WaterLevel.FullOpen;
            _level = lvl;
            var grc = MediaManager.Instance;
            AnimationPlayer.CustomIndex = 0;//(int)_level;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Water"), false);
            _accessibility = Accessibility.Water;
            IsFull = isFull;
        }
        /// <summary>
        /// Tworzy nowy teren z wodą
        /// </summary>
        /// <param name="lvl">Określa poziom wody w terenie</param>
        /// <param name="isFull">Określa, czy teren przechowuje informacjęo tym, że przechowuje wodę.
        /// Jest źródłem wody.</param>
        /// <param name="isClosed">Określa, czy teren graniczy ze stropem</param>
        public Water(double lvl = 1, bool isFull = true, bool isClosed = true)
        {
            if (lvl > 0.66)
            {
                _level = isClosed ? WaterLevel.FullClosed : WaterLevel.FullOpen;
            }
            else if (lvl > 0.33)
                _level = WaterLevel.Half;
            else
                _level = WaterLevel.Quarter;
            AnimationPlayer.CustomIndex = 0;//(int)_level;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Water"),false);
            _accessibility = Accessibility.Water;
            IsFull = isFull;
        }

        private WaterLevel _level;
        /// <summary>
        /// Określa poziom wody w terenie
        /// </summary>
        public WaterLevel Level
        {
            set
            {
                _level = value;
                AnimationPlayer.CustomIndex = 0;//(int) _level;
                AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Water"), false);
            }
            get { return _level; }
        }
        /// <summary>
        /// Określa, czy teren jest źródłem wody
        /// </summary>
        public bool IsFull { get; set; }
    }
}
