using MetroDigger.Effects;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Stone : DynamicEntity
    {
        private MediaManager _grc;

        public Stone(IDriver driver, Tile occupiedTile)
            : base(driver, occupiedTile, new Vector2(0, -1), 5f)
        {
            PowerCellsCount = 0;
            HasDrill = true;
            _grc = MediaManager.Instance;
            MovementHandler.Direction = new Vector2(0, 1);
            OccupiedTile = occupiedTile;
            Position = OccupiedTile.Position;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Stone"));
            ParticleEngine = new ParticleEngine(_grc.DrillingPracticles, Position);
            Aggressiveness = Aggressiveness.None;
            MovementHandler.Finished += (handler, tile1, tile2) =>
            {
                Aggressiveness = Aggressiveness.None;
                State = EntityState.Idle;
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White);
        }

        protected override void StartMoving(Tile destinationTile)
        {
            if (MovementHandler.Direction == Level.GravityVector)
                Aggressiveness = Aggressiveness.All;
            base.StartMoving(destinationTile);
        }

        public override void CollideWith(ICollideable character)
        {
            if (character.OccupiedTile.Y > OccupiedTile.Y)
                base.CollideWith(character);
            if(character.OccupiedTile != OccupiedTile)//character przeżył kolizję więc podchodził od boku
                Shift(character.Direction);
        }

        private void Shift(Vector2 dir)
        {
            if (State != EntityState.Idle)
                return;
            MovementHandler.Direction = dir;
            Driver.UpdateMovement(MovementHandler,EntityState.StartingMoving);
        }


    }

}
