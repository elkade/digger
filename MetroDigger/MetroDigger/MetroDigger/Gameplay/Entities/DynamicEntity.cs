using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    public abstract class DynamicEntity : Entity
    {//todo to jest fatalnie
        public float Width { get { return Sprite.Animation.FrameWidth * Sprite.Animation.Scale.X; } }
        public float Height { get { return Sprite.Animation.FrameHeight * Sprite.Animation.Scale.Y; } }

        public IDriver Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public Vector2 Position;

        public float MoveSpeed { get { return _moveSpeed; } }

        protected MovementHandler MovementHandler;

        private EntityState _state;
        protected float _moveSpeed;

        public DynamicEntity(IDriver driver, Tile firstTile, Vector2 firstDirection)
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
        }

        public EntityState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public Tile OccupiedTile
        {
            get { return _occupiedTile; }
        }

        protected Tile _occupiedTile;

        public abstract void StartShooting();
        public abstract void StartDrilling(Tile destination);
        public virtual void StartMoving(Tile destinationTile)
        {
            if (MovementHandler.IsMoving || destinationTile == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destinationTile, _moveSpeed);
        }

        protected void UpdateMoving()
        {
            if (!MovementHandler.IsMoving)
                return;
            MovementHandler.Update();
            Position = MovementHandler.Position;
        }

        public override Vector2 Direction
        {
            get { return MovementHandler.Direction; }
            set { MovementHandler.Direction = value; }
        }

        protected float Angle = 0.0f;
        private IDriver _driver;

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
        }

        public virtual void Update()
        {
            Driver.UpdateMovement(MovementHandler,_state);
            Angle = GetAngle(Direction);
            UpdateMoving();
        }


    }
}
