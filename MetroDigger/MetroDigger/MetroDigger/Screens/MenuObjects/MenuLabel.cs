using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroDigger.Screens.MenuObjects;

namespace XNA_GSM.Screens.MenuObjects
{
    class MenuLabel : MenuObject
    {
        public MenuLabel(string text)
            : base(text)
        {
            IsSelectable = false;
        }
    }
}
