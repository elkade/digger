using MetroDigger.Gameplay.Abstract;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public abstract class CollisionDetector : ICollisionDetector
    {
        public abstract bool CheckCollision(IDynamicEntity entity1, IDynamicEntity entity2);
    }
}
