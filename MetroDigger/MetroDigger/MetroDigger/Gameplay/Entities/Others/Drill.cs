using MetroDigger.Gameplay.Abstract;
using MetroDigger.Logging;
using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class Drill : Item
    {
        public Drill()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Drill"));
        }

        public override void GetCollected(ICollector collector)
        {
            collector.HasDrill = true;
            base.GetCollected(collector);
            Logger.Log("Drill picked");
        }

    }
}
