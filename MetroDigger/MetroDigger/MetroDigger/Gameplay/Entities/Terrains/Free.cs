using MetroDigger.Gameplay.Entities.Tiles;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Terrains
{
    class Free : Terrain
    {
        public Free()
        {
            var grc = MediaManager.Instance;
            Animations = new[]{new Animation(grc.Free,1,false,300)};
            Sprite.PlayAnimation(Animations[0]);
            _accessibility = Accessibility.Free;
        }
    }
}
