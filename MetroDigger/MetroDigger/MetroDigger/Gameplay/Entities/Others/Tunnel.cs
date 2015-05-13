using MetroDigger.Logging;

namespace MetroDigger.Gameplay.Entities.Others
{
    /// <summary>
    /// Reprezentuje znacznik tunelu matra
    /// </summary>
    class Tunnel : Metro
    {
        /// <summary>
        /// Tworzy nowy znacznik tunelu metra
        /// </summary>
        public Tunnel()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Tunnel"));
        }
        /// <summary>
        /// Czyści znacznik
        /// </summary>
        /// <param name="stationsCount">reprezentuje liczbę stacji, które pozostały do oczyszczenia.</param>
        /// <returns></returns>
        public override int Clear(ref int stationsCount)
        {
            int points = 0;
            if (!IsCleared)
                points = 50;
            base.Clear(ref stationsCount);
            Logger.Log("Tunnel cleared");

            return points;
        }
    }
}
