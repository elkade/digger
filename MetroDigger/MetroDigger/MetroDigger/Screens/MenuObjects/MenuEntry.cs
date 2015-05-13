namespace MetroDigger.Screens.MenuObjects
{
    /// <summary>
    /// Kontrolka przejœcia do innego menu
    /// </summary>
    class MenuEntry : MenuObject
    {

        #region Initialization
        /// <summary>
        /// Tworzy now¹ kontrolkê przejœcia do innego menu.
        /// </summary>
        /// <param name="text">Tekst wyœwietlany przez tê kontrolkê</param>
        public MenuEntry(string text)
            : base(text)
        {
            IsSelectable = true;
        }

        #endregion


    }
}
