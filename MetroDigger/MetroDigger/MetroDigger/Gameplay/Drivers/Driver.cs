using System;
using MetroDigger.Gameplay.Entities;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Drivers
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla sterowników
    /// </summary>
    public abstract class Driver : IDriver
    {
        private readonly Vector2 _unit;
        private readonly Board _board;

        protected Driver(Vector2 unit ,Board board)
        {
            _unit = unit;
            _board = board;
        }

        protected Vector2 Unit
        {
            get { return _unit; }
        }

        protected Board Board
        {
            get { return _board; }
        }

        public abstract void UpdateMovement(IMover mover, EntityState state);

        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma wykonać strzał
        /// </summary>
        public event Action Shoot;
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma zacząć wiercić
        /// </summary>
        /// <remarks>Tile to kafelek, który ma zaostć wiercony</remarks>
        public event Action<Tile> Drill;
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma zacząć się poruszać
        /// </summary>
        /// <remarks>Tile to kafelek, w kierunku którego ma odbyć się ruch</remarks>
        public event Action<Tile> Move;
        /// <summary>
        /// Zdarzenie wywoływane gdy sterowany obiekt ma zacząć się obracać.
        /// </summary>
        /// <remarks>Vector2 - kierunek docelowy obrotu</remarks>
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

        protected Tile PosToTile(Vector2 dirVec)
        {
            int x = (int)Math.Floor(dirVec.X / Tile.Width);
            int y = (int)Math.Floor(dirVec.Y / Tile.Height);

            //if (x < 0 || x >= _board.GetLength(0) || y < 0 || y >= _board.GetLength(1))
            //    return null;
            return _board[x,y];
        }

    }
}
