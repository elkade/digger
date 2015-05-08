using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;
using IUpdateable = MetroDigger.Gameplay.Entities.IUpdateable;

namespace MetroDigger.Gameplay.Abstract
{
    public interface IBoardObject : IMoveable
    {
        Vector2 Direction { get; }
        Tile OccupiedTile { get; }
        float Width { get; }
        float Height { get; }
    }

    public interface IMoveable : IUpdateable
    {
        float MovementSpeed { get; set; }
    }
}