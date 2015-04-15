using System;
using MetroDigger.Effects;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public abstract class Character : DynamicEntity
    {
        private bool _hasDrill;

        protected Character(IDriver driver, float moveSpeed, Tile firstTile, Vector2 firstDirection)
            : base(driver, firstTile, firstDirection)
        {
            IsToRemove = false;
            _moveSpeed = moveSpeed;
            MovementHandler.Finished += (handler, tile1, tile2) => RaiseVisited(tile1, tile2);
            Driver.Drill += StartDrilling;
            Driver.Move += StartMoving;
            Driver.Shoot += StartShooting;
            Driver.Turn += vector2 =>
            {
                MovementHandler.Direction = vector2;
                Direction = vector2;
            };
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

        public bool IsToRemove { get; set; }

        public event Action<Character, Bullet> Shoot;

        protected void RaiseShoot()
        {
            //if (PowerCellCount <= 0) return;//TODO

            Shoot(this, null);//TODO
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
