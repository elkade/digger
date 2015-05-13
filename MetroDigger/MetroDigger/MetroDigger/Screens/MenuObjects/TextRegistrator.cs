using System;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace MetroDigger.Screens.MenuObjects
{
    /// <summary>
    /// Narzędzie służące do rejestracji wpisywanego tekstu i konwersji na znaki gotowe do wyświetlenia.
    /// </summary>
    class TextRegistrator
    {
        Keys _inputKey;
        Keys _oldKey;
        string _text = String.Empty;
        private readonly double _textDelay1;
        private readonly double _textDelay2;
        double _time;

        private bool _isAccelerated = false;
        /// <summary>
        /// Tworzy nowy rejestrator
        /// </summary>
        /// <param name="textDelay1">Opóźnienie po pierwszym wciśnięciu klawisza</param>
        /// <param name="textDelay2">Opóźnienie po wciśnięciu klawisza pomiędzy zapisem kolejnych liter</param>
        public TextRegistrator(double textDelay1, double textDelay2)
        {
            _textDelay1 = textDelay1;
            _textDelay2 = textDelay2;
        }
        /// <summary>
        /// Podaje zestaw klawiszy klawiatury do rejestratora w celu konwersji i zapisu.
        /// </summary>
        /// <param name="keys">Tablica klawiszy</param>
        public void Input(Keys[] keys)
        {
            if (keys.Length > 0)
                foreach (var key in keys)
                {
                    if (key <= Keys.Z && key >= Keys.A || key == Keys.Back)
                    {
                        _inputKey = key;
                        break;
                    }
                    _inputKey = 0;
                }
            else
                _inputKey = 0;
        }
        /// <summary>
        /// Zwraca zapisany tekst
        /// </summary>
        /// <returns>zapisany tekst.</returns>
        public string Output()
        {
            return _text;
        }
        /// <summary>
        /// Aktualizuje wpisany tekst.
        /// </summary>
        /// <param name="newTime">Aktualny czas gry.</param>
        internal void Update(double newTime)
        {
            if (_inputKey != Keys.None)
            {
                if (_oldKey != _inputKey || newTime - _time > _textDelay1 || (_isAccelerated && newTime - _time > _textDelay2))
                {
                    StringBuilder sb = new StringBuilder(_text);
                    if (_inputKey == Keys.Back)
                    {
                        if (sb.Length > 0)
                            sb.Remove(sb.Length - 1, 1);
                    }
                    else
                        sb.Append(Char.ConvertFromUtf32((int) _inputKey));
                    _text = sb.ToString();
                    _time = newTime;
                    _isAccelerated = _oldKey == _inputKey;
                }
            }
            else
            {
                _time = newTime;
                _isAccelerated = false;
            }
            _oldKey = _inputKey;
        }
    }
}
