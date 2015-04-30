namespace XNA_GSM.Screens.MenuObjects
{
    class MenuCheckField : MenuObject
    {
        public int Number { get; private set; }

        public override string Text { get { return string.Format("{0} {1}", _text, _valueTexts[Number]); } }

        private string[] _valueTexts;

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
