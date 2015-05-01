using System;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay
{
    public interface IMover
    {
        Tile StartTile { get; }

        Tile EndTile { get; }

        Vector2 Position { get; }

        Vector2 Direction { get; set; }

        bool IsMoving { get; }
        
        void Reset(Tile firstTile, Vector2 firstDirection);

        void MakeMove(Tile startTile, Tile endTile, float speed);

        void Update();

        event Action<IMover,Tile,Tile> Started;
        event Action<IMover, Tile, Tile> Halved;
        event Action<IMover, Tile, Tile> Finished;
    }
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
