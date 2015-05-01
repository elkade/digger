using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Rock : Terrain
    {
        public Rock()
        {
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.Rock, 1, false, 300, MediaManager.Instance.Scale) };
            Sprite.PlayAnimation(Animations[0]);
            _accessibility = Accessibility.Rock;
        }

    }
}
