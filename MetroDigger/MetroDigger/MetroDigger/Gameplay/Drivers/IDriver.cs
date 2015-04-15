using System;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;

namespace MetroDigger.Gameplay.Drivers
{
    public interface IDriver
    {
        void UpdateMovement(MovementHandler mh, EntityState state);
        event Action Shoot;
        event Action<Tile> Drill;
        event Action<Tile> Move;
    }
}
