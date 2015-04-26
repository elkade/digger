using MetroDigger.Effects;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Player : Character, ICollector
    {
        private MediaManager _grc;

        private int _score;

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
        public override void StartShooting()
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

        public void Reset(Tile startTile)
        {
            LivesCount--;
            State = EntityState.Idle;
            MovementHandler.Reset(startTile, new Vector2(0,1));
            _occupiedTile = startTile;
            Position = startTile.Position;
        }
    }

}
