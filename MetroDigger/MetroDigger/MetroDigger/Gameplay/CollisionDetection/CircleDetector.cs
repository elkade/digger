using System;
using MetroDigger.Gameplay.Abstract;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.CollisionDetection
{
    /// <summary>
    /// Detektor kolizji, który sprawdza przecięcia koła otaczającego obiekt
    /// </summary>
    public class CircleDetector : CollisionDetector
    {        
        /// <summary>
        /// Sprawdza, czy zachodzi kolizja pomiędzy dwoma obiektami
        /// </summary>
        /// <param name="entity1">Obiekt, którego kolizja ma zostać sprawdzona</param>
        /// <param name="entity2">Obiekt, którego kolizja ma zostać sprawdzona</param>
        /// <returns></returns>
        public override bool CheckCollision(ICollideable entity1, ICollideable entity2)
        {

            BoundingSphere bs1 = new BoundingSphere(new Vector3(entity1.Position, 0), Math.Max(0.75f * entity1.Height, 0.75f * entity1.Width) / 2);
            BoundingSphere bs2 = new BoundingSphere(new Vector3(entity2.Position, 0), Math.Max(0.75f * entity2.Height, 0.75f * entity2.Width) / 2);
            return bs1.Intersects(bs2);
        }
    }
}
