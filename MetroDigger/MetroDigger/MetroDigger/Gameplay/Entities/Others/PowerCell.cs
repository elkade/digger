using MetroDigger.Gameplay.Abstract;
using MetroDigger.Logging;

namespace MetroDigger.Gameplay.Entities.Others
{
    /// <summary>
    /// Bateria
    /// </summary>
    public class PowerCell : Item
    {
        /// <summary>
        /// Tworzy nową baterię
        /// </summary>
        public PowerCell()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("PowerCell"));
        }
        /// <summary>
        /// Metoda wywoływana, gdy bateria zostaje podniesiona
        /// </summary>
        /// <param name="collector">Obiekt podnoszący baterię</param>
        public override void GetCollected(ICollector collector)
        {
            collector.PowerCellsCount++;
            base.GetCollected(collector); 
            Logger.Log("Power Cell picked");

        }
    }
}
