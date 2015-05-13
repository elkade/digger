using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, który mo¿e doznaæ kolizji z innym obiektem tego typu.
    /// </summary>
    public interface ICollideable : IBoardObject
    {
        /// <summary>
        /// Okreœla to, jak obiekt reaguje na kolizjê.
        /// </summary>
        Aggressiveness Aggressiveness { get; }
        /// <summary>
        /// Wywo³uje zachowanie obiektu w momencie kolizji
        /// </summary>
        /// <param name="collideable">Obiekt, z którym zasz³a kolizja.</param>
        void CollideWith(ICollideable collideable);
        /// <summary>
        /// Metoda wywo³ywana w momencie, gdy obiekt zostaje raniony.
        /// </summary>
        void Harm();
        /// <summary>
        /// Okreœla, czy obiekt mo¿e zostaæ raniony w kontakcie z wod¹.
        /// </summary>
        bool IsWaterProof { get; set; }
    }
}