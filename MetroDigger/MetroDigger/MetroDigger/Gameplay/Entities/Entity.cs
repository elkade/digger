using System;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class Entity
    {
        protected Animation[] Animations;

        protected AnimationPlayer Sprite;

        public Vector2 Direction;

        protected Entity()
        {
            Direction = new Vector2(1, 0);
            Sprite = new AnimationPlayer();
        }

        protected float GetAngle(Vector2 vector)
        {
            return (float)(Math.Atan2(vector.Y, vector.X) - Math.PI/2);
        }

    }
}
