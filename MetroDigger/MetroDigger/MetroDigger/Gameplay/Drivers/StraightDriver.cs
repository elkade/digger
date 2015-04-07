using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    class StraightDriver : Driver
    {
        public StraightDriver(DynamicEntity entity, Vector2 unit, Tile[,] board) : base(entity, unit, board)
        {
        }

        public override void UpdateMovement()
        {
            if (Entity.State == EntityState.Moving)
            {
                Entity.Update();
            }
            else
            {
                var dirVec = new Vector2(Entity.Direction.X * Unit.X, Entity.Direction.Y * Unit.Y);

                Tile destTile = PosToTile(dirVec + Entity.Position);
                Entity.StartMoving(destTile);
            }
        }
    }
}
