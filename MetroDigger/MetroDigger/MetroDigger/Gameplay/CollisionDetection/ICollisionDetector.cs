using MetroDigger.Gameplay.Abstract;

namespace MetroDigger.Gameplay.CollisionDetection
{
    /// <summary>
    /// Wykrywa kolizję pomiędzy obiektami
    /// </summary>
    public interface ICollisionDetector
    {
        /// <summary>
        /// Sprawdza, czy zachodzi kolizja pomiędzy dwoma obiektami
        /// </summary>
        /// <param name="entity1">Obiekt, którego kolizja ma zostać sprawdzona</param>
        /// <param name="entity2">Obiekt, którego kolizja ma zostać sprawdzona</param>
        /// <returns></returns>
        bool CheckCollision(ICollideable entity1, ICollideable entity2);
    }
}
