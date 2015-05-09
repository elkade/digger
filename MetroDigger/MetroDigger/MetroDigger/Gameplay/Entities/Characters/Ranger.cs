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
    public class Ranger : DynamicEntity, ICollector, IDriller, IShooter
    {
        private readonly MediaManager _grc;

        public Ranger(IDriver driver, Tile occupiedTile, bool hasDrill, int energyCells)
            : base(driver, occupiedTile, new Vector2(0, -1), 5f)
        {
            _grc = MediaManager.Instance;
            MovementHandler.Direction = new Vector2(0, 1);
            OccupiedTile = occupiedTile;
            Position = OccupiedTile.Position;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Ranger"));
            ParticleEngine = new ParticleEngine(_grc.DrillingPracticles, Position);
            Aggressiveness = Aggressiveness.Enemy;
            MovementHandler.Finished += (handler, tile1, tile2) => RaiseVisited(tile1, tile2);
            IsWaterProof = false;
            PowerCellsCount = energyCells;
            HasDrill = hasDrill;
            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (State == EntityState.Drilling)
                    RaiseDrilled(tile1, tile2);
            };
            Value = 500;
            Driver.Shoot += StartShooting;
        }

        public event Action<IDriller, Tile, Tile> Drilled;

        private void RaiseDrilled(Tile tile1, Tile tile2)
        {
            if (Drilled != null)
                Drilled(this, tile1, tile2);
        }

        public override void Update()
        {
            ParticleEngine.EmitterLocation = Position;
            ParticleEngine.Update();
            base.Update();
        }

        public override Vector2 Direction
        {
            get { return MovementHandler.Direction; }
            protected set { MovementHandler.Direction = value; }
        }

        public override bool HasDrill
        {
            get { return base.HasDrill; }
            set
            {
                base.HasDrill = value;
                if (value)
                {
                    AnimationPlayer.PlayAnimation(Mm.GetDynamicAnimation("Miner"));
                }
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (State == EntityState.Drilling)
                ParticleEngine.Draw(spriteBatch);
            base.Draw(gameTime, spriteBatch);
        }
        private void RaiseVisited(Tile tile1, Tile tile2)
        {
            if (Visited != null)
                Visited(this, tile1, tile2);
        }
        public event Action<ICollector, Tile, Tile> Visited;
        public void StartShooting()
        {
            if (PowerCellsCount <= 0 || State != EntityState.Idle) return;
            RaiseShoot();
            PowerCellsCount--;
        }

        private void RaiseShoot()
        {
            Shoot(this);
        }


        public event Action<IShooter> Shoot;
    }

}
