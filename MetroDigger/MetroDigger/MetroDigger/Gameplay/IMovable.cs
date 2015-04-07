using MetroDigger.Gameplay.Entities.Tiles;

namespace MetroDigger.Gameplay
{
    internal interface IMovable
    {
        void StartMoving(Tile destinationTile);
    }
}
