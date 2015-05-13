using System.Collections;
using System.Collections.Generic;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay
{
    /// <summary>
    /// Plansza gry sk³adaj¹ca siê z siatki dostêpnych kafelków i otoczki buforowej.
    /// </summary>
    public class Board : IEnumerable<Tile>
    {
        private readonly int _height;
        private readonly Tile[,] _tiles;
        private readonly int _width;
        /// <summary>
        /// Tworzy now¹ planszê
        /// </summary>
        /// <param name="width">Szerokoœæ dostêpnego obszaru planszy</param>
        /// <param name="height">Wysokoœæ dostêpnego obszaru planszy</param>
        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            _tiles = new Tile[width + 2, height + 2];
            for (int i = 0; i < width + 2; i++)
            {
                for (int j = 0; j < height + 2; j++)
                {
                    _tiles[i, j] = new Tile(i - 1, j - 1, new Buffer());
                }
            }
        }
        /// <summary>
        /// Indeksator kafelków na planszy
        /// </summary>
        /// <param name="x">Indeks X od 0 do Width - 1</param>
        /// <param name="y">Index Y od 0 do Heigth -1</param>
        /// <returns>Kafelek o podanych wspó³rzêdnych</returns>
        public Tile this[int x, int y]
        {
            get
            {
                if (x + 1 < 0 || x > _width || y + 1 < 0 || y > _height)
                    return null;
                return _tiles[x + 1, y + 1];
            }
            set { _tiles[x + 1, y + 1] = value; }
        }
        /// <summary>
        /// Kafelek startowy dla gracza
        /// </summary>
        public Tile StartTile { get; set; }
        /// <summary>
        /// Implementacja IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Tile> GetEnumerator()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    yield return this[i, j];
                }
            }
        }
        /// <summary>
        /// Implementacja IEnumerable
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Liczba kafelków w danym wymiarze
        /// </summary>
        /// <param name="p0">Wymiar</param>
        /// <returns>Wielkoœæ wymiaru</returns>
        public int GetLength(int p0)
        {
            if (p0 == 0)
                return _width;
            return _height;
        }
        /// <summary>
        /// Szerokoœæ planszy
        /// </summary>
        public int Width { get { return _width; } }
        /// <summary>
        /// Wysokoœæ planszy
        /// </summary>
        public int Height { get { return _height; } }
    }
}