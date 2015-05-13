namespace MetroDigger.Screens.MenuObjects
{
    /// <summary>
    /// Kontrolka przej�cia do innego menu
    /// </summary>
    class MenuEntry : MenuObject
    {

        #region Initialization
        /// <summary>
        /// Tworzy now� kontrolk� przej�cia do innego menu.
        /// </summary>
        /// <param name="text">Tekst wy�wietlany przez t� kontrolk�</param>
        public MenuEntry(string text)
            : base(text)
        {
            IsSelectable = true;
        }

        #endregion


    }
}
