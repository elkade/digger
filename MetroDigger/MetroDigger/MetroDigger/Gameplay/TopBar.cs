using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.GameObjects
{
    class TopBar
    {
        private int _score;

        private string _scoreString = "score: ";
        private string _hpString = "hp: ";
        private string _energyString = "energy: ";
        private int _hp;
        private int _energy;

        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = MediaManager.Instance.TopBarFont;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2f);

            spriteBatch.DrawString(font, _hpString + _hp, new Vector2(50, 50), Color.White, 0,
                       origin, 2, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, _scoreString + _score, new Vector2(200, 50), Color.White, 0,
                                   origin, 2, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, _energyString + _energy, new Vector2(450, 50), Color.White, 0,
                       origin, 2, SpriteEffects.None, 0);
        }

        public void Update(int hp, int score, int energy)
        {
            _score = score;
            _hp = hp;
            _energy = energy;
        }
    }
}
