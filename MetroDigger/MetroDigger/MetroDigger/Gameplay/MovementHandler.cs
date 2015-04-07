using System;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay
{
    public class MovementHandler
    {
        private int _distance;

        private Vector2 _endPoint;

        private Vector2 _direction;

        private Vector2 _startPoint;

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
            set { _position = value; }
        }

        public Vector2 Direction
        {
            get { return _direction; }
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set { _isMoving = value; }
        }

        public void MakeMove(Tile startTile, Tile endTile, float speed)
        {
            _startTile = startTile;
            _endTile = endTile;
            _startPoint = startTile.Position;
            _endPoint = endTile.Position;
            Vector2 route = _endPoint - _startPoint;
            _direction = Vector2.Normalize(route);
            _distance = (int)(route.Length() / speed);
            _stepCount = 0;
            _position = _startPoint;
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
                if(Finished!=null)
                    Finished(this, _startTile, _endTile);
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

        public event Action<MovementHandler,Tile,Tile> Started;
        public event Action<MovementHandler, Tile, Tile> Halved;
        public event Action<MovementHandler, Tile, Tile> Finished;
    }
}
