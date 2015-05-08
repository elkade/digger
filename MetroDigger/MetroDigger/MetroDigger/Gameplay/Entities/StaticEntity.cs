using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class StaticEntity : Entity
    {
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None,Color.White);
        }
    }
}
