using System;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    /// <summary>
    /// Pocisk, który może zostać wytrzelony przez IShootable.
    /// </summary>
    public class Bullet : DynamicEntity
    {
        private readonly IShooter _shooter;
        /// <summary>
        /// Tworzy nowy pocisk
        /// </summary>
        /// <param name="driver">sterownik, zgodnie z którym porusza się pocisk</param>
        /// <param name="shooter">Obiekt, który wystrzelił pocisk</param>
        public Bullet(IDriver driver, IShooter shooter)
            : base(driver, shooter.OccupiedTile, shooter.Direction, shooter.MovementSpeed*2)
        {
            OccupiedTile = shooter.OccupiedTile;
            _shooter = shooter;
            Position = shooter.Position;
            Direction = shooter.Direction;
            IsToRemove = false;
            MediaManager.Instance.PlaySound("laser");
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Bullet", 120));

            MovementHandler.Halved += (handler, tile1, tile2) =>
            {
                if (Hit != null)
                    Hit(this, tile2);
            };
            Aggressiveness = Aggressiveness.All;
        }

        private IShooter Shooter
        {
            get { return _shooter; }
        }
        /// <summary>
        /// Zdarzenie wywoływane w momencie zderzenia z obiektem lub przejściem pomiędzy kafelkami
        /// </summary>
        public event Action<Bullet, Tile> Hit;

        /// <summary>
        /// Rysuje obiekt na planszy
        /// </summary>
        /// <param name="gameTime">Czas gry</param>
        /// <param name="spriteBatch">Obiekt XNA służący do rysowania</param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        /// <summary>
        /// Wywołuje zachowanie obiektu w momencie kolizji
        /// </summary>
        /// <param name="collideable">Obiekt, z którym zaszła kolizja.</param>
        public override void CollideWith(ICollideable collideable)
        {
            if(collideable is Bullet)
                if ((collideable as Bullet).Shooter == Shooter) return;
            if (collideable == Shooter) return;
            collideable.Harm();
            Harm();
        }

    }
}