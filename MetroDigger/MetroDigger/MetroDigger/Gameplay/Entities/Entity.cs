using System;
using MetroDigger.Effects;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = MetroDigger.Gameplay.Abstract.IDrawable;

namespace MetroDigger.Gameplay.Entities
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla obiektów, które mogą zostać narysowane na planszy
    /// </summary>
    public abstract class Entity : IDrawable
    {
        private MediaManager _mm;

        protected AnimationPlayer AnimationPlayer;
        private Vector2 _direction;

        /// <summary>
        /// Określa kierunek, w którym jest zwrócony obiekt
        /// </summary>
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
        /// <summary>
        /// Rysuje obiekt na planszy
        /// </summary>
        /// <param name="gameTime">Czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
        /// <summary>
        /// Położenie boiektu na ekranie
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// ZIndex obiektu
        /// </summary>
        public int ZIndex { get; set; }

        protected MediaManager Mm
        {
            get { return _mm; }
            set { _mm = value; }
        }
    }
}
