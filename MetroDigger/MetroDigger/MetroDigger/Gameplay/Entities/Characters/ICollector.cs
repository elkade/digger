using System;
using MetroDigger.Gameplay.Entities.Tiles;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public interface ICollector
    {
        bool HasDrill { get; set; }
        int PowerCellCount { get; set; }
        event Action<ICollector, Tile, Tile> Visited;

    }
}
