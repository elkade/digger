using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Effects
{
    public class Particle
    {
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

        private Texture2D Texture { get; set; } // The texture that will be drawn to represent the particle
        private Vector2 Position { get; set; } // The current position of the particle        
        private Vector2 Velocity { get; set; } // The speed of the particle at the current instance
        private float Angle { get; set; } // The current angle of rotation of the particle
        private float AngularVelocity { get; set; } // The speed that the angle is changing
        private Color Color { get; set; } // The color of the particle
        private float Size { get; set; } // The size of the particle
        public int Ttl { get; private set; } // The 'time to live' of the particle

        public void Update()
        {
            Ttl--;
            Position += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            var origin = new Vector2((float)Texture.Width/2, (float)Texture.Height/2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}