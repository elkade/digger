using System;
using MetroDigger.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MetroDigger.Screens.MenuObjects
{
    /// <summary>
    /// Kontrolka umożliwiająca wpisywanie tekstu do gry. Używa rejestratora tekstu.
    /// </summary>
    class MenuTextInput : MenuObject
    {
        private readonly TextRegistrator _textRegistrator;

        private readonly string _initText;

        /// <summary>
        /// Tworzy nową kontrolkę tekstową
        /// </summary>
        /// <param name="text">tekst do wyświetlenia, gdy kontrolka jest pusta</param>
        public MenuTextInput(string text) : base(text)
        {
            _textRegistrator = new TextRegistrator(0.3d,0.03d);
            IsSelectable = true;
            _initText = text;
        }
        /// <summary>
        /// Obsługuje wciśnięte klawisze
        /// </summary>
        /// <param name="input">InputManager dostarczający aktualnie wciśniętych klawiszy</param>
        public override void HandleInput(InputHandler input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            KeyboardState keyboardState = input.CurrentKeyboardState;
            _textRegistrator.Input(keyboardState.GetPressedKeys());
            base.HandleInput(input);
        }
        /// <summary>
        /// Aktualizuje stan kontrolki i wpisanego tekstu.
        /// </summary>
        /// <param name="isSelected">Czy kontrolka jest zaznaczona</param>
        /// <param name="gameTime">Aktualny czas gry</param>
        public override void Update(bool isSelected, GameTime gameTime)
        {
            if (isSelected)
            {
                _textRegistrator.Update(gameTime.TotalGameTime.TotalSeconds);
                Text = _textRegistrator.Output();
            }
            base.Update(isSelected, gameTime);
        }
        /// <summary>
        /// A=Tekst aktualnie wypisany przez kontrolkę.
        /// </summary>
        public override string Text {
            get {
                return _text == String.Empty ? _initText : _text;
            }
        }
    }
}
