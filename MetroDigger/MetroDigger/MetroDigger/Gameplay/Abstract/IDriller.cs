using System;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay.Abstract
{
    public interface IDriller
    {
        void StartDrilling(Tile destination);

        event Action<IDriller, Tile, Tile> Drilled;
    }
}
