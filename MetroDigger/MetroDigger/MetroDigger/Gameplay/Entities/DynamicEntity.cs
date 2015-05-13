using MetroDigger.Effects;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla dynamicznych obiektów gry
    /// </summary>
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
        /// <summary>
        /// Określa, czy obiekt posiada wiertło
        /// </summary>
        public virtual bool HasDrill { get; set; }
        /// <summary>
        /// Określa liczbę baterii posiadanych przez obiekt
        /// </summary>
        public int PowerCellsCount { get; set; }
        /// <summary>
        /// Określa szerokość obiektu
        /// </summary>
        public float Width
        {
            get { return AnimationPlayer.Animation.FrameWidth*AnimationPlayer.Animation.Scale.X; }
        }
        /// <summary>
        /// Określa wysokość obiektu
        /// </summary>
        public float Height
        {
            get { return AnimationPlayer.Animation.FrameHeight*AnimationPlayer.Animation.Scale.Y; }
        }
        /// <summary>
        /// Określa prędkość obiektu
        /// </summary>
        public float MovementSpeed { get; set; }
        /// <summary>
        /// Określa, czy obiekt ma być aktualizowany, czy usunięty
        /// </summary>
        public bool IsToRemove { get; set; }
        /// <summary>
        /// Zwraca Kafelek aktualnie zajmowany przez obiekt
        /// </summary>
        public Tile OccupiedTile
        {
            get { return _occupiedTile; }
            protected set { _occupiedTile = value; }
        }
        /// <summary>
        /// Rysuje obiekt na planszy
        /// </summary>
        /// <param name="gameTime">Czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
        }
        /// <summary>
        /// Aktualizuje stan obiektu
        /// </summary>
        public virtual void Update()
        {
            Driver.UpdateMovement(MovementHandler, _state);
            Angle = GetAngle(Direction);
            UpdateMoving();
        }
        /// <summary>
        /// Wywołuje zachowanie obiektu w momencie kolizji
        /// </summary>
        /// <param name="collideable">Obiekt, z którym zaszła kolizja.</param>
        public virtual void CollideWith(ICollideable collideable)
        {
            if (IsToRemove || collideable.IsToRemove)
                return;
            switch (Aggressiveness)
            {
                case Aggressiveness.All:
                    collideable.Harm();
                    break;
                case Aggressiveness.None:
                    return;
                case Aggressiveness.Player:
                    return;
                case Aggressiveness.Enemy:
                    if (collideable.Aggressiveness == Aggressiveness.Player)
                        collideable.Harm();
                    return;
            }
        }
        /// <summary>
        /// Określa to, jak obiekt reaguje na kolizję.
        /// </summary>
        public Aggressiveness Aggressiveness { get; set; }
        /// <summary>
        /// Metoda wywoływana w momencie, gdy obiekt zostaje raniony.
        /// </summary>
        public virtual void Harm()
        {
            IsToRemove = true;
        }
        /// <summary>
        /// Określa, czy obiekt może zostać raniony w kontakcie z wodą.
        /// </summary>
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
        /// <summary>
        /// Rozpoczyna wiercenie
        /// </summary>
        /// <param name="destination">Docelowy kafelek wiercenia.</param>
        public virtual void StartDrilling(Tile destination)
        {
            if (MovementHandler.IsMoving || destination == null)
                return;
            MovementHandler.MakeMove(_occupiedTile, destination, MovementSpeed/3f);
            State = EntityState.Drilling;
        }
        /// <summary>
        /// Wartość obiektu - liczba punktów zdobywanych przy jego zniszczeniu.
        /// </summary>
        public int Value { get; set; }
    }
    /// <summary>
    /// Stan, w którym znajduje się obiekt
    /// </summary>
    public enum EntityState
    {
        Idle,
        Drilling,
        Moving,
        StartingMoving
    }
    /// <summary>
    /// Zachowanie obiektu przy kolizji
    /// </summary>
    public enum Aggressiveness
    {
        None,
        Player,
        Enemy,
        All
    }
}