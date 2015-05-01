using System;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    public abstract class Driver : IDriver
    {
        private readonly Vector2 _unit;
        private readonly Board _board;

        public Driver(Vector2 unit ,Board board)
        {
            _unit = unit;
            _board = board;
        }

        public Vector2 Unit
        {
            get { return _unit; }
        }

        public Board Board
        {
            get { return _board; }
        }

        public abstract void UpdateMovement(IMover mh, EntityState state);

        public event Action Shoot;
        public event Action<Tile> Drill;
        public event Action<Tile> Move;
        public event Action<Vector2> Turn;

        protected void RaiseTurn(Vector2 direction)
        {
            if (Turn != null)
                Turn(direction);
        }

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

        public Tile PosToTile(Vector2 dirVec)
        {
            int x = (int)Math.Floor(dirVec.X / Tile.Width);
            int y = (int)Math.Floor(dirVec.Y / Tile.Height);

            //if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
            //    return null;
            return _board[x,y];
        }

    }
}
