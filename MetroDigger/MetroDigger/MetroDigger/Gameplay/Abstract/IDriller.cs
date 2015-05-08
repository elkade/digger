using System;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay
{
    public interface IDriller
    {
        void StartDrilling(Tile destination);

        event Action<IDriller, Tile> Drilled;
    }
}
