namespace MetroDigger.Screens.MenuObjects
{
    /// <summary>
    /// Kontrolka wyboru opcji. Pozwala na wygór jednej z opcji.
    /// </summary>
    class MenuCheckField : MenuObject
    {
        /// <summary>
        /// Numer aktualnie wybranej opcji
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Wyświetlany tekst związany z aktualnie wybraną opcją.
        /// </summary>
        public override string Text { get { return string.Format("{0} {1}", _text, _valueTexts[Number]); } }

        private readonly string[] _valueTexts;
        /// <summary>
        /// Towrzy nową kontrolkę wyboru opcji
        /// </summary>
        /// <param name="text">nazwa kontrolki</param>
        /// <param name="texts">lista nazw opcji</param>
        /// <param name="currentNumber">startowy numer wybranej opcji</param>
        public MenuCheckField(string text,string[] texts, int currentNumber)
            : base(text)
        {
            _valueTexts = texts;
            IsSelectable = true;
            Number = currentNumber;
            Selected += (sender, args) => Number = (Number + 1) % _valueTexts.Length;
        }

    }
}
