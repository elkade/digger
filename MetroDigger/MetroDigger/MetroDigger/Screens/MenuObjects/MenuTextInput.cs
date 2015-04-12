using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XNA_GSM;
using XNA_GSM.Screens.MenuObjects;

namespace MetroDigger.Screens.MenuObjects
{
    class MenuTextInput : MenuObject
    {
        private TextRegistrator _textRegistrator;

        private string _initText;

        public MenuTextInput(string text) : base(text)
        {
            _textRegistrator = new TextRegistrator(0.3d,0.03d);
            IsSelectable = true;
            _initText = text;
        }

        public override void HandleInput(InputHandler input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            KeyboardState keyboardState = input.CurrentKeyboardState;
            _textRegistrator.Input(keyboardState.GetPressedKeys());
            base.HandleInput(input);
        }

        public override void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            _textRegistrator.Update(gameTime.TotalGameTime.TotalSeconds);
            Text = _textRegistrator.Output();
            base.Update(screen, isSelected, gameTime);
        }

        public override string Text {
            get {
                return _text == String.Empty ? _initText : _text;
            }
        }
    }
}
