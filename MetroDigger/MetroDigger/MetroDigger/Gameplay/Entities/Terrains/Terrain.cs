using MetroDigger.Gameplay.Abstract;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    public interface ITerrain : IDrawable
    {
         Accessibility Accessibility { get; }
    }

    public abstract class Terrain : StaticEntity, ITerrain
    {
        protected Accessibility _accessibility;

        public  Accessibility Accessibility
        {
            get { return _accessibility; }
        }

        public Terrain()
        {
            ZIndex = -1000;

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
