using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt, który może zostać narysowany na mapie.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Rysuje obiekt na mapie
        /// </summary>
        /// <param name="gameTime">Aktualny czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        /// <summary>
        /// Miejsce na ekranie, w którym ma zostać narysowany obiekt.
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// ZIndex obiektu
        /// </summary>
        int ZIndex { get; set; }
    }
}
