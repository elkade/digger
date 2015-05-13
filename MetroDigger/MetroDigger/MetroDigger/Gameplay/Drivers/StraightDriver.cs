using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    /// <summary>
    /// Sterownik, który wyznacza ścieżkę - linię prostą.
    /// </summary>
    class StraightDriver : Driver
    {
        /// <summary>
        /// Tworzy nowy prostoliniowy sterownik
        /// </summary>
        /// <param name="unit">Rozmiar kafelków na planszy</param>
        /// <param name="board">Plansza, po której porusza się obiekt</param>
        public StraightDriver(Vector2 unit, Board board)
            : base(unit, board)
        {
        }
        /// <summary>
        /// Aktualizuje ścieżkę, po której ma poruszać się sterowany obiekt
        /// </summary>
        /// <param name="mover">narzędzie dokonujące zmiany położenia obiektu</param>
        /// <param name="state">okraśla stan, w jakim znajduje się obiekt</param>
        public override void UpdateMovement(IMover mover, EntityState state)
        {
            if (state == EntityState.Moving) return;
            var dirVec = new Vector2(mover.Direction.X * Unit.X, mover.Direction.Y * Unit.Y);

            Tile destTile = PosToTile(dirVec + mover.Position);
            RaiseMove(destTile);
        }
    }
}
