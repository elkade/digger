namespace XNA_GSM.Screens.MenuObjects
{
    class MenuCheckField : MenuObject
    {
        public bool IsOn { get; private set; }

        public override string Text { get { return string.Format("{0} {1}", _text, _valueTexts[IsOn ? 0 : 1]); } }

        private string[] _valueTexts;

        public MenuCheckField(string text, string textOn, string textOff, bool isOn) : base(text)
        {
            _valueTexts = new []{textOn, textOff};
            IsSelectable = true;
            IsOn = isOn;
            Selected += (sender, args) => IsOn = !IsOn;
        }

    }
}
