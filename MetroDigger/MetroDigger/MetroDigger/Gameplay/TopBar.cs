using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.GameObjects
{
    class TopBar
    {
        private int _score;

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = MediaManager.Instance.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2f);

            spriteBatch.DrawString(font, _score.ToString(), new Vector2(50, 50), Color.White, 0,
                                   origin, 2, SpriteEffects.None, 0);
        }

        public void Update(int score)
        {
            _score = score;
        }
    }
}
