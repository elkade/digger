namespace MetroDigger.Gameplay.Abstract
{
    /// <summary>
    /// Obiekt zdolny do aktualizacji swojego stanu.
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Aktualizuje stan obiektu.
        /// </summary>
        void Update();
        /// <summary>
        /// Określa, czy obiekt ma być nadal aktualizowany, czy usunięty.
        /// </summary>
        bool IsToRemove { get; set; }
    }
}
