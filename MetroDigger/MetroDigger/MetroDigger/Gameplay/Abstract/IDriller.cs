using System;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt zdolny do wiercenia
    /// </summary>
    public interface IDriller
    {
        /// <summary>
        /// Rozpoczyna wiercenie
        /// </summary>
        /// <param name="destination">Docelowy kafelek wiercenia.</param>
        void StartDrilling(Tile destination);
        /// <summary>
        /// Zdarzenie wywoływane, gdy obiekt zakończy wiercenie.
        /// </summary>
        event Action<IDriller, Tile, Tile> Drilled;
    }
}
