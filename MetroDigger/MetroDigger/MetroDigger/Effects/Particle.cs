using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Effects
{
    /// <summary>
    /// Cząstka będąca częścią animacji rozpraszających się cząstek.
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// Tworzy nową czastkę.
        /// </summary>
        /// <param name="texture">Tekstura zawierająca cząstkę.</param>
        /// <param name="position">Miejsce wyświetlenia cząstki na ekranie.</param>
        /// <param name="velocity">Prędkośc, z jaką ma się przesuwać cząstka.</param>
        /// <param name="angle">kierunek ruchu cząstki.</param>
        /// <param name="angularVelocity">prędkość kątowa</param>
        /// <param name="color">kolor cząstki</param>
        /// <param name="size">rozmiar cząstki</param>
        /// <param name="ttl">czas zycia cząstki</param>
        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            Ttl = ttl;
        }

        private Texture2D Texture { get; set; }
        private Vector2 Position { get; set; }     
        private Vector2 Velocity { get; set; }
        private float Angle { get; set; }
        private float AngularVelocity { get; set; }
        private Color Color { get; set; }
        private float Size { get; set; }
        /// <summary>
        /// Time to live - liczba updatów, jaka pozostała cząstce do momentu, aż przestanie być wyświetlana
        /// </summary>
        public int Ttl { get; private set; }
        /// <summary>
        /// Aktualizuje kąt, położenie i czas życia cząstki
        /// </summary>
        public void Update()
        {
            Ttl--;
            Position += Velocity;
            Angle += AngularVelocity;
        }
        /// <summary>
        /// Rysuje cząstkę na ekranie
        /// </summary>
        /// <param name="spriteBatch">obiekt XNA służący do ryzowania.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            var origin = new Vector2((float)Texture.Width/2, (float)Texture.Height/2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}