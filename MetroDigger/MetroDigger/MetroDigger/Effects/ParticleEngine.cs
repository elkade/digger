using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Effects
{
    public class ParticleEngine
    {
        private readonly Random _random;
        public Vector2 EmitterLocation { get; set; }
        private readonly List<Particle> _particles;
        private readonly List<Texture2D> _textures;
        private readonly Color[] _colors =
        {
            new Color(0x66, 0x33, 0x00),
            new Color(0x2B, 0x0E, 0x00),
            new Color(0x29, 0x14, 0x00),
            new Color(0x60, 0x20, 0x00),
            new Color(0x14, 0x0A, 0x00)
        };

        public ParticleEngine(List<Texture2D> textures, Vector2 location)
        {
            EmitterLocation = location;
            _textures = textures;
            _particles = new List<Particle>();
            _random = new Random();
        }

        public void Update()
        {
            const int total = 5;

            for (int i = 0; i < total; i++)
            {
                _particles.Add(GenerateNewParticle());
            }

            for (int particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update();
                if (_particles[particle].Ttl <= 0)
                {
                    _particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = _textures[_random.Next(_textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(_random.NextDouble() * 2 - 1),
                                    1f * (float)(_random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);
            Color color = _colors[(new Random()).Next(_colors.Length-1)];
            float size = (float)_random.NextDouble();
            int ttl = 20 + _random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin();
            for (int index = 0; index < _particles.Count; index++)
            {
                _particles[index].Draw(spriteBatch);
            }
            //spriteBatch.End();
        }
    }
}