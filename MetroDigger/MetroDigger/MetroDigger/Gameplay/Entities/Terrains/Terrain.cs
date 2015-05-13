using MetroDigger.Gameplay.Abstract;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    /// <summary>
    /// Teren kafelka. Przechowuje informację, czy kafelek jest dostępny dla obiektu.
    /// </summary>
    public interface ITerrain : IDrawable
    {
        /// <summary>
        /// Dostępność terenu dla obiektów.
        /// </summary>
        Accessibility Accessibility { get; }
    }

    /// <summary>
    /// Abstrakcyjna klasa bazowa dla szczegółowych typów terenu
    /// </summary>
    public abstract class Terrain : StaticEntity, ITerrain
    {
        protected Accessibility _accessibility;
        /// <summary>
        /// Dostępność terenu dla obiektów.
        /// </summary>
        public  Accessibility Accessibility
        {
            get { return _accessibility; }
        }

        protected Terrain()
        {
            ZIndex = -1000;

            _accessibility = Accessibility.Rock;
        }
    }
    /// <summary>
    /// Typy dostępności terenu dla obiektów.
    /// </summary>
    public enum Accessibility
    {
        Free = 0,
        Soil = 1,
        Water = 2,
        Rock = 4,
        Buffer
    }
}
