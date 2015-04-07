using System;
using MetroDigger;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNA_GSM.Screens.MenuObjects
{
    class MenuTextInput : MenuObject
    {
        private TextRegistrator _textRegistrator;

        public MenuTextInput(string text) : base(text)
        {
            _textRegistrator = new TextRegistrator(0.3d,0.03d);
            IsSelectable = true;
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
    }
}
