using MetroDigger.Gameplay.Entities;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public interface ICollisionDetector
    {
        bool CheckCollision(IDynamicEntity entity1, IDynamicEntity entity2);
    }
}
