using System;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class Bullet : Character
    {
        private readonly IShooter _shooter;
        public Bullet(IDriver driver, IShooter shooter)
            : base(driver, shooter.MovementSpeed * 2, shooter.OccupiedTile, shooter.Direction)
        {
            _occupiedTile = shooter.OccupiedTile;
            _shooter = shooter;
            var grc = MediaManager.Instance;
            Position = shooter.Position;
            Direction = shooter.Direction;
            Animations = new[]
            {
                new Animation(grc.RedBullet[0], 1f, false, 0, MediaManager.Instance.Scale),
                new Animation(grc.RedBullet[1], 1f, false, 0, MediaManager.Instance.Scale),
            };
            IsToRemove = false;
            MediaManager.Instance.PlaySound("laser");
            Sprite.PlayAnimation(Animations[1]);

            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (Hit != null)
                    Hit(this, tile2);
            };
            Aggressiveness = Aggressiveness.All;
        }

        public event Action<Bullet, Tile> Hit;

        public IShooter Shooter
        {
            get { return _shooter; }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            Sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void CollideWith(ICollideable character)
        {
            if (character == Shooter) return;
            character.Harm();
            Harm();
        }

        public override void Update()
        {
            Driver.UpdateMovement(MovementHandler, State);
            Angle = GetAngle(Direction);
            UpdateMoving();
        }

    }
}
