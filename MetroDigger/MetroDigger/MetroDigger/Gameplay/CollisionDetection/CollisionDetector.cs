using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public abstract class CollisionDetector : ICollisionDetector
    {
        public abstract bool CheckCollision(DynamicEntity entity1, DynamicEntity entity2);
    }
}
