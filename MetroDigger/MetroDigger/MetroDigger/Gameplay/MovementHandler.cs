using System;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Logging;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay
{
    /// <summary>
    /// Narzędzie zmieniające położenie obiektu zgodnie z określonymi parametrami.
    /// </summary>
    public interface IMover
    {
        /// <summary>
        /// Kafelek startowy ruchu
        /// </summary>
        Tile StartTile { get; }
        /// <summary>
        /// Kafelek końcowy ruchu
        /// </summary>
        Tile EndTile { get; }
        /// <summary>
        /// Aktualna rzeczywista pozycja obiektu poruszającego się
        /// </summary>
        Vector2 Position { get; }
        /// <summary>
        /// Kierunek obiektu poruszającego się
        /// </summary>
        Vector2 Direction { get; set; }
        /// <summary>
        /// Określa, czy obiekt obecnie się porusza
        /// </summary>
        bool IsMoving { get; }
        /// <summary>
        /// Resetue parametry ruchu do stanu pocatkowego
        /// </summary>
        /// <param name="firstTile">Początkowy kafelek obiektu</param>
        /// <param name="firstDirection">Początkowy kierunek obiektu</param>
        void Reset(Tile firstTile, Vector2 firstDirection);
        /// <summary>
        /// Rozpoczyna ruch obiektu
        /// </summary>
        /// <param name="startTile">Kafelek startowy ruchu</param>
        /// <param name="endTile">Kafelek końcowy ruchu</param>
        /// <param name="speed">Prędkość ruchu</param>
        void MakeMove(Tile startTile, Tile endTile, float speed);
        /// <summary>
        /// Aktualizuje rozpoczęty ruch
        /// </summary>
        void Update();
        /// <summary>
        /// Zdarzenie wywoływane w momencie rozpoczęcia ruchu
        /// </summary>
        event Action<IMover,Tile,Tile> Started;
        /// <summary>
        /// Zdarzenie wywoływane w momencie przekroczenia połowy trasy biezącego ruchu
        /// </summary>
        event Action<IMover, Tile, Tile> Halved;
        /// <summary>
        /// Zdarzenie wywoływane po zakończeniu bieżącego ruchu
        /// </summary>
        event Action<IMover, Tile, Tile> Finished;
    }
    /// <summary>
    /// Narzędzie służące do zmiany położenia obiektu w linni prostej pomiędzy kafelkami
    /// </summary>
    public class MovementHandler : IMover
    {
        private int _distance;

        private Vector2 _direction;

        private Vector2 _stepSize;

        private int _halfWay;

        private bool _isMoving;

        private int _stepCount;

        private Vector2 _position;

        private Tile _startTile;

        private Tile _endTile;

        public Tile StartTile
        {
            get { return _startTile; }
        }

        public Tile EndTile
        {
            get { return _endTile; }
        }

        public Vector2 Position
        {
            get { return _position; }
        }

        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        public bool IsMoving
        {
            get { return _isMoving; }
        }
        /// <summary>
        /// Tworzy nowy MovementHandler
        /// </summary>
        /// <param name="firstTile">Początkowe położenie obiektu</param>
        /// <param name="firstDirection">Początkowy kierunek obiektu</param>
        public MovementHandler(Tile firstTile, Vector2 firstDirection)
        {
            Reset(firstTile, firstDirection);
        }

        public void Reset(Tile firstTile, Vector2 firstDirection)
        {
            _startTile = firstTile;
            _direction = firstDirection;
            _position = _startTile.Position;
            _distance = 0;
            _stepCount = 0;
            _position = _startTile.Position;
            _halfWay = 0;
            _stepSize = Vector2.Zero;
            _isMoving = false;
        }

        public void MakeMove(Tile startTile, Tile endTile, float speed)
        {
            Logger.Log("Move started");
            _startTile = startTile;
            _endTile = endTile;
            Vector2 route = _endTile.Position - _startTile.Position;
            _direction = Vector2.Normalize(route);
            _distance = (int)(route.Length() / speed);
            _stepCount = 0;
            _position = _startTile.Position;
            _halfWay = _distance/2;
            _stepSize = Direction * speed;
            _isMoving = true;
            if (Started != null)
                Started(this, _startTile, _endTile);
        }

        public void Update()
        {
            if (_stepCount == _distance)
            {
                if (Finished != null)
                {
                    Finished(this, _startTile, _endTile);
                    _startTile = _endTile;
                }
                _isMoving = false;
            }
            else
            {
                if (_stepCount == _halfWay)
                {
                    if (Halved != null)
                        Halved(this, _startTile, _endTile);
                }
                _position += _stepSize;
                _stepCount++;
            }
        }

        public event Action<IMover,Tile,Tile> Started;
        public event Action<IMover, Tile, Tile> Halved;
        public event Action<IMover, Tile, Tile> Finished;
    }
}
