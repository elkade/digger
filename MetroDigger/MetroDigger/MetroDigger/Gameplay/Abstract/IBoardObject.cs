using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;
using IUpdateable = MetroDigger.Gameplay.Abstract.IUpdateable;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, który mo¿e zostaæ umieszczony na planszy gry.
    /// </summary>
    public interface IBoardObject : IMoveable, IDrawable
    {
        /// <summary>
        /// Kierunek, w którym ustawiony jest obiekt.
        /// </summary>
        Vector2 Direction { get; }
        /// <summary>
        /// Kafelek zajmowany przez obiekt.
        /// </summary>
        Tile OccupiedTile { get; }
        /// <summary>
        /// Szerokoœæ obiektu.
        /// </summary>
        float Width { get; }
        /// <summary>
        /// Wysokoœæ obiektu.
        /// </summary>
        float Height { get; }
    }
    /// <summary>
    /// Obiekt, który mo¿e siê poruszaæ.
    /// </summary>
    public interface IMoveable : IUpdateable
    {
        /// <summary>
        /// Prêdkoœæ poruszania obiektu.
        /// </summary>
        float MovementSpeed { get; }
    }
}