using System;
using MetroDigger.Effects;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Tiles;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public abstract class Character : DynamicEntity
    {
        private bool _hasDrill;

        protected Character(float moveSpeed) : base()
        {
            _moveSpeed = moveSpeed;
            MovementHandler.Finished += (handler, tile1, tile2) => RaiseVisited(tile1, tile2);
        }

        private void RaiseVisited(Tile tile1, Tile tile2)
        {
            if (Visited != null)
                Visited(this, tile1, tile2);
        }

        protected ParticleEngine ParticleEngine;

        public event Action<Character, Tile> Drilled;

        protected void RaiseDrilled(Tile tile)
        {
            if(Drilled!=null)
            Drilled(this, tile);
        }

        public override void StartDrilling(Tile destination)
        {
            if (MovementHandler.IsMoving || destination == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destination, _moveSpeed/3f);
            State = EntityState.Drilling;
        }

        public bool HasDrill
        {
            get { return _hasDrill; }
            set
            {
                _hasDrill = value;
                if (_hasDrill)
                    Sprite.PlayAnimation(Animations[1]);
            }
        }
        public event Action<Character, Bullet> Shoot;

        protected void RaiseShoot()
        {
            //if (PowerCellCount <= 0) return;//TODO
            Bullet bullet = new Bullet(this, Direction, Position, _moveSpeed * 2);
            bullet.Update();
            Shoot(this, bullet);
        }
        public override void StartShooting()
        {

        }

        public event Action<Character, Tile, Tile> Visited;
    }
    public enum EntityState
    {
        Idle,
        Drilling,
        Moving
    }

}
