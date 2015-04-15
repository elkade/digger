using System;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    public abstract class Driver : IDriver
    {
        private readonly Vector2 _unit;
        private readonly Tile[,] _board;

        public Driver(Vector2 unit ,Tile[,] board)
        {
            _unit = unit;
            _board = board;
        }

        public Vector2 Unit
        {
            get { return _unit; }
        }

        public Tile[,] Board
        {
            get { return _board; }
        }

        public abstract void UpdateMovement(MovementHandler mh, EntityState state);

        public event Action Shoot;
        public event Action<Tile> Drill;
        public event Action<Tile> Move;

        protected void RaiseShoot()
        {
            if (Shoot != null)
                Shoot();
        }

        protected void RaiseDrill(Tile destination)
        {
            if (Drill != null)
                Drill(destination);
        }

        protected void RaiseMove(Tile destination)
        {
            if (Move != null)
                Move(destination);
        }

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
