using MetroDigger.Effects;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class DynamicEntity : Entity, IDynamicEntity
    {
        private readonly IDriver _driver;
        protected float Angle;
        protected readonly IMover MovementHandler;
        protected ParticleEngine ParticleEngine;

        private Tile _occupiedTile;
        private EntityState _state;

        protected DynamicEntity(IDriver driver, Tile firstTile, Vector2 firstDirection, float movementSpeed)
        {
            MovementHandler = new MovementHandler(firstTile, firstDirection);
            MovementHandler.Started += (handler, tile1, tile2) => State = EntityState.Moving;
            MovementHandler.Finished += (handler, tile1, tile2) =>
            {
                _occupiedTile = tile2;
                State = EntityState.Idle;
            };

            _driver = driver;
            _driver.Move += StartMoving;
            IsWaterProof = true;
            IsToRemove = false;
            MovementSpeed = movementSpeed;
            Driver.Drill += tile =>
            {
                if (HasDrill) StartDrilling(tile);
                else MovementHandler.Direction = Vector2.Normalize(tile.Position - OccupiedTile.Position);
            };
            Driver.Move += StartMoving;
            Driver.Turn += vector2 => MovementHandler.Direction = vector2;
            Aggressiveness = Aggressiveness.None;
            Value = 0;
        }

        protected IDriver Driver
        {
            get { return _driver; }
        }

        protected EntityState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public virtual bool HasDrill { get; set; }

        public int PowerCellsCount { get; set; }

        public float Width
        {
            get { return AnimationPlayer.Animation.FrameWidth*AnimationPlayer.Animation.Scale.X; }
        }

        public float Height
        {
            get { return AnimationPlayer.Animation.FrameHeight*AnimationPlayer.Animation.Scale.Y; }
        }

        public float MovementSpeed { get; set; }

        public bool IsToRemove { get; set; }


        public Tile OccupiedTile
        {
            get { return _occupiedTile; }
            protected set { _occupiedTile = value; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
        }

        public virtual void Update()
        {
            Driver.UpdateMovement(MovementHandler, _state);
            Angle = GetAngle(Direction);
            UpdateMoving();
        }


        public virtual void CollideWith(ICollideable character)
        {
            if (IsToRemove || character.IsToRemove)
                return;
            switch (Aggressiveness)
            {
                case Aggressiveness.All:
                    character.Harm();
                    break;
                case Aggressiveness.None:
                    return;
                case Aggressiveness.Player:
                    return;
                case Aggressiveness.Enemy:
                    if (character.Aggressiveness == Aggressiveness.Player)
                        character.Harm();
                    return;
            }
        }

        public Aggressiveness Aggressiveness { get; set; }

        public virtual void Harm()
        {
            IsToRemove = true;
        }

        public bool IsWaterProof { get; set; }

        protected virtual void StartMoving(Tile destinationTile)
        {
            if (MovementHandler.IsMoving || destinationTile == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destinationTile, MovementSpeed);
        }

        private void UpdateMoving()
        {
            if (!MovementHandler.IsMoving)
                return;
            MovementHandler.Update();
            Position = MovementHandler.Position;
        }

        public virtual void StartDrilling(Tile destination)
        {
            if (MovementHandler.IsMoving || destination == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destination, MovementSpeed/3f);
            State = EntityState.Drilling;
        }

        public int Value { get; set; }
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