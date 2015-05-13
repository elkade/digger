using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    /// <summary>
    /// Sterownik, który wyznacza ścieżkę zgodną ze swobodnym spadkiem w kierunku działania siły grawitacji.
    /// </summary>
    class GravityDriver : Driver
    {
        private readonly Vector2 _gravityDirection;
        /// <summary>
        /// Tworzy nowy sterownik grawitacyjny
        /// </summary>
        /// <param name="unit">Rozmiar kafelków na planszy</param>
        /// <param name="board">Plansza, po której porusza się obiekt</param>
        /// <param name="gravityDirection">Keirunek, w którym działa siła grawitacji</param>
        public GravityDriver(Vector2 unit, Board board, Vector2 gravityDirection) : base(unit, board)
        {
            _gravityDirection = gravityDirection;
        }
        /// <summary>
        /// Aktualizuje ścieżkę, po której ma poruszać się sterowany obiekt
        /// </summary>
        /// <param name="mover">narzędzie dokonujące zmiany położenia obiektu</param>
        /// <param name="state">okraśla stan, w jakim znajduje się obiekt</param>
        public override void UpdateMovement(IMover mover, EntityState state)
        {
            if (state == EntityState.Moving) return;
            if (state != EntityState.StartingMoving)
                mover.Direction = _gravityDirection;

            var dirVec = new Vector2(_gravityDirection.X * Unit.X, _gravityDirection.Y * Unit.Y);

            var tileBelow = PosToTile(dirVec + mover.Position);

            if(tileBelow.Accessibility!=Accessibility.Free && tileBelow.Accessibility != Accessibility.Water)
                dirVec = new Vector2(mover.Direction.X * Unit.X, mover.Direction.Y * Unit.Y);

            if (dirVec != Vector2.Zero && Vector2.Normalize(dirVec)!=new Vector2(0,-1))
            {
                Tile destTile = PosToTile(dirVec + mover.Position);
                if (destTile != null)
                    switch (destTile.Accessibility)
                    {
                        case Accessibility.Free:
                            RaiseMove(destTile);
                            break;
                        case Accessibility.Water:
                            RaiseMove(destTile);
                            break;
                        default:
                            break;
                    }
            }
        }

    }
}
