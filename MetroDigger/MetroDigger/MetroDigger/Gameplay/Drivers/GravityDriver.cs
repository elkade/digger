using System;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    class GravityDriver : Driver
    {
        private readonly Vector2 _gravityDirection;

        public GravityDriver(Vector2 unit, Board board, Vector2 gravityDirection) : base(unit, board)
        {
            _gravityDirection = gravityDirection;
        }

        public override void UpdateMovement(IMover mh, EntityState state)
        {
            if (state == EntityState.Moving) return;
            if (state != EntityState.StartingMoving)
                mh.Direction = _gravityDirection;

            var dirVec = new Vector2(_gravityDirection.X * Unit.X, _gravityDirection.Y * Unit.Y);

            var tileBelow = PosToTile(dirVec + mh.Position);

            if(tileBelow.Accessibility!=Accessibility.Free && tileBelow.Accessibility != Accessibility.Water)
                dirVec = new Vector2(mh.Direction.X * Unit.X, mh.Direction.Y * Unit.Y);

            if (dirVec != Vector2.Zero && Vector2.Normalize(dirVec)!=new Vector2(0,-1))
            {
                Tile destTile = PosToTile(dirVec + mh.Position);
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
