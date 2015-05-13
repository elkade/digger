using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay
{
    /// <summary>
    /// Reprezentuje górny pasek stanu gry zawierający liczbę żyć bohatera, liczbę baterii oraz wynik
    /// </summary>
    class TopBar
    {
        private int _score;

        private const string ScoreString = "score: ";
        private const string HpString = "hp: ";
        private const string EnergyString = "energy: ";
        private int _hp;
        private int _energy;
        /// <summary>
        /// Rysuje na ekranie elementy pasku
        /// </summary>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = MediaManager.Instance.TopBarFont;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2f);

            spriteBatch.DrawString(font, HpString + _hp, new Vector2(50, 50), Color.White, 0,
                       origin, 2, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, ScoreString + _score, new Vector2(200, 50), Color.White, 0,
                                   origin, 2, SpriteEffects.None, 0);

            spriteBatch.DrawString(font, EnergyString + _energy, new Vector2(450, 50), Color.White, 0,
                       origin, 2, SpriteEffects.None, 0);
        }
        /// <summary>
        /// Aktualizuje stan pasku
        /// </summary>
        /// <param name="hp">liczba żyć głównego bohatera</param>
        /// <param name="score">wynik gracza</param>
        /// <param name="energy">liczba baterii głównego bohatera</param>
        public void Update(int hp, int score, int energy)
        {
            _score = score;
            _hp = hp;
            _energy = energy;
        }
    }
}
