using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MetroDigger.Gameplay.Drivers
{
    /// <summary>
    /// Sterownik wyznaczający ścieżkę poruszania się obiektu zgodnie z rozkazami płynącymi z InputManagera
    /// </summary>
    internal class KeyboardDriver : Driver
    {
        private readonly InputHandler _im;
        public KeyboardDriver(Vector2 unit, Board board)
            : base(unit, board)
        {
            _im = InputHandler.Instance;
        }
        /// <summary>
        /// Aktualizuje ścieżkę, po której ma poruszać się sterowany obiekt
        /// </summary>
        /// <param name="mover">narzędzie dokonujące zmiany położenia obiektu</param>
        /// <param name="state">okraśla stan, w jakim znajduje się obiekt</param>
        public override void UpdateMovement(IMover mover, EntityState state)
        {
            bool wsad = GameOptions.Instance.Controls == Controls.Wsad;//to powinien ogarniac inputmanager
            int x = _im.Horizontal(wsad);
            int y = _im.Vertical(wsad);
            Vector2 direction;
            if((x!=0 && y==0)||(y!=0 && x==0))
                direction = new Vector2(x, y);
            else
                direction = new Vector2(x, 0);
            var dirVec = new Vector2(direction.X * Unit.X, direction.Y * Unit.Y);

            if (_im.IsNewKeyPress(Keys.Space))
            {
                if(state!=EntityState.Drilling)
                    RaiseShoot();
            }

            if (dirVec != Vector2.Zero)
            {
                Tile destTile = PosToTile(dirVec + mover.Position);
                if (destTile != null && state == EntityState.Idle)
                    switch (destTile.Accessibility)
                    {
                        case Accessibility.Free:
                            RaiseMove(destTile);
                            break;
                        case Accessibility.Water:
                            RaiseMove(destTile);
                            break;
                        case Accessibility.Rock:
                            RaiseTurn(direction);
                            break;
                        case Accessibility.Soil:
                            RaiseDrill(destTile);
                            break;
                    }
            }
        }
    }
}
