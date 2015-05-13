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
    /// <summary>
    /// Robot górniczy
    /// </summary>
    public class Miner : DynamicEntity, IDriller
    {
        private readonly MediaManager _grc;
        /// <summary>
        /// Tworzy nowego robota górniczego
        /// </summary>
        /// <param name="driver">sterownik, zgodnie z którym porusza się robot</param>
        /// <param name="occupiedTile">kafelek, w którym poczatkowo przebywa obiekt</param>
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
            IsWaterProof = true;
            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (State == EntityState.Drilling)
                    RaiseDrilled(tile1, tile2);
            };
        }
        public event Action<IDriller, Tile, Tile> Drilled;

        private void RaiseDrilled(Tile tile1, Tile tile2)
        {
            if (Drilled != null)
                Drilled(this, tile1, tile2);
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
    }

}
