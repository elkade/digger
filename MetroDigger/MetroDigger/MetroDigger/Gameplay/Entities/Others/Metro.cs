using MetroDigger.Gameplay.Entities.Terrains;

namespace MetroDigger.Gameplay.Entities.Others
{
    /// <summary>
    /// Abstrakcyjna klasa bazowa dla znaczników metra
    /// </summary>
    public abstract class Metro : StaticEntity
    {
        /// <summary>
        /// Określa, czy znacznik został już oczyszczony
        /// </summary>
        public bool IsCleared { get; set; }

        protected Metro()
        {
            IsCleared = false;
            IsVisitedInSequence = true;
            ClearedOf = Accessibility.Free;
        }
        /// <summary>
        /// Czyści znacznik
        /// </summary>
        /// <param name="stationsCount">reprezentuje liczbę stacji, które pozostały do oczyszczenia.</param>
        /// <returns></returns>
        public virtual int Clear(ref int stationsCount)
        {
            IsCleared = true;

            return 0;
        }

        private bool IsVisitedInSequence { get; set; }
        private Accessibility ClearedOf { get; set; }
    }
}
