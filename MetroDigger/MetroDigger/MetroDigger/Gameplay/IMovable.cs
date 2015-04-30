using MetroDigger.Gameplay.Entities.Tiles;

namespace MetroDigger.Gameplay
{
    public interface IMovable
    {
        void StartMoving(Tile destinationTile);
    }
}
