using MetroDigger.Gameplay.Entities;
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
        public KeyboardDriver(DynamicEntity entity, Vector2 unit, Tile[,] board)
            : base(entity, unit, board)
        {
            _im = InputHandler.Instance;
        }

        public override void UpdateMovement()
        {
            bool wsad = GameOptions.Instance.Controls == Controls.Wsad;//to powinien ogarniac inputmanager
            var dirVec = new Vector2(_im.Horizontal(wsad) * Unit.X, _im.Vertical(wsad) * Unit.Y);

            if (_im.IsNewKeyPress(Keys.Space))
                Entity.StartShooting();

            if (dirVec != Vector2.Zero)
            {
                Tile destTile = PosToTile(dirVec + Entity.Position);
                if (destTile != null && Entity.State == EntityState.Idle)
                    switch (destTile.Accessibility)
                    {
                        case Accessibility.Free:
                            Entity.StartMoving(destTile);
                            break;
                        case Accessibility.Water:
                            Entity.StartMoving(destTile);
                            break;
                        case Accessibility.Rock:
                            break;
                        case Accessibility.Soil:
                            Entity.StartDrilling(destTile);
                            break;
                    }
            }

            Entity.Update();
        }
    }
}
