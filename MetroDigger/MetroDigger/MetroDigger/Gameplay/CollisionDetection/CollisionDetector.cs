using MetroDigger.Gameplay.Abstract;

namespace MetroDigger.Gameplay.CollisionDetection
{
    /// <summary>
    /// Abstrakcyjny detektor kolizji
    /// </summary>
    public abstract class CollisionDetector : ICollisionDetector
    {
        /// <summary>
        /// Sprawdza, czy zachodzi kolizja pomiędzy dwoma obiektami
        /// </summary>
        /// <param name="entity1">Obiekt, którego kolizja ma zostać sprawdzona</param>
        /// <param name="entity2">Obiekt, którego kolizja ma zostać sprawdzona</param>
        /// <returns></returns>
        public abstract bool CheckCollision(ICollideable entity1, ICollideable entity2);
    }
}
