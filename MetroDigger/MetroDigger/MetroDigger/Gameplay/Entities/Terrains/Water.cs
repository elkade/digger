using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Water : Terrain
    {
        internal enum WaterLevel
        {
            FullClosed=3,
            FullOpen=2,
            Half=1,
            Quarter=0
        }

        public Water(WaterLevel lvl, bool isFull = true, bool isClosed = true)
        {
            if (lvl == WaterLevel.FullClosed && !isClosed)
                lvl = WaterLevel.FullOpen;
            _level = lvl;
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.Water, 1, false, 300, MediaManager.Instance.Scale, 300) };
            Sprite.CustomIndex = (int)_level;
            Sprite.PlayAnimation(Animations[0], false);
            _accessibility = Accessibility.Water;
            IsFull = isFull;
        }

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
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.Water, 1, false, 300, MediaManager.Instance.Scale,300) };
            Sprite.CustomIndex = (int)_level;
            Sprite.PlayAnimation(Animations[0],false);
            _accessibility = Accessibility.Water;
            IsFull = isFull;
        }

        private WaterLevel _level;

        public WaterLevel Level
        {
            set
            {
                _level = value;
                Sprite.CustomIndex = (int) _level;
                Sprite.PlayAnimation(Animations[0],false);
            }
            get { return _level; }
        }

        public bool IsFull { get; set; }
    }
}
