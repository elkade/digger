namespace MetroDigger.Screens.MenuObjects

{
    /// <summary>
    /// Kontrolka - etykieta. Nie można jej zaznaczyć - służy jedynie za wyświetlany tekst.
    /// </summary>
    class MenuLabel : MenuObject
    {
        /// <summary>
        /// Tworzy nową etykietę.
        /// </summary>
        /// <param name="text">Tekst etykiety</param>
        public MenuLabel(string text)
            : base(text)
        {
            IsSelectable = false;
        }
    }
}
