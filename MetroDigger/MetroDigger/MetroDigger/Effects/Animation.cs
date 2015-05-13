using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Effects
{
    /// <summary>
    /// Reprezentuje animowaną teksturę.
    /// </summary>
    /// <remarks>
    /// Dzieli obraz na klatki, które mogą być wyświetlane w kolejności.
    /// </remarks>
public class Animation
    {
        /// <summary>
        /// Wyświetlana tekstura; klatki są ustawione horyzontalnie. 
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }
        Texture2D texture;

        /// <summary>
        /// Czas trwania pokazu każdej klatki
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        /// <summary>
        /// Określa, czy po zakończeniu animacji powinna się ona odtworzyć ponownie
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;
        private readonly Vector2 _scale;

        /// <summary>
        /// Pobiera liczbę klatek animacji
        /// </summary>
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        /// <summary>
        /// Pobiera szerokość klatki animacji
        /// </summary>
        public int FrameWidth
        {
            get; private set;
        }

        /// <summary>
        /// Pobiera wysokość klatki animacji.
        /// </summary>
        public int FrameHeight { get; private set; }
        /// <summary>
        /// Określa w jakiej skali do wielkości pierwotnej ma byś wyświeetlany obraz
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
        }


        /// <summary>
        /// Tworzy nową animację
        /// </summary>
        /// <param name="texture">wyświetlane obrazy</param>
        /// <param name="frameTime">czas wyświetlania pojedynczej klatki</param>
        /// <param name="isLooping">czy animacja ma byś zapętlona</param>
        /// <param name="frameWidth">szerokość klatki</param>
        /// <param name="scale">skala w stosunku do wielkości pierwotnej obrazu</param>
        /// <param name="frameHeight">wysokość klatki</param>
      
        public Animation(Texture2D texture, float frameTime, bool isLooping, int frameWidth, Vector2 scale,int frameHeight=0)
        {
            this.texture = texture;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
            _scale = scale;
            if (frameWidth == 0)
                FrameWidth = texture.Width;
            else if (frameWidth > 0)
                FrameWidth = frameWidth;
            FrameHeight = frameHeight == 0 ? texture.Height : frameHeight;
        }
    }
}
