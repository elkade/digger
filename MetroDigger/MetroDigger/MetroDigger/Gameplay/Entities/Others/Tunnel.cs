using System;
using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    class Tunnel : Metro
    {
        public Tunnel()
        {
            var grc = MediaManager.Instance;
            Animations = new[] {new Animation(grc.MetroTunnel, 1, false, 0, MediaManager.Instance.Scale)};
            Sprite.PlayAnimation(Animations[0]);
        }
        public override int Clear(ref int stationsCount)
        {
            int points = 0;
            if (!IsCleared)
                points = 50;
            base.Clear(ref stationsCount);
            return points;
        }
    }
}
