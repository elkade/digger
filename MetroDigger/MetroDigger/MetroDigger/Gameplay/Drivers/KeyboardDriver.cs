using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using MetroDigger.Manager.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MetroDigger.Gameplay.Drivers
{
    internal class KeyboardDriver : Driver
    {
        private InputHandler _im;
        public KeyboardDriver(Vector2 unit, Tile[,] board)
            : base(unit, board)
        {
            _im = InputHandler.Instance;
        }

        public override void UpdateMovement(MovementHandler mh, EntityState state)
        {
            bool wsad = GameOptions.Instance.Controls == Controls.Wsad;//to powinien ogarniac inputmanager
            var dirVec = new Vector2(_im.Horizontal(wsad) * Unit.X, _im.Vertical(wsad) * Unit.Y);

            if (_im.IsNewKeyPress(Keys.Space))
                RaiseShoot();

            if (dirVec != Vector2.Zero)
            {
                Tile destTile = PosToTile(dirVec + mh.Position);
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
                            break;
                        case Accessibility.Soil:
                            RaiseDrill(destTile);
                            break;
                    }
            }
        }
    }
}
