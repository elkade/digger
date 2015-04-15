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

        public int Score = 0;

        public Player(IDriver driver, Tile occupiedTile)
            : base(driver, 5f)
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
                new Animation(_grc.PlayerIdle, 1f, true, 300),
                new Animation(_grc.PlayerWithDrill, 1f, true, 300),
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
            RaiseShoot();
            PowerCellCount--;

        }
        public int PowerCellCount { get; set; }
        public int LivesCount { get; set; }
    }

}
