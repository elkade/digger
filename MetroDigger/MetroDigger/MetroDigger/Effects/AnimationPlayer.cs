using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Effects
{
    /// <summary>
    /// Kontroluje odtwarzanie animacji z klasy Animation
    /// </summary>
    public struct AnimationPlayer
    {
        private float _time;

        #region Properties

        /// <summary>
        /// Pobiera aktualnie odtwarzaną animację
        /// </summary>
        public Animation Animation
        {
            get { return _animation; }
        }
        Animation _animation;

        private int FrameIndex
        {
            get { return _frameIndex; }
        }
        int _frameIndex;

        private Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight / 2.0f); }
        }

        #endregion

        private bool _orientation;
        /// <summary>
        /// Rozpoczyna lub kontynuuje animację.
        /// </summary>
        public void PlayAnimation(Animation animation, bool orientation=true)
        {
            _orientation = orientation;
            if (Animation == animation)
                return;

            _animation = animation;
            _frameIndex = 0;
            _time = 0.0f;
        }
        /// <summary>
        /// Pozwala ręcznie ustawić numer aktualnie wyświetlanej klatki
        /// </summary>
        public int CustomIndex { private get; set; }

        /// <summary>
        /// Resetuje animację
        /// </summary>
        /// <param name="animation">Animacja, która ma byś zresetowana</param>
        public void ResetAnimation(Animation animation)
        {
            _frameIndex = 0;
            _time = 0.0f;
            CustomIndex = 0;
        }

        #region Draw
        /// <summary>
        /// Rysuje aktualnie odtwarzaną klatkę animacji
        /// </summary>
        /// <param name="gameTime">aktualny czas gry</param>
        /// <param name="spriteBatch">obiekt XNA służący do rysowania</param>
        /// <param name="position">położenie animacji na ekranie</param>
        /// <param name="spriteEffects">efekty sprita XNA</param>
        /// <param name="color">kolor animacji XNA</param>
        /// <param name="angle">kąt, pod jakim do osi pionowej wyświetlana jest animacja</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, Color color, float angle = 0.0f)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time > Animation.FrameTime)
            {
                _time -= Animation.FrameTime;
                if (Animation.IsLooping)
                    _frameIndex = (_frameIndex + 1)%Animation.FrameCount;
            }
            if (!Animation.IsLooping)
                _frameIndex = CustomIndex;
            Rectangle source;
            if(_orientation)
                source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight);
            else
                source = new Rectangle(0, FrameIndex * Animation.FrameHeight, Animation.FrameWidth, Animation.FrameHeight);
            spriteBatch.Draw(Animation.Texture, position, source, color, angle, Origin, Animation.Scale, spriteEffects, 0.0f);
        }
        #endregion

    }
}
