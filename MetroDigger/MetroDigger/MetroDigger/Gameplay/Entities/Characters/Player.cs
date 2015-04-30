using System;
using MetroDigger.Effects;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Others;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Player : Character, ICollector, IDriller, IShooter
    {
        private MediaManager _grc;

        private int _score;

        private Tile StartTile;

        public Player(IDriver driver, Tile occupiedTile)
            : base(driver, 5f, occupiedTile, new Vector2(0,1))
        {
            PowerCellCount = 0;
            HasDrill = false;
            _grc = MediaManager.Instance;
            Direction = new Vector2(0, 1);
            _occupiedTile = occupiedTile;
            Position = OccupiedTile.Position;
            LoadContent();
            Sprite.PlayAnimation(Animations[0]);
            ParticleEngine = new ParticleEngine(_grc.DrillingPracticles, Position);
            StartTile = _occupiedTile;
            Aggressiveness = Aggressiveness.Player;
            MovementHandler.Finished += (handler, tile1, tile2) => RaiseVisited(tile1, tile2);
            Driver.Shoot += RaiseShoot;
        }
        private void LoadContent()
        {
            Animations = new[]
            {
                new Animation(_grc.PlayerIdle, 1f, true, 300, MediaManager.Instance.Scale),
                new Animation(_grc.PlayerWithDrill, 1f, true, 300, MediaManager.Instance.Scale),
            };
            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (State==EntityState.Drilling)
                    RaiseDrilled(tile2);
            };
        }


        public event Action<IShooter, Bullet> Shoot;

        private void RaiseShoot()
        {
            if (PowerCellCount <= 0) return;

            Shoot(this, null); //TODO
        }

        public event Action<IDriller, Tile> Drilled;

        protected void RaiseDrilled(Tile tile)
        {
            if (Drilled != null)
                Drilled(this, tile);
        }
        public override void Update()
        {
            ParticleEngine.EmitterLocation = Position;
            ParticleEngine.Update();
            base.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (base.State == EntityState.Drilling)
                ParticleEngine.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }
        public void StartShooting()
        {
            if (PowerCellCount <= 0) return;
            RaiseShoot();
            PowerCellCount--;
        }
        public int PowerCellCount { get; set; }
        public int LivesCount { get; set; }

        public int Score
        {
            get { return _score; }
            set
            {
                if (value >= 10000 && _score < 10000)//trzeba pamiętać żeby przy wczytywaniu najpierw robić score a później życia
                    LivesCount++;
                _score = value;
            }
        }

        public void Reset()
        {
            LivesCount--;
            State = EntityState.Idle;
            MovementHandler.Reset(StartTile, new Vector2(0,1));
            _occupiedTile = StartTile;
            Position = StartTile.Position;
        }

        public override void Harm()
        {
            Reset();
        }
        private void RaiseVisited(Tile tile1, Tile tile2)
        {
            if (Visited != null)
                Visited(this, tile1, tile2);
        }
        public event Action<ICollector, Tile, Tile> Visited;


    }

}
