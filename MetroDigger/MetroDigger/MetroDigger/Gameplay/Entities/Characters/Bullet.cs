using System;
using MetroDigger.Gameplay.Abstract;
using MetroDigger.Gameplay.Drivers;
using MetroDigger.Gameplay.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Characters
{
    public class Bullet : DynamicEntity
    {
        private readonly IShooter _shooter;

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

        public event Action<Bullet, Tile> Hit;


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive);
            AnimationPlayer.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Color.White, Angle);
            spriteBatch.End();
            spriteBatch.Begin();
        }

        public override void CollideWith(ICollideable character)
        {
            if(character is Bullet)
                if ((character as Bullet).Shooter == Shooter) return;
            if (character == Shooter) return;
            character.Harm();
            Harm();
        }

    }
}