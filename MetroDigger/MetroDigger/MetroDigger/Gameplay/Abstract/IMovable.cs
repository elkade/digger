using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay
{
    public interface IMovable
    {
        void StartMoving(Tile destinationTile);
    }
}
