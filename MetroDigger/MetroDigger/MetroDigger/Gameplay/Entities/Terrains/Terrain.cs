namespace MetroDigger.Gameplay.Entities.Terrains
{
    public abstract class Terrain : StaticEntity
    {
        protected Accessibility _accessibility;

        public  Accessibility Accessibility
        {
            get { return _accessibility; }
        }

        public Terrain()
        {
            _accessibility = Accessibility.Rock;
        }
    }
    public enum Accessibility
    {
        Free = 0,
        Soil = 1,
        Water = 2,
        Rock = 4,
        Buffer
    }
}
