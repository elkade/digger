using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public interface ICollisionDetector
    {
        bool CheckCollision(DynamicEntity entity1, DynamicEntity entity2);
    }
}
