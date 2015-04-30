using MetroDigger.Effects;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Stone : Character
    {
        private MediaManager _grc;

        public Stone(IDriver driver, Tile occupiedTile)
            : base(driver, 5f, occupiedTile, new Vector2(0, -1))
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
            Aggressiveness = Aggressiveness.None;
        }
        private void LoadContent()
        {
            Animations = new[]
            {
                new Animation(_grc.Stone, 1f, true, 300, MediaManager.Instance.Scale),
            };
            MovementHandler.Finished += (handler, tile1, tile2) =>
            {
                Aggressiveness = Aggressiveness.None;
                State = EntityState.Idle;
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
            Sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White);

        }

        public int PowerCellCount { get; set; }
        public override void StartMoving(Tile destinationTile)
        {
            if (MovementHandler.Direction == Level.GravityVector)
                Aggressiveness = Aggressiveness.All;
            base.StartMoving(destinationTile);
        }

        public override void CollideWith(ICollideable character)
        {
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
