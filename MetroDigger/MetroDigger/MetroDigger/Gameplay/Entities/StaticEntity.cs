using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla obiektów, które nie mogą się poruszać.
    /// Nie mają one określonej pozycji na mapie, a  należą do kafelków.
    /// </summary>
    public abstract class StaticEntity : Entity
    {
        /// <summary>
        /// Rysuje obiekt na planszy
        /// </summary>
        /// <param name="gameTime">Czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None,Color.White);
        }
    }
}
