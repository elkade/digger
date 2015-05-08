using System;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay.Abstract
{
    public interface ICollector
    {
        bool HasDrill { get; set; }
        int PowerCellsCount { get; set; }
        event Action<ICollector, Tile, Tile> Visited;

    }
}
