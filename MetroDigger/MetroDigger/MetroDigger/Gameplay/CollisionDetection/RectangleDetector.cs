using MetroDigger.Gameplay.Entities;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public class RectangleDetector : CollisionDetector
    {
        public override bool CheckCollision(DynamicEntity entity1, DynamicEntity entity2)
        {
            Vector2 v1 = new Vector2(entity1.Width / 2, entity1.Height / 2);
            Vector2 v2 = new Vector2(entity2.Width / 2, entity2.Height / 2);
            BoundingBox bs1 = new BoundingBox(new Vector3(entity1.Position - v1,0),new Vector3(entity1.Position + v1,0));
            BoundingBox bs2 = new BoundingBox(new Vector3(entity2.Position - v2, 0), new Vector3(entity2.Position + v2, 0));
            return bs1.Intersects(bs2);
        }
    }
}
