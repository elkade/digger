using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Entities
{
    public interface IBoardObject : IMoveable
    {
        Vector2 Direction { get; }
        Tile OccupiedTile { get; }
        float Width { get; }
        float Height { get; }
    }

    public interface IMoveable
    {
        Vector2 Position { get; }
        float MovementSpeed { get; set; }
    }
}