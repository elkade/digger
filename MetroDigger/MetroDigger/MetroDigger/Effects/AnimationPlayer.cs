using System;
using MetroDigger.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Effects
{
    /// <summary>
    /// Controls playback of an AnimationPlayer.
    /// </summary>
    public struct AnimationPlayer
    {

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;

        #region Properties

        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation
        {
            get { return _animation; }
        }
        Animation _animation;

        /// <summary>
        /// Gets or sets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return _frameIndex; }
            set { _frameIndex = value; }
        }
        int _frameIndex;

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight / 2.0f); }
        }

        #endregion

        private bool _orientation;
        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation, bool orientation=true)
        {
            _orientation = orientation;
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;

            // Start the new animation.
            this._animation = animation;
            this._frameIndex = 0;
            this.time = 0.0f;
        }

        /// <summary>
        /// Resets an animation (used for repeat a non-looping animation 
        /// </summary>
        /// <param name="animation">The animation to be reset</param>
        public void ResetAnimation(Animation animation)
        {
            this._frameIndex = 0;
            this.time = 0.0f;
            CustomIndex = 0;
        }

        ///// <summary>
        ///// Draws an specific _sprite frame(instead of animating)
        ///// </summary>
        //public void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        //{
        //    // Calculate the source rectangle of the current frame.
        //    Rectangle source = new Rectangle(_frameIndex * AnimationPlayer.FrameWidth, _shift, AnimationPlayer.FrameWidth, AnimationPlayer.FrameHeight + _shift);

        //    // Draw the current frame.
        //    spriteBatch.Draw(AnimationPlayer.Texture, position, source, Color.White, 0.0f, Origin, AnimationPlayer.Scale, spriteEffects, 0.0f);
        //}

        #region Draw
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, Color color, float angle = 0.0f)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                    _frameIndex = (_frameIndex + 1)%Animation.FrameCount;
            }
            if (!Animation.IsLooping)
                _frameIndex = CustomIndex;
            // Calculate the source rectangle of the current frame.
            Rectangle source;
            if(_orientation)
                source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight);
            else
                source = new Rectangle(0, FrameIndex * Animation.FrameHeight, Animation.FrameWidth, Animation.FrameHeight);

            // Draw the current frame.
            spriteBatch.Draw(Animation.Texture, position, source, color, angle, Origin, Animation.Scale, spriteEffects, 0.0f);
        }

        public int CustomIndex { get; set; }

        //public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        //{
        //    Draw(gameTime, spriteBatch, position, spriteEffects, Color.White);
        //}
        #endregion

    }
}
