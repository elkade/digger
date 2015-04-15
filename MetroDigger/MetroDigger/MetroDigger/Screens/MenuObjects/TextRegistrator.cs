using System;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XNA_GSM
{
    class TextRegistrator
    {
        Keys _inputKey;
        Keys _oldKey;
        string _text = String.Empty;
        private readonly double _textDelay1;
        private readonly double _textDelay2;
        double _time;

        private bool _isAccelerated = false;

        public TextRegistrator(double textDelay1, double textDelay2)
        {
            _textDelay1 = textDelay1;
            _textDelay2 = textDelay2;
        }

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

        public string Output()
        {
            return _text;
        }

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
