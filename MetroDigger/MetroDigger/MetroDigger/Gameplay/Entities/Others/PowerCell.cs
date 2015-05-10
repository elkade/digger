using MetroDigger.Gameplay.Abstract;
using MetroDigger.Logging;
using MetroDigger.Manager;

namespace MetroDigger.Gameplay.Entities.Others
{
    public class PowerCell : Item
    {
        public PowerCell()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("PowerCell"));
        }

        public override void GetCollected(ICollector collector)
        {
            collector.PowerCellsCount++;
            base.GetCollected(collector); 
            Logger.Log("Power Cell picked");

        }
    }
}
