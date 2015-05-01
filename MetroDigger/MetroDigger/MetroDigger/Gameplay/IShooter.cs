using System;
using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay
{
    public interface IShooter : IDynamicEntity
    {
        void StartShooting();
        event Action<IShooter> Shoot;
    }
}
