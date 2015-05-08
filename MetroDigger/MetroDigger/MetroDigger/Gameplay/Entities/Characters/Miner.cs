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
    public class Miner : DynamicEntity, ICollector, IDriller
    {
        private readonly MediaManager _grc;

        public Miner(IDriver driver, Tile occupiedTile)
            : base(driver, occupiedTile, new Vector2(0,-1), 5f)
        {
            PowerCellsCount = 0;
            HasDrill = true;
            _grc = MediaManager.Instance;
            MovementHandler.Direction = new Vector2(0, 1);
            OccupiedTile = occupiedTile;
            Position = OccupiedTile.Position;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Miner"));
            ParticleEngine = new ParticleEngine(_grc.DrillingPracticles, Position);
            Value = 500;
            Aggressiveness = Aggressiveness.Enemy;
            MovementHandler.Finished += (handler, tile1, tile2) => RaiseVisited(tile1, tile2);
            IsWaterProof = true;
            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (State == EntityState.Drilling)
                    RaiseDrilled(tile2);
            };

        }
        public event Action<IDriller, Tile> Drilled;

        private void RaiseDrilled(Tile tile)
        {
            if (Drilled != null)
                Drilled(this, tile);
        }

        public override Vector2 Direction
        {
            get { return MovementHandler.Direction; }
            protected set { MovementHandler.Direction = value; }
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
        private void RaiseVisited(Tile tile1, Tile tile2)
        {
            if (Visited != null)
                Visited(this, tile1, tile2);
        }

        public event Action<ICollector, Tile, Tile> Visited;
    }

}
