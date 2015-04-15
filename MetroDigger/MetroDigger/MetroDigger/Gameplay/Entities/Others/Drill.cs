using MetroDigger.Gameplay.Entities.Characters;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class Drill : Item
    {
        public Drill()
        {
            var grc = MediaManager.Instance;
            Animations = new[] { new Animation(grc.Drill, 1, false, 300) };
            Sprite.PlayAnimation(Animations[0]);
        }

        public override void GetCollected(ICollector collector)
        {
            collector.HasDrill = true;
            base.GetCollected(collector);
        }

    }
}
