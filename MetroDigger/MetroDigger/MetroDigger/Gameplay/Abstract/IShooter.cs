using System;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt zdolny do strzelania.
    /// </summary>
    public interface IShooter : IDynamicEntity
    {
        /// <summary>
        /// Rozpoczyna strzał
        /// </summary>
        void StartShooting();
        /// <summary>
        /// Zdarzenie wywoływane w momencie wykonania strzału
        /// </summary>
        event Action<IShooter> Shoot;
    }
}
