using MetroDigger.Logging;

namespace MetroDigger.Gameplay.Entities.Others
{
    /// <summary>
    /// Reprezentuje znacznik stacji metra
    /// </summary>
    internal class Station : Metro
    {
        /// <summary>
        /// Tworzy nowy znacznik stacji metra
        /// </summary>
        public Station()
        {
            AnimationPlayer.PlayAnimation(Mm.GetStaticAnimation("Station"));
        }
        /// <summary>
        /// Czyści znacznik
        /// </summary>
        /// <param name="stationsCount">reprezentuje liczbę stacji, które pozostały do oczyszczenia.
        /// Odejmuje 1 od tej liczby</param>
        /// <returns></returns>
        public override int Clear(ref int stationsCount)
        {
            if (!IsCleared)
            {
                IsCleared = true;
                stationsCount--;
            }
            base.Clear(ref stationsCount);
            Logger.Log("Station cleared");

            return 0;
        }
    }
}