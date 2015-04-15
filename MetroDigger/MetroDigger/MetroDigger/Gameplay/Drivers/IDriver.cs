using System;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    public interface IDriver
    {
        void UpdateMovement(MovementHandler mh, EntityState state);
        event Action Shoot;
        event Action<Tile> Drill;
        event Action<Tile> Move;
        event Action<Vector2> Turn;
    }
}
