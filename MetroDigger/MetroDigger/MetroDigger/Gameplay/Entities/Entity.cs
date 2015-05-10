using System;
using MetroDigger.Effects;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = MetroDigger.Gameplay.Abstract.IDrawable;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class Entity : IDrawable
    {
        private MediaManager _mm;

        protected AnimationPlayer AnimationPlayer;
        private Vector2 _direction;

        public virtual Vector2 Direction
        {
            get { return _direction; }
            protected set { _direction = value; }
        }

        protected Entity()
        {
            Mm = MediaManager.Instance;
            _direction = new Vector2(1, 0);
            AnimationPlayer = new AnimationPlayer();
        }

        protected float GetAngle(Vector2 vector)
        {
            return (float)(Math.Atan2(vector.Y, vector.X) - Math.PI/2);
        }

        protected int Value { private get; set; }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public Vector2 Position { get; set; }

        public int ZIndex { get; set; }

        protected MediaManager Mm
        {
            get { return _mm; }
            set { _mm = value; }
        }
    }
}
