using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class Entity : IDrawable
    {
        protected Animation[] Animations;

        protected AnimationPlayer Sprite;
        private Vector2 _direction;

        public virtual Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        protected Entity()
        {
            _direction = new Vector2(1, 0);
            Sprite = new AnimationPlayer();
        }

        protected float GetAngle(Vector2 vector)
        {
            return (float)(Math.Atan2(vector.Y, vector.X) - Math.PI/2);
        }

        public int Value { get; set; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
