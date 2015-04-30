using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class StaticEntity : Entity
    {

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            Sprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None,Color.White);
        }
    }
}
