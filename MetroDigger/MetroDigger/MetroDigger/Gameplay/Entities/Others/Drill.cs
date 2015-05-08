using MetroDigger.Gameplay.Abstract;
using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class Drill : Item
    {
        public Drill()
        {
            var grc = MediaManager.Instance;
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Drill"));
        }

        public override void GetCollected(ICollector collector)
        {
            collector.HasDrill = true;
            base.GetCollected(collector);
        }

    }
}
