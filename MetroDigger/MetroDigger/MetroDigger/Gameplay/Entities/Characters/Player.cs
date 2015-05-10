using System;
using MetroDigger.Effects;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Player : DynamicEntity, IDriller, IShooter, ICollector
    {
        private readonly MediaManager _grc;

        private readonly Tile _startTile;

        public Player(IDriver driver, Tile occupiedTile, Tile startTile)
            : base(driver, occupiedTile, new Vector2(0, 1), 5f)
        {
            PowerCellsCount = 0;
            HasDrill = false;
            _grc = MediaManager.Instance;
            MovementHandler.Direction = new Vector2(0, 1);
            OccupiedTile = occupiedTile;
            Position = OccupiedTile.Position;
            AnimationPlayer.PlayAnimation(Mm.GetDynamicAnimation("PlayerIdle"));
            ParticleEngine = new ParticleEngine(_grc.DrillingPracticles, Position);
            _startTile = startTile;
            Aggressiveness = Aggressiveness.Player;
            MovementHandler.Finished += (handler, tile1, tile2) => RaiseVisited(tile1, tile2);
            Driver.Shoot += StartShooting;
            ZIndex = 1000;
            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (State == EntityState.Drilling)
                    RaiseDrilled(tile1,tile2);
            };

        }

        public override Vector2 Direction
        {
            get { return MovementHandler.Direction; }
            protected set { MovementHandler.Direction = value; }
        }

        public event Action<IShooter> Shoot;

        private void RaiseShoot()
        {
            Shoot(this);
        }

        public event Action<IDriller, Tile, Tile> Drilled;

        private void RaiseDrilled(Tile tile1,Tile tile2)
        {
            if (Drilled != null)
                Drilled(this, tile1,tile2);
        }
        public override void Update()
        {
            ParticleEngine.EmitterLocation = Position;
            ParticleEngine.Update();
            base.Update();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (State == EntityState.Drilling)
                ParticleEngine.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }
        public void StartShooting()
        {
            if (/*PowerCellsCount <= 0 ||*/ State!=EntityState.Idle) return;
            RaiseShoot();
            PowerCellsCount--;
        }
        public int LivesCount { get; set; }

        public int Score { get; set; }

        private void Reset()
        {
            LivesCount--;
            State = EntityState.Idle;
            MovementHandler.Reset(_startTile, new Vector2(0,1));
            OccupiedTile = _startTile;
            Position = _startTile.Position;
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

        public override bool HasDrill
        {
            get { return base.HasDrill; }
            set
            {
                base.HasDrill = value;
                if(value)
                    AnimationPlayer.PlayAnimation(Mm.GetDynamicAnimation("PlayerWithDrill"));
            }
        }
    }

}
