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

        protected Character(IDriver driver, float movementSpeed, Tile firstTile, Vector2 firstDirection)
            : base(driver, firstTile, firstDirection)
        {
            IsToRemove = false;
            MovementSpeed = movementSpeed;
            Driver.Drill += tile =>
            {
                if (HasDrill) StartDrilling(tile);
                else MovementHandler.Direction = Vector2.Normalize(tile.Position - OccupiedTile.Position);
            };
            Driver.Move += StartMoving;
            Driver.Turn += vector2 =>
            {
                MovementHandler.Direction = vector2;
            };
            Aggressiveness = Aggressiveness.None;
        }

        protected ParticleEngine ParticleEngine;

        public virtual void StartDrilling(Tile destination)
        {
            if (MovementHandler.IsMoving || destination == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destination, MovementSpeed / 3f);
            State = EntityState.Drilling;
        }

        public bool HasDrill
        {
            get { return _hasDrill; }
            set
            {
                _hasDrill = value;
                if (_hasDrill && Animations != null)
                    Sprite.PlayAnimation(Animations[1]);
            }
        }

        public int PowerCellCount { get; set; }

    }
    public enum EntityState
    {
        Idle,
        Drilling,
        Moving,
        StartingMoving
    }

    public enum Aggressiveness
    {
        None,
        Player,
        Enemy,
        All
    }
}
