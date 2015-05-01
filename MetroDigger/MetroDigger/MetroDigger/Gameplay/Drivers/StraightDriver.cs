using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    class StraightDriver : Driver
    {
        public StraightDriver(Vector2 unit, Board board)
            : base(unit, board)
        {
        }

        public override void UpdateMovement(IMover mh, EntityState state)
        {
            if (state == EntityState.Moving) return;
            var dirVec = new Vector2(mh.Direction.X * Unit.X, mh.Direction.Y * Unit.Y);

            Tile destTile = PosToTile(dirVec + mh.Position);
            RaiseMove(destTile);
        }
    }
}
