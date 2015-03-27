using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagement
{
    class TextInput
    {
        Keys inputKey;
        string text = String.Empty;
        double textDelay = 0.1d;
        double time;

        public void Input(Keys[] keys)
        {
            if (keys.Length > 0)
                foreach (var key in keys)
                {
                    if (key <= Keys.Z && key >= Keys.A || key == Keys.Back)
                    {
                        inputKey = key;
                        break;
                    }
                }
        }

        public string Output()
        {
            return text;
        }

        internal void Update(double newTime)
        {
            if (inputKey != 0)
            {
                if (newTime - time > textDelay)
                {
                    StringBuilder sb = new StringBuilder(text);
                    if (inputKey == Keys.Back)
                    {
                        if (sb.Length > 0)
                            sb.Remove(sb.Length - 1, 1);
                    }
                    else
                        sb.Append(Char.ConvertFromUtf32((int)inputKey));
                    text = sb.ToString();
                    time = newTime;
                }
                inputKey = 0;
            }
        }
    }
}
