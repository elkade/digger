using MetroDigger.Gameplay.Abstract;
using MetroDigger.Logging;

namespace MetroDigger.Gameplay.Entities.Others
{
    /// <summary>
    /// Wiertło
    /// </summary>
    public class Drill : Item
    {
        /// <summary>
        /// Tworzy nowe wiertło
        /// </summary>
        public Drill()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Drill"));
        }
        /// <summary>
        /// Metoda wywoływana, gdy wierto zostaje podniesione
        /// </summary>
        /// <param name="collector">Obiekt podnoszący wirtło</param>
        public override void GetCollected(ICollector collector)
        {
            collector.HasDrill = true;
            base.GetCollected(collector);
            Logger.Log("Drill picked");
        }

    }
}
