using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, kt�ry mo�e dozna� kolizji z innym obiektem tego typu.
    /// </summary>
    public interface ICollideable : IBoardObject
    {
        /// <summary>
        /// Okre�la to, jak obiekt reaguje na kolizj�.
        /// </summary>
        Aggressiveness Aggressiveness { get; }
        /// <summary>
        /// Wywo�uje zachowanie obiektu w momencie kolizji
        /// </summary>
        /// <param name="collideable">Obiekt, z kt�rym zasz�a kolizja.</param>
        void CollideWith(ICollideable collideable);
        /// <summary>
        /// Metoda wywo�ywana w momencie, gdy obiekt zostaje raniony.
        /// </summary>
        void Harm();
        /// <summary>
        /// Okre�la, czy obiekt mo�e zosta� raniony w kontakcie z wod�.
        /// </summary>
        bool IsWaterProof { get; set; }
    }
}