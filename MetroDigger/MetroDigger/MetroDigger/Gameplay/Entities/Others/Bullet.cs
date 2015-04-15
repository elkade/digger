using System;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class Bullet : DynamicEntity
    {
        private readonly Character _shooter;
        public Bullet(IDriver driver, Character shooter) : base(driver, shooter.OccupiedTile, shooter.Direction)
        {
            _moveSpeed = shooter.MoveSpeed * 2;
            _occupiedTile = shooter.OccupiedTile;
            _shooter = shooter;
            var grc = MediaManager.Instance;
            Position = shooter.Position;
            Direction = shooter.Direction;
            Animations = new[]
            {
                new Animation(grc.RedBullet[0], 1, false),
                new Animation(grc.RedBullet[1], 1, false),
            };
            IsToRemove = false;
            MediaManager.Instance.PlaySound("laser");
            Sprite.PlayAnimation(Animations[1]);

            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (Hit != null)
                    Hit(this, tile2);
            };
        }

        public event Action<Bullet, Tile> Hit;

        public Character Shooter
        {
            get { return _shooter; }
        }

        public bool IsToRemove { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void StartShooting()
        {
        }

        public override void StartDrilling(Tile destination)
        {
        }
    }
}
