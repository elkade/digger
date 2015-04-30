using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{

    public interface IDynamicEntity : IUpdateable, ICollideable, IDrawable
    {
    }
    public abstract class DynamicEntity : Entity, IDynamicEntity
    {
//todo to jest fatalnie

        protected float Angle = 0.0f;
        protected IMover MovementHandler;
        private IDriver _driver;

        protected Tile _occupiedTile;
        private EntityState _state;

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

        public IDriver Driver
        {
            get { return _driver; }
        }


        public EntityState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public float Width
        {
            get { return Sprite.Animation.FrameWidth*Sprite.Animation.Scale.X; }
        }

        public float Height
        {
            get { return Sprite.Animation.FrameHeight*Sprite.Animation.Scale.Y; }
        }

        public Vector2 Position { get; set; }
        public float MovementSpeed { get; set; }

        public bool IsToRemove { get; set; }


        public Tile OccupiedTile
        {
            get { return _occupiedTile; }
        }

        public override Vector2 Direction
        {
            get { return MovementHandler.Direction; }
            set { MovementHandler.Direction = value; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
        }

        public virtual void Update()
        {
            Driver.UpdateMovement(MovementHandler, _state);
            Angle = GetAngle(Direction);
            UpdateMoving();
        }


        public virtual void CollideWith(ICollideable character)
        {
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

        public virtual void StartMoving(Tile destinationTile)
        {
            if (MovementHandler.IsMoving || destinationTile == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destinationTile, MovementSpeed);
        }

        protected void UpdateMoving()
        {
            if (!MovementHandler.IsMoving)
                return;
            MovementHandler.Update();
            Position = MovementHandler.Position;
        }
    }

}