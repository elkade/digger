using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Terrains;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay
{
    interface ISpiller
    {
        int Spill(int x, int y);
    }
    class WaterSpiller : ISpiller
    {
        private readonly Board _board;
        private Vector2 _gravityVector;
        public WaterSpiller(Board board, Vector2 gravityVector)
        {
            _board = board;
            _gravityVector = gravityVector;
        }
        /// <summary>
        /// rozlewa wodę zgodnie z prawem grawitacji
        /// </summary>
        /// <param name="x">x pola z wodą garniczącego z polem pustym</param>
        /// <param name="y">y pola z wodą garniczącego z polem pustym</param>
        public int Spill(int x, int y)
        {
            bool[,] visited = new bool[_board.Width,_board.Height];
            int volumeOver = CheckWater(x, y, visited,true);
            if (volumeOver == 0) return 0;
            GetTilesToFlood(volumeOver, x, y);
            return DrawWater(visited);
        }

        private int DrawWater(bool[,] wateredPreviously)
        {
            int score = 0;
            foreach (var tile in _board)
            {
                if (tile.Accessibility == Accessibility.Water)
                {
                    Water w = tile.Terrain as Water;
                    tile.Terrain = new Water(w.Level, w.IsFull, _board[tile.X, tile.Y - 1].Accessibility != Accessibility.Free && _board[tile.X, tile.Y - 1].Accessibility != Accessibility.Buffer);
                }
                else
                {
                    if (wateredPreviously[tile.X, tile.Y])
                        if(tile.Metro is Tunnel)
                            if (!tile.Metro.IsCleared)
                            {
                                tile.Metro.IsCleared = true;
                                score += 150;
                            }
                }
            }
            return score;
        }

        private void GetTilesToFlood(int volumeOver, int x, int y)
        {
            bool[,] visited = new bool[_board.Width, _board.Height];
            volumeOver += CheckFree(x, y, visited);
            for (int i = _board.Height-1; i >=0; i--)
            {
                int c = 0;
                for (int j = _board.Width - 1; j >= 0; j--)
                    if (visited[j, i]) c++;
                if (c > volumeOver)
                {
                    int l = volumeOver/c;
                    for (int j = _board.Width - 1; j >= 0; j--)
                        if (visited[j, i])
                        {
                            _board[j, i].Terrain = new Water(l,volumeOver>0);
                            volumeOver--;
                        }
                }
                else
                {
                    for (int j = _board.Width - 1; j >= 0; j--)
                        if (visited[j, i])
                        {
                            _board[j, i].Terrain = new Water();
                            volumeOver--;
                        }
                }
                if (volumeOver <= 0)
                    return;
            }
        }

        private int CheckFree(int x, int y, bool[,] visited, bool wasUp = false)
        {
            if (x < 0 || x >= _board.Width || y < 0 || y >= _board.Height)
                return 0;
            if (visited[x, y])
                return 0;
            int s = 0;
            if (y>0&&visited[x, y - 1] && wasUp)
                return 0;
            switch (_board[x, y].Accessibility)
            {
                case Accessibility.Water:
                    if (wasUp) return 0;
                    s += (_board[x, y].Terrain as Water).IsFull ? 1 : 0;
                    _board[x, y].Terrain = new Free();
                    break;
                default:
                    if (_board[x, y].Accessibility != Accessibility.Free)
                        return 0;
                    break;
            }
            visited[x, y] = true;
            s += CheckFree(x, y - 1, visited, true);
            s += CheckFree(x - 1, y, visited,wasUp);
            s += CheckFree(x + 1, y, visited,wasUp);
            s += CheckFree(x, y + 1, visited,wasUp);
            return s;
        }

        private int CheckWater(int x, int y, bool[,] visited, bool isFirst = false)
        {
            if (x < 0 || x >= _board.Width || y < 0 || y >= _board.Height)
                return 0;
            if (visited[x, y])
                return 0;
            int s=0;
            if (_board[x, y].Accessibility != Accessibility.Water)
            {
                if (!isFirst || _board[x, y].Accessibility != Accessibility.Free)
                    return 0;
                s = 0;
            }
            else
                s += (_board[x, y].Terrain as Water).IsFull ? 1 : 0;
            visited[x, y] = true;
            _board[x,y].Terrain = new Free();
            s += CheckWater(x - 1, y, visited);
            s += CheckWater(x + 1, y, visited);
            s += CheckWater(x, y - 1, visited);
            return s;
        }
    }
}
