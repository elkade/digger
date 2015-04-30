using MetroDigger.Effects;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Miner : Character, ICollector, IShooter
    {
        private MediaManager _grc;

        public int Score = 0;

        public Miner(IDriver driver, Tile occupiedTile)
            : base(driver, 5f, occupiedTile, new Vector2(0,-1))
        {
            PowerCellCount = 0;
            HasDrill = true;
            _grc = MediaManager.Instance;
            Direction = new Vector2(0, 1);
            _occupiedTile = occupiedTile;
            Position = OccupiedTile.Position;
            LoadContent();
            Sprite.PlayAnimation(Animations[0]);
            ParticleEngine = new ParticleEngine(_grc.DrillingPracticles, Position);
            Value = 500;
            Aggressiveness = Aggressiveness.Enemy;
        }
        private void LoadContent()
        {
            Animations = new[]
            {
                new Animation(_grc.Miner, 1f, true, 300, MediaManager.Instance.Scale),
            };
            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (State == EntityState.Drilling)
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
    }

}
