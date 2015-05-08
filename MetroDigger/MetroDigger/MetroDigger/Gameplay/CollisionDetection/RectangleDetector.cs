using MetroDigger.Gameplay.Abstract;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public class RectangleDetector : CollisionDetector
    {
        public override bool CheckCollision(IDynamicEntity entity1, IDynamicEntity entity2)
        {
            if (entity1 == null || entity2 == null)
                return false;
            Vector2 v1 = new Vector2(0.75f * entity1.Width / 2, 0.75f * entity1.Height / 2);
            Vector2 v2 = new Vector2(0.75f * entity2.Width / 2, 0.75f * entity2.Height / 2);
            BoundingBox bs1 = new BoundingBox(new Vector3(entity1.Position - v1,0),new Vector3(entity1.Position + v1,0));
            BoundingBox bs2 = new BoundingBox(new Vector3(entity2.Position - v2, 0), new Vector3(entity2.Position + v2, 0));
            return bs1.Intersects(bs2);
        }
    }
}
