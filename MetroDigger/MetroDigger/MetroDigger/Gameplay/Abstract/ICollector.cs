using System;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, który potrafi zbierać inne obiekty z planszy.
    /// </summary>
    public interface ICollector
    {
        /// <summary>
        /// Określa, czy obiekt ma wiertło
        /// </summary>
        bool HasDrill { get; set; }
        /// <summary>
        /// Określa liczbę baterii posiadanych przez obiekt
        /// </summary>
        int PowerCellsCount { get; set; }
        /// <summary>
        /// Zdarzenie wywoływane w momencie odwiedzenia nowego kafelka.
        /// </summary>
        event Action<ICollector, Tile, Tile> Visited;

    }
}
