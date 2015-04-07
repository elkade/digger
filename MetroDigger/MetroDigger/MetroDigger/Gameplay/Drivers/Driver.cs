using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    public abstract class Driver : IDriver
    {
        private readonly DynamicEntity _entity;
        private readonly Vector2 _unit;
        private readonly Tile[,] _board;

        public Driver(DynamicEntity entity, Vector2 unit ,Tile[,] board)
        {
            _entity = entity;
            _unit = unit;
            _board = board;
        }

        public DynamicEntity Entity
        {
            get { return _entity; }
        }

        public Vector2 Unit
        {
            get { return _unit; }
        }

        public Tile[,] Board
        {
            get { return _board; }
        }

        public abstract void UpdateMovement();

        protected Tile PosToTile(Vector2 dirVec)
        {
            int x = (int)(dirVec.X / Tile.Width);
            int y = (int)(dirVec.Y / Tile.Height);

            if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
                return null;
            return _board[x, y];
        }

    }
}
