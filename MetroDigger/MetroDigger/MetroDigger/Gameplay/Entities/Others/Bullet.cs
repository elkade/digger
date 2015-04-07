using System;
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

        public Bullet(Character shooter, Vector2 direction, Vector2 position, float moveSpeed)
        {
            _moveSpeed = moveSpeed;
            _occupiedTile = shooter.OccupiedTile;
            _shooter = shooter;
            _moveSpeed = moveSpeed;
            var grc = GraphicResourceContainer.Instance;
            Position = position;
            Direction = direction;
            Animations = new[]
            {
                new Animation(grc.RedBullet[0], 1, false),
                new Animation(grc.RedBullet[1], 1, false),
            };
            ToRemove = false;
            Direction.Normalize();
            SoundManager.Instance.PlaySound("laser");
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

        public bool ToRemove { get; set; }

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
