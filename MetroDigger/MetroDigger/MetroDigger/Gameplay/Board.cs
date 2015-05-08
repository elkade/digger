using System.Collections;
using System.Collections.Generic;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;

namespace MetroDigger.Gameplay
{
    public class Board : IEnumerable<Tile>
    {
        private readonly int _height;
        private readonly Tile[,] _tiles;
        private readonly int _width;

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

        public Tile StartTile { get; set; }

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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int GetLength(int p0)
        {
            if (p0 == 0)
                return _width;
            return _height;
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }
    }
}