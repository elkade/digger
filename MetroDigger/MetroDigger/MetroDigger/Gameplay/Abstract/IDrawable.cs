using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Abstract
{
    public interface IDrawable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        Vector2 Position { get; set; }
        int ZIndex { get; set; }
    }
}
