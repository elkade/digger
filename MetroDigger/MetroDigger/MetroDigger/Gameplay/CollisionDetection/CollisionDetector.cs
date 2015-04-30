using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public abstract class CollisionDetector : ICollisionDetector
    {
        public abstract bool CheckCollision(IDynamicEntity entity1, IDynamicEntity entity2);
    }
}
