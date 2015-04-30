using System;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Others;

namespace MetroDigger.Gameplay
{
    public interface IShooter : IDynamicEntity
    {
        void StartShooting();
        event Action<IShooter, Bullet> Shoot;
    }
}
