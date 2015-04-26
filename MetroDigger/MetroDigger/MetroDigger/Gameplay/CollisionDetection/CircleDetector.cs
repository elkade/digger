using System;
using MetroDigger.Gameplay.Entities;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.CollisionDetection
{
    public class CircleDetector : CollisionDetector
    {
        public override bool CheckCollision(DynamicEntity entity1, DynamicEntity entity2)
        {
            BoundingSphere bs1 = new BoundingSphere(new Vector3(entity1.Position, 0), Math.Max(0.75f * entity1.Height, 0.75f * entity1.Width) / 2);
            BoundingSphere bs2 = new BoundingSphere(new Vector3(entity2.Position, 0), Math.Max(0.75f * entity2.Height, 0.75f * entity2.Width) / 2);
            return bs1.Intersects(bs2);
        }
    }
}
