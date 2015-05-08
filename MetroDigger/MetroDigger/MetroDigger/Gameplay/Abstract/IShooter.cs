using System;

namespace MetroDigger.Gameplay.Abstract
{
    public interface IShooter : IDynamicEntity
    {
        void StartShooting();
        event Action<IShooter> Shoot;
    }
}
