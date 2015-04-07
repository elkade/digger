using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Others
{
    class Tunnel : Metro
    {
        public Tunnel()
        {
            var grc = GraphicResourceContainer.Instance;
            Animations = new[] {new Animation(grc.MetroTunnel, 1, false)};
            Sprite.PlayAnimation(Animations[0]);
        }
    }
}
