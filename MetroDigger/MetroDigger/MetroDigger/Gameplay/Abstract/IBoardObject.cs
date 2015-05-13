using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;
using IUpdateable = MetroDigger.Gameplay.Abstract.IUpdateable;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, kt�ry mo�e zosta� umieszczony na planszy gry.
    /// </summary>
    public interface IBoardObject : IMoveable, IDrawable
    {
        /// <summary>
        /// Kierunek, w kt�rym ustawiony jest obiekt.
        /// </summary>
        Vector2 Direction { get; }
        /// <summary>
        /// Kafelek zajmowany przez obiekt.
        /// </summary>
        Tile OccupiedTile { get; }
        /// <summary>
        /// Szeroko�� obiektu.
        /// </summary>
        float Width { get; }
        /// <summary>
        /// Wysoko�� obiektu.
        /// </summary>
        float Height { get; }
    }
    /// <summary>
    /// Obiekt, kt�ry mo�e si� porusza�.
    /// </summary>
    public interface IMoveable : IUpdateable
    {
        /// <summary>
        /// Pr�dko�� poruszania obiektu.
        /// </summary>
        float MovementSpeed { get; }
    }
}